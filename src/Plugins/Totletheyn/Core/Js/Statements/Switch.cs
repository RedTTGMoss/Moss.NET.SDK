using System;
using System.Collections.Generic;
using System.Linq;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Expressions;

namespace Totletheyn.Core.Js.Statements;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class SwitchCase
{
    internal int index;
    internal CodeNode statement;

    public int Index => index;
    public CodeNode Statement => statement;
}

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class Switch : CodeNode
{
    private CodeNode _image;

    internal Switch(CodeNode[] body)
    {
        Body = body;
    }

    public FunctionDefinition[] Functions { get; private set; }

    public CodeNode[] Body { get; }

    public SwitchCase[] Cases { get; private set; }

    public CodeNode Image => _image;

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
        var i = index;
        if (!Parser.Validate(state.Code, "switch (", ref i) && !Parser.Validate(state.Code, "switch(", ref i))
            return null;

        while (Tools.IsWhiteSpace(state.Code[i]))
            i++;

        var body = new List<CodeNode>();
        var funcs = new List<FunctionDefinition>();
        var cases = new List<SwitchCase>();
        CodeNode result = null;
        cases.Add(new SwitchCase { index = int.MaxValue });
        state.AllowBreak.Push(true);
        var oldVariablesCount = state.Variables.Count;
        VariableDescriptor[] vars = null;
        state.LexicalScopeLevel++;
        try
        {
            var image = ExpressionTree.Parse(state, ref i);

            if (state.Code[i] != ')')
                ExceptionHelper.Throw(new SyntaxError("Expected \")\" at + " +
                                                      CodeCoordinates.FromTextPosition(state.Code, i, 0)));

            do
            {
                i++;
            } while (Tools.IsWhiteSpace(state.Code[i]));

            if (state.Code[i] != '{')
                ExceptionHelper.Throw(new SyntaxError("Expected \"{\" at + " +
                                                      CodeCoordinates.FromTextPosition(state.Code, i, 0)));

            do
            {
                i++;
            } while (Tools.IsWhiteSpace(state.Code[i]));

            while (state.Code[i] != '}')
            {
                do
                {
                    if (Parser.Validate(state.Code, "case", i) && Parser.IsIdentifierTerminator(state.Code[i + 4]))
                    {
                        i += 4;
                        while (Tools.IsWhiteSpace(state.Code[i]))
                            i++;
                        var sample = ExpressionTree.Parse(state, ref i);
                        if (state.Code[i] != ':')
                            ExceptionHelper.Throw(new SyntaxError("Expected \":\" at + " +
                                                                  CodeCoordinates.FromTextPosition(state.Code, i, 0)));
                        i++;
                        cases.Add(new SwitchCase { index = body.Count, statement = sample });
                    }
                    else if (Parser.Validate(state.Code, "default", i) &&
                             Parser.IsIdentifierTerminator(state.Code[i + 7]))
                    {
                        i += 7;
                        while (Tools.IsWhiteSpace(state.Code[i]))
                            i++;
                        if (cases[0].index != int.MaxValue)
                            ExceptionHelper.Throw(new SyntaxError("Duplicate default case in switch at " +
                                                                  CodeCoordinates.FromTextPosition(state.Code, i, 0)));
                        if (state.Code[i] != ':')
                            ExceptionHelper.Throw(new SyntaxError("Expected \":\" at + " +
                                                                  CodeCoordinates.FromTextPosition(state.Code, i, 0)));
                        i++;
                        cases[0].index = body.Count;
                    }
                    else
                    {
                        break;
                    }

                    while (Tools.IsWhiteSpace(state.Code[i]) || state.Code[i] == ';')
                        i++;
                } while (true);

                if (cases.Count == 1 && cases[0].index == int.MaxValue)
                    ExceptionHelper.Throw(new SyntaxError("Switch statement must contain cases. " +
                                                          CodeCoordinates.FromTextPosition(state.Code, index, 0)));

                var t = Parser.Parse(state, ref i, 0);
                if (t == null)
                    continue;

                body.Add(t);
                while (Tools.IsWhiteSpace(state.Code[i]) || state.Code[i] == ';')
                    i++;
            }

            state.AllowBreak.Pop();
            i++;
            var pos = index;
            index = i;
            result = new Switch(body.ToArray())
            {
                Functions = funcs.ToArray(),
                Cases = cases.ToArray(),
                _image = image,
                Position = pos,
                Length = index - pos
            };
            vars = CodeBlock.extractVariables(state, oldVariablesCount);
        }
        finally
        {
            state.LexicalScopeLevel--;
        }

        return new CodeBlock([result])
        {
            _variables = vars,
            Position = result.Position,
            Length = result.Length
        };
    }

    public override JSValue Evaluate(Context context)
    {
#if DEBUG
        if (Functions != null)
            throw new InvalidOperationException();
#endif
        JSValue imageValue = null;
        var caseIndex = 1;
        var lineIndex = Cases[0].index;

        var frame = ExceptionHelper.GetStackFrame(context, false);

        frame.CodeNode = _image;

        if (context._executionMode >= ExecutionMode.Resume)
        {
            var sdata = context.SuspendData[this] as SuspendData;
            if (sdata.imageValue == null)
            {
                if (context._debugging)
                    context.raiseDebugger(_image);

                imageValue = _image.Evaluate(context);
            }
            else
            {
                imageValue = sdata.imageValue;
            }

            caseIndex = sdata.caseIndex;
            lineIndex = sdata.lineIndex;
        }
        else
        {
            if (context._debugging)
                context.raiseDebugger(_image);

            imageValue = _image.Evaluate(context);
        }

        if (context._executionMode == ExecutionMode.Suspend)
        {
            context.SuspendData[this] = new SuspendData { caseIndex = 1 };
            return null;
        }

        for (; caseIndex < Cases.Length; caseIndex++)
        {
            frame.CodeNode = Cases[caseIndex].Statement;

            if (context._debugging)
                context.raiseDebugger(Cases[caseIndex].statement);

            var cseResult = Cases[caseIndex].statement.Evaluate(context);
            if (context._executionMode == ExecutionMode.Suspend)
            {
                context.SuspendData[this] = new SuspendData
                {
                    caseIndex = caseIndex,
                    imageValue = imageValue
                };
                return null;
            }

            if (StrictEqual.Check(imageValue, cseResult))
            {
                lineIndex = Cases[caseIndex].index;
                caseIndex = Cases.Length;
                break;
            }
        }

        for (; lineIndex < Body.Length; lineIndex++)
        {
            if (Body[lineIndex] == null)
                continue;

            frame.CodeNode = Body[lineIndex];

            context._lastResult = Body[lineIndex].Evaluate(context) ?? context._lastResult;
            if (context._executionMode != ExecutionMode.Regular)
            {
                if (context._executionMode == ExecutionMode.Break && context._executionInfo == null)
                    context._executionMode = ExecutionMode.Regular;
                else if (context._executionMode == ExecutionMode.Suspend)
                    context.SuspendData[this] = new SuspendData
                    {
                        caseIndex = caseIndex,
                        imageValue = imageValue,
                        lineIndex = lineIndex
                    };

                return null;
            }
        }

        return null;
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        if (expressionDepth < 1)
            throw new InvalidOperationException();

        Parser.Build(ref _image, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);

        for (var i = 0; i < Body.Length; i++)
            Parser.Build(ref Body[i], 1, variables, codeContext | CodeContext.Conditional, message, stats, opts);

        for (var i = 0; Functions != null && i < Functions.Length; i++)
        {
            CodeNode stat = Functions[i];
            Parser.Build(ref stat, 1, variables, codeContext, message, stats, opts);
        }

        Functions = null;

        for (var i = 1; i < Cases.Length; i++)
            Parser.Build(ref Cases[i].statement, 2, variables, codeContext, message, stats, opts);

        return false;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
        var res = new List<CodeNode>
        {
            _image
        };

        res.AddRange(Body);

        if (Functions != null && Functions.Length > 0)
            res.AddRange(Functions);

        if (Cases.Length > 0)
            res.AddRange(from c in Cases where c != null select c.statement);

        res.RemoveAll(x => x == null);
        return res.ToArray();
    }

    public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message,
        Options opts, FunctionInfo stats)
    {
        _image.Optimize(ref _image, owner, message, opts, stats);
        for (var i = 1; i < Cases.Length; i++)
            Cases[i].statement.Optimize(ref Cases[i].statement, owner, message, opts, stats);
        for (var i = Body.Length; i-- > 0;)
        {
            if (Body[i] == null)
                continue;

            var cn = Body[i];
            cn.Optimize(ref cn, owner, message, opts, stats);
            Body[i] = cn;
        }
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        var res = "switch (" + _image + ") {" + Environment.NewLine;
        var replp = Environment.NewLine;
        var replt = Environment.NewLine + "  ";
        for (var i = Body.Length; i-- > 0;)
        {
            for (var j = 0; j < Cases.Length; j++)
                if (Cases[j] != null && Cases[j].index == i)
                    res += "case " + Cases[j].statement + ":" + Environment.NewLine;

            var lc = Body[i].ToString().Replace(replp, replt);
            res += "  " + lc + (lc[lc.Length - 1] != '}' ? ";" + Environment.NewLine : Environment.NewLine);
        }

        if (Functions != null)
            for (var i = 0; i < Functions.Length; i++)
            {
                var func = Functions[i].ToString().Replace(replp, replt);
                res += "  " + func + Environment.NewLine;
            }

        return res + "}";
    }

    public override void Decompose(ref CodeNode self)
    {
        for (var i = 0; i < Cases.Length; i++)
            if (Cases[i].statement != null)
                Cases[i].statement.Decompose(ref Cases[i].statement);

        for (var i = 0; i < Body.Length; i++) Body[i].Decompose(ref Body[i]);
    }

    public override void RebuildScope(FunctionInfo functionInfo,
        Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
    {
        _image.RebuildScope(functionInfo, transferedVariables, scopeBias);

        for (var i = 0; i < Cases.Length; i++)
            if (Cases[i].statement != null)
                Cases[i].statement.RebuildScope(functionInfo, transferedVariables, scopeBias);

        for (var i = 0; i < Body.Length; i++) Body[i]?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    private sealed class SuspendData
    {
        public int caseIndex;
        public JSValue imageValue;
        public int lineIndex;
    }
}