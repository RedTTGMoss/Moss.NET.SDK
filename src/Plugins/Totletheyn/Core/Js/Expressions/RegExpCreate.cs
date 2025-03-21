using System;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Statements;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class RegExpExpression : Expression
{
    private string flags;
    private string pattern;

    public RegExpExpression(string pattern, string flags)
    {
        this.pattern = pattern;
        this.flags = flags;
    }

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    protected internal override PredictedType ResultType => PredictedType.Object;

    public static CodeNode Parse(ParseInfo state, ref int position)
    {
        var i = position;
        if (!Parser.ValidateRegex(state.Code, ref i, false))
            return null;

        var value = state.Code.Substring(position, i - position);
        position = i;

        state.Code = Parser.RemoveComments(state.SourceCode, position);

        var s = value.LastIndexOf('/') + 1;
        var flags = value.Substring(s);
        try
        {
            return new RegExpExpression(value.Substring(1, s - 2), flags); // объекты должны быть каждый раз разные
        }
        catch (Exception e)
        {
            if (state.Message != null)
                state.Message(MessageLevel.Error, i - value.Length, value.Length,
                    string.Format(Strings.InvalidRegExp, value));

            return new ExpressionWrapper(new Throw(e));
        }
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
        return null;
    }

    public override JSValue Evaluate(Context context)
    {
        return new RegExp(pattern, flags);
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "/" + pattern + "/" + flags;
    }
}