using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class New : Expression
{
    internal New(Call call)
        : base(call, null, false)
    {
    }

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => PredictedType.Object;

    public static CodeNode Parse(ParseInfo state, ref int index)
    {
        var i = index;
        if (!Parser.Validate(state.Code, "new", ref i) || !Parser.IsIdentifierTerminator(state.Code[i]))
            return null;

        Tools.SkipSpaces(state.Code, ref i);

        var result = ExpressionTree.Parse(state, ref i, true, false, true);
        if (result == null)
        {
            var cord = CodeCoordinates.FromTextPosition(state.Code, i, 0);
            ExceptionHelper.Throw(new SyntaxError("Invalid prefix operation. " + cord));
        }

        if (result is Call)
        {
            result = new New(result as Call) { Position = index, Length = i - index };
        }
        else
        {
            if (state.Message != null)
                state.Message(MessageLevel.Warning, index, 0, "Missed brackets in a constructor invocation.");
            result = new New(new Call(result, new Expression[0]) { Position = result.Position, Length = result.Length })
                { Position = index, Length = i - index };
        }

        index = i;
        return result;
    }

    public override JSValue Evaluate(Context context)
    {
        throw new InvalidOperationException();
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        if (message != null && expressionDepth <= 1)
            message(MessageLevel.Warning, Position, 0, "Do not use NewOperator for side effect");

        (_left as Call)._callMode = CallMode.Construct;
        _this = _left;

        return true;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "new " + _left;
    }
}