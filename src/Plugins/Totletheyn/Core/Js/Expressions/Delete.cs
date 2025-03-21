using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class Delete : Expression
{
    public Delete(Expression first)
        : base(first, null, false)
    {
    }

    protected internal override PredictedType ResultType => PredictedType.Bool;

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => false;

    public override JSValue Evaluate(Context context)
    {
        var temp = _left.Evaluate(context);
        if (temp._valueType < JSValueType.Undefined)
            return true;

        if ((temp._attributes & JSValueAttributesInternal.Argument) != 0) return false;

        if ((temp._attributes & JSValueAttributesInternal.DoNotDelete) == 0)
        {
            if ((temp._attributes & JSValueAttributesInternal.SystemObject) == 0)
            {
                temp._valueType = JSValueType.NotExists;
                temp._oValue = null;
            }

            return true;
        }

        if (context._strict) ExceptionHelper.Throw(new TypeError("Cannot delete property \"" + _left + "\"."));

        return false;
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        if (base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts))
            return true;
        if (_left is Variable variable)
        {
            if ((codeContext & CodeContext.Strict) != 0)
                ExceptionHelper.Throw(new SyntaxError("Cannot delete variable in strict mode"));

            if (variable._throwMode is not ThrowMode.ForceThrow)
                variable._throwMode = ThrowMode.Suspend;
        }

        var gme = _left as Property;
        if (gme != null)
        {
            _this = new DeleteProperty(gme._left, gme._right);
            return false;
        }

        var f = _left as VariableReference ?? (_left is AssignmentOperatorCache
            ? (_left as AssignmentOperatorCache).Source as VariableReference
            : null);
        if (f != null)
        {
            if (f.Descriptor.IsDefined && message != null)
                message(MessageLevel.Warning, Position, Length,
                    "Tring to delete defined variable." + ((codeContext & CodeContext.Strict) != 0
                        ? " In strict mode it cause exception."
                        : " It is not allowed"));
            (f.Descriptor.assignments ??
             (f.Descriptor.assignments = new List<Expression>())).Add(this);
        }

        return false;
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "delete " + _left;
    }
}