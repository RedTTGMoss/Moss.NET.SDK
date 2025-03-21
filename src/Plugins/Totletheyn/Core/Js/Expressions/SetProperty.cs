using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Totletheyn.Core.Js.Core;

namespace Totletheyn.Core.Js.Expressions;

#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class SetProperty : Expression
{
    private JSValue _cachedMemberName;
    private JSValue _propertyNameTempContainer;
    private JSValue _sourceTempContainer;

    internal SetProperty(Expression obj, Expression fieldName, Expression value)
        : base(obj, fieldName, true)
    {
        if (fieldName is Constant)
            _cachedMemberName = fieldName.Evaluate(null);
        else
            _propertyNameTempContainer = new JSValue();
        Value = value;
        _sourceTempContainer = new JSValue();
    }

    public Expression Source => _left;
    public Expression FieldName => _right;
    public Expression Value { get; private set; }

    protected internal override bool ContextIndependent => false;

    internal override bool ResultInTempContainer => true;

    public override JSValue Evaluate(Context context)
    {
        JSValue sjso = null;
        JSValue source = null;

        var tc = _tempContainer;
        var pntc = _propertyNameTempContainer;
        var stc = _sourceTempContainer;

        source = _left.Evaluate(context);
        if (source._valueType >= JSValueType.Object
            && source._oValue != null
            && source._oValue != source
            && (sjso = source._oValue as JSValue) != null
            && sjso._valueType >= JSValueType.Object)
        {
            source = sjso;
        }
        else
        {
            if (_sourceTempContainer == null)
                _sourceTempContainer = new JSValue();

            _sourceTempContainer.Assign(source);
            source = _sourceTempContainer;
            _sourceTempContainer = null;
        }

        var propertyName = _cachedMemberName;
        if (propertyName == null)
        {
            if (_propertyNameTempContainer == null)
                _propertyNameTempContainer = new JSValue();

            propertyName = safeGet(_propertyNameTempContainer, _right, context);
            _propertyNameTempContainer = null;
        }

        if (_tempContainer == null)
            _tempContainer = new JSValue();

        var value = safeGet(_tempContainer, Value, context);
        _tempContainer = null;

        source.SetProperty(
            propertyName,
            value,
            context._strict);

        context._objectSource = null;

        _tempContainer = tc;
        _propertyNameTempContainer = pntc;
        _sourceTempContainer = stc;

        return value;
    }

#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static JSValue safeGet(JSValue temp, CodeNode source, Context context)
    {
        temp.Assign(source.Evaluate(context));
        return temp;
    }

    public override bool Build(ref CodeNode _this, int expressionDepth,
        Dictionary<string, VariableDescriptor> variables, CodeContext codeContext,
        InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
    {
        _codeContext = codeContext;
        return false;
    }

    public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message,
        Options opts, FunctionInfo stats)
    {
        var cn = Value as CodeNode;
        Value.Optimize(ref cn, owner, message, opts, stats);
        Value = cn as Expression;
        base.Optimize(ref _this, owner, message, opts, stats);
    }

    public override void RebuildScope(FunctionInfo functionInfo,
        Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
    {
        base.RebuildScope(functionInfo, transferedVariables, scopeBias);

        Value.RebuildScope(functionInfo, transferedVariables, scopeBias);
    }

    public override T Visit<T>(Visitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    protected internal override CodeNode[] GetChildrenImpl()
    {
        return [_left, _right, Value];
    }

    public override string ToString()
    {
        var res = _left.ToString();
        var i = 0;
        var cn = _right as Constant;
        if (_right is Constant
            && cn.value.ToString().Length > 0
            && Parser.ValidateName(cn.value.ToString(), ref i, true))
            res += "." + cn.value;
        else
            res += "[" + _right + "]";
        return res + " = " + Value;
    }
}