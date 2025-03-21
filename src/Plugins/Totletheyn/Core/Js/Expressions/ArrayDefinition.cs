using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Statements;
using Array = Totletheyn.Core.Js.BaseLibrary.Array;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class ArrayDefinition : Expression
{
    private static JSValue writableNotExists;

    private ArrayDefinition()
    {
    }

    public Expression[] Elements { get; private set; }

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override bool NeedDecompose
    {
        get
        {
            for (var i = 0; i < Elements.Length; i++)
                if (Elements[i] != null && Elements[i].NeedDecompose)
                    return true;
            return false;
        }
    }

    internal static CodeNode Parse(ParseInfo state, ref int index)
    {
        var i = index;
        if (state.Code[index] != '[')
            throw new ArgumentException("Syntax error. Expected '['");
        do
        {
            i++;
        } while (Tools.IsWhiteSpace(state.Code[i]));

        var elms = new List<Expression>();
        while (state.Code[i] != ']')
        {
            var start = i;
            var spread = Parser.Validate(state.Code, "...", ref i);
            if (state.Code[i] == ',')
            {
                if (spread)
                    ExceptionHelper.ThrowSyntaxError("Expected expression", state.Code, i);
                elms.Add(null);
            }
            else
            {
                elms.Add(ExpressionTree.Parse(state, ref i, false, false));
            }

            if (spread)
                elms[elms.Count - 1] = new Spread(elms[elms.Count - 1]) { Position = start, Length = i - start };
            while (Tools.IsWhiteSpace(state.Code[i]))
                i++;
            if (state.Code[i] == ',')
                do
                {
                    i++;
                } while (Tools.IsWhiteSpace(state.Code[i]));
            else if (state.Code[i] != ']')
                ExceptionHelper.ThrowSyntaxError("Expected ']'", state.Code, i);
        }

        i++;
        var pos = index;
        index = i;
        return new ArrayDefinition
        {
            Elements = elms.ToArray(),
            Position = pos,
            Length = index - pos
        };
    }

    public override JSValue Evaluate(Context context)
    {
        var length = Elements.Length;
        var res = new Array(length);
        if (length > 0)
            for (int sourceIndex = 0, targetIndex = 0; sourceIndex < length; sourceIndex++, targetIndex++)
                if (Elements[sourceIndex] != null)
                {
                    var e = Elements[sourceIndex].Evaluate(context);
                    if (e._valueType == JSValueType.SpreadOperatorResult)
                    {
                        var spreadArray = e._oValue as IList<JSValue>;
                        for (var i = 0; i < spreadArray.Count; i++, targetIndex++)
                            res._data[targetIndex] = spreadArray[i].CloneImpl(false);
                        targetIndex--;
                    }
                    else
                    {
                        e = e.CloneImpl(true);
                        e._attributes = 0;
                        res._data[targetIndex] = e;
                    }
                }
                else
                {
                    if (writableNotExists == null)
                        writableNotExists = new JSValue
                        {
                            _valueType = JSValueType.NotExistsInObject,
                            _attributes = JSValueAttributesInternal.SystemObject
                        };
                    res._data[targetIndex] = writableNotExists;
                }

        return res;
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
        return Elements;
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        _codeContext = codeContext;

        for (var i = 0; i < Elements.Length; i++)
            Parser.Build(ref Elements[i], 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
        return false;
    }

    public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message,
        Options opts, FunctionInfo stats)
    {
        for (var i = Elements.Length; i-- > 0;)
        {
            var cn = Elements[i] as CodeNode;
            if (cn != null)
            {
                cn.Optimize(ref cn, owner, message, opts, stats);
                Elements[i] = cn as Expression;
            }
        }
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override void Decompose(ref Expression self, IList<CodeNode> result)
    {
        var lastDecomposeIndex = -1;
        for (var i = 0; i < Elements.Length; i++)
        {
            Elements[i].Decompose(ref Elements[i], result);
            if (Elements[i].NeedDecompose) lastDecomposeIndex = i;
        }

        for (var i = 0; i < lastDecomposeIndex; i++)
            if (!(Elements[i] is ExtractStoredValue))
            {
                result.Add(new StoreValue(Elements[i], false));
                Elements[i] = new ExtractStoredValue(Elements[i]);
            }
    }

    public override void RebuildScope(FunctionInfo functionInfo,
        Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
    {
        base.RebuildScope(functionInfo, transferedVariables, scopeBias);

        for (var i = 0; i < Elements.Length; i++)
            Elements[i]?.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override string ToString()
    {
        var res = "[";
        for (var i = 0; i < Elements.Length; i++)
        {
            res += Elements[i];
            if (i + 1 < Elements.Length)
                res += ", ";
        }

        return res + ']';
    }
}