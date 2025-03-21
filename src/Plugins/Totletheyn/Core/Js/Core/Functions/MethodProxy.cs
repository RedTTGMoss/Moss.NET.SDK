using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core.Interop;
using Totletheyn.Core.Js.Expressions;
using Array = Totletheyn.Core.Js.BaseLibrary.Array;

namespace Totletheyn.Core.Js.Core.Functions;

[Flags]
internal enum ConvertArgsOptions
{
    Default = 0,
    ThrowOnError = 1,
    StrictConversion = 2,
    AllowDefaultValues = 4
}

internal delegate object WrapperDelegate(object target, Context initiator, Expression[] arguments,
    Arguments argumentsObject);

[Prototype(typeof(Function), true)]
internal sealed class MethodProxy : Function
{
    private static readonly Dictionary<MethodBase, WrapperDelegate> _wrapperCache = new();
    private static readonly MethodInfo _argumentsGetItemMethod = typeof(Arguments).GetMethod("get_Item", [typeof(int)]);

    internal readonly WrapperDelegate _fastWrapper;

    private readonly bool _forceInstance;
    internal readonly object _hardTarget;
    internal readonly MethodBase _method;
    internal readonly ParameterInfo[] _parameters;
    private readonly ConvertValueAttribute[] _paramsConverters;
    private readonly RestPrmsConverter _restPrmsArrayCreator;
    internal readonly ConvertValueAttribute _returnConverter;
    private readonly bool _strictConversion;

    public MethodProxy(Context context, MethodBase methodBase)
        : this(context, methodBase, null)
    {
    }

    public MethodProxy(Context context, MethodBase methodBase, object hardTarget)
        : base(context)
    {
        _method = methodBase;
        _hardTarget = hardTarget;
        _parameters = methodBase.GetParameters();
        _strictConversion = methodBase.IsDefined(typeof(StrictConversionAttribute), true);
        name = methodBase.Name;

        if (methodBase.IsDefined(typeof(JavaScriptNameAttribute), false))
        {
            name =
                (methodBase.GetCustomAttributes(typeof(JavaScriptNameAttribute), false).First() as
                    JavaScriptNameAttribute).Name;
            if (name.StartsWith("@@"))
                name = name.Substring(2);
        }

        if (_length == null)
            _length = new Number(0)
            {
                _attributes = JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.DoNotDelete |
                              JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.SystemObject
            };

        if (methodBase.IsDefined(typeof(ArgumentsCountAttribute), false))
        {
            var argsCountAttribute =
                methodBase.GetCustomAttributes(typeof(ArgumentsCountAttribute), false).First() as
                    ArgumentsCountAttribute;
            _length._iValue = argsCountAttribute.Count;
        }
        else
        {
            _length._iValue = _parameters.Length;
        }

        for (var i = 0; i < _parameters.Length; i++)
            if (_parameters[i].IsDefined(typeof(ConvertValueAttribute), false))
            {
                var t = _parameters[i].GetCustomAttributes(typeof(ConvertValueAttribute), false).First();
                if (_paramsConverters == null)
                    _paramsConverters = new ConvertValueAttribute[_parameters.Length];
                _paramsConverters[i] = t as ConvertValueAttribute;
            }

        var methodInfo = methodBase as MethodInfo;
        if (methodInfo != null)
        {
            _returnConverter =
                methodInfo.ReturnParameter.GetCustomAttribute(typeof(ConvertValueAttribute), false) as
                    ConvertValueAttribute;

            _forceInstance = methodBase.IsDefined(typeof(InstanceMemberAttribute), false);

            if (_forceInstance)
            {
                if (!methodInfo.IsStatic
                    || _parameters.Length == 0
                    || _parameters[0].ParameterType != typeof(JSValue))
                    throw new ArgumentException("Force-instance method \"" + methodBase + "\" has invalid signature");

                _parameters = _parameters.Skip(1).ToArray();
                if (_paramsConverters != null)
                    _paramsConverters = _paramsConverters.Skip(1).ToArray();
            }

            if (_parameters.Length > 0 &&
                _parameters.Last().GetCustomAttribute(typeof(ParamArrayAttribute), false) != null)
                _restPrmsArrayCreator = makeRestPrmsArrayCreator();

            lock (_wrapperCache)
            {
                if (!_wrapperCache.TryGetValue(methodBase, out _fastWrapper))
                    _wrapperCache[methodBase] = _fastWrapper = makeFastWrapper(methodInfo);
            }

            RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
        }
        else if (methodBase is ConstructorInfo)
        {
            lock (_wrapperCache)
            {
                if (!_wrapperCache.TryGetValue(methodBase, out _fastWrapper))
                    _wrapperCache[methodBase] = _fastWrapper = makeFastWrapper(methodBase as ConstructorInfo);
            }

            if (_parameters.Length > 0 &&
                _parameters.Last().GetCustomAttribute(typeof(ParamArrayAttribute), false) != null)
                _restPrmsArrayCreator = makeRestPrmsArrayCreator();
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private MethodProxy(Context context, object hardTarget, MethodBase method, ParameterInfo[] parameters,
        WrapperDelegate fastWrapper, bool forceInstance)
        : base(context)
    {
        _hardTarget = hardTarget;
        _method = method;
        _parameters = parameters;
        _fastWrapper = fastWrapper;
        _forceInstance = forceInstance;
        RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
    }

    public ParameterInfo[] Parameters => _parameters;

    public override string name { get; }

    public override JSValue prototype
    {
        get => null;
        set { }
    }

    private RestPrmsConverter makeRestPrmsArrayCreator()
    {
        var convertArg = ((Func<int, JSValue, object>)convertArgument).GetMethodInfo();
        var processArg = ((Func<Expression[], Context, int, object>)processArgument).GetMethodInfo();

        var context = System.Linq.Expressions.Expression.Parameter(typeof(Context), "context");
        var arguments = System.Linq.Expressions.Expression.Parameter(typeof(Expression[]), "arguments");
        var argumentsObjectPrm = System.Linq.Expressions.Expression.Parameter(typeof(Arguments), "argumentsObject");
        var restItemType = _parameters.Last().ParameterType.GetElementType();
        var returnLabel = System.Linq.Expressions.Expression.Label("return");

        var argumentIndex = System.Linq.Expressions.Expression.Variable(typeof(int), "argumentIndex");
        var resultArray = System.Linq.Expressions.Expression.Variable(_parameters.Last().ParameterType, "resultArray");
        var resultArrayIndex = System.Linq.Expressions.Expression.Variable(typeof(int), "resultArrayIndex");
        var tempValue = System.Linq.Expressions.Expression.Variable(typeof(object), "temp");

        var resultArrayCtor = resultArray.Type.GetConstructor([typeof(int)]);

        var convertedValueArgObj = System.Linq.Expressions.Expression.Call(
            System.Linq.Expressions.Expression.Constant(this), convertArg, argumentIndex,
            System.Linq.Expressions.Expression.Call(argumentsObjectPrm, _argumentsGetItemMethod,
                System.Linq.Expressions.Expression.PostIncrementAssign(argumentIndex)));
        var conditionArgObj = System.Linq.Expressions.Expression.GreaterThanOrEqual(argumentIndex,
            System.Linq.Expressions.Expression.PropertyOrField(argumentsObjectPrm, nameof(Arguments.Length)));
        var arrayAssignArgObj = System.Linq.Expressions.Expression.Assign(
            System.Linq.Expressions.Expression.ArrayAccess(resultArray,
                System.Linq.Expressions.Expression.PostIncrementAssign(resultArrayIndex)),
            System.Linq.Expressions.Expression.Convert(convertedValueArgObj, restItemType));

        var conditionExp = System.Linq.Expressions.Expression.GreaterThanOrEqual(argumentIndex,
            System.Linq.Expressions.Expression.ArrayLength(arguments));
        var getValueExp = System.Linq.Expressions.Expression.Call(
            System.Linq.Expressions.Expression.Constant(this),
            processArg,
            arguments,
            context,
            argumentIndex);
        var arrayAssignExp = System.Linq.Expressions.Expression.Assign(
            System.Linq.Expressions.Expression.ArrayAccess(resultArray,
                System.Linq.Expressions.Expression.PostIncrementAssign(resultArrayIndex)),
            System.Linq.Expressions.Expression.Convert(getValueExp, restItemType));

        var tree = new System.Linq.Expressions.Expression[]
        {
            System.Linq.Expressions.Expression.Assign(argumentIndex,
                System.Linq.Expressions.Expression.Constant(_parameters.Length - 1)),
            System.Linq.Expressions.Expression.Assign(resultArrayIndex, System.Linq.Expressions.Expression.Constant(0)),
            System.Linq.Expressions.Expression.IfThenElse(
                System.Linq.Expressions.Expression.NotEqual(argumentsObjectPrm,
                    System.Linq.Expressions.Expression.Constant(null)),
                System.Linq.Expressions.Expression.Block(
                    System.Linq.Expressions.Expression.IfThen(
                        System.Linq.Expressions.Expression.Equal(
                            System.Linq.Expressions.Expression.PropertyOrField(argumentsObjectPrm,
                                nameof(Arguments.Length)),
                            System.Linq.Expressions.Expression.Constant(_parameters.Length)),
                        System.Linq.Expressions.Expression.Block(
                            System.Linq.Expressions.Expression.Assign(tempValue,
                                System.Linq.Expressions.Expression.Call(
                                    System.Linq.Expressions.Expression.Constant(this), convertArg, argumentIndex,
                                    System.Linq.Expressions.Expression.Call(argumentsObjectPrm, _argumentsGetItemMethod,
                                        argumentIndex))),
                            System.Linq.Expressions.Expression.IfThen(
                                System.Linq.Expressions.Expression.NotEqual(tempValue,
                                    System.Linq.Expressions.Expression.Constant(null)),
                                System.Linq.Expressions.Expression.Return(returnLabel, tempValue)))),
                    System.Linq.Expressions.Expression.Assign(resultArray,
                        System.Linq.Expressions.Expression.New(resultArrayCtor,
                            System.Linq.Expressions.Expression.Subtract(
                                System.Linq.Expressions.Expression.PropertyOrField(argumentsObjectPrm,
                                    nameof(Arguments.Length)), argumentIndex))),
                    System.Linq.Expressions.Expression.Loop(
                        System.Linq.Expressions.Expression.IfThenElse(conditionArgObj,
                            System.Linq.Expressions.Expression.Return(returnLabel,
                                System.Linq.Expressions.Expression.Assign(tempValue, resultArray)),
                            arrayAssignArgObj))),
                System.Linq.Expressions.Expression.Block(
                    System.Linq.Expressions.Expression.IfThen(
                        System.Linq.Expressions.Expression.Equal(
                            System.Linq.Expressions.Expression.ArrayLength(arguments),
                            System.Linq.Expressions.Expression.Constant(_parameters.Length)),
                        System.Linq.Expressions.Expression.Block(
                            System.Linq.Expressions.Expression.Assign(tempValue, getValueExp),
                            System.Linq.Expressions.Expression.IfThen(
                                System.Linq.Expressions.Expression.TypeIs(tempValue, _parameters.Last().ParameterType),
                                System.Linq.Expressions.Expression.Return(returnLabel, tempValue)),
                            System.Linq.Expressions.Expression.Assign(tempValue,
                                System.Linq.Expressions.Expression.NewArrayInit(restItemType,
                                    System.Linq.Expressions.Expression.Convert(tempValue, restItemType))),
                            System.Linq.Expressions.Expression.Return(returnLabel, tempValue))),
                    System.Linq.Expressions.Expression.Assign(resultArray,
                        System.Linq.Expressions.Expression.New(resultArrayCtor,
                            System.Linq.Expressions.Expression.Subtract(
                                System.Linq.Expressions.Expression.ArrayLength(arguments), argumentIndex))),
                    System.Linq.Expressions.Expression.Loop(
                        System.Linq.Expressions.Expression.IfThenElse(conditionExp,
                            System.Linq.Expressions.Expression.Return(returnLabel,
                                System.Linq.Expressions.Expression.Assign(tempValue, resultArray)),
                            System.Linq.Expressions.Expression.Block(arrayAssignExp,
                                System.Linq.Expressions.Expression.PostIncrementAssign(argumentIndex)))))),
            System.Linq.Expressions.Expression.Label(returnLabel),
            tempValue
        };

        var lambda = System.Linq.Expressions.Expression.Lambda<RestPrmsConverter>(
            System.Linq.Expressions.Expression.Block(new[] { argumentIndex, resultArray, resultArrayIndex, tempValue },
                tree),
            context, arguments, argumentsObjectPrm);

        return lambda.Compile();
    }

    private WrapperDelegate makeFastWrapper(MethodInfo methodInfo)
    {
        System.Linq.Expressions.Expression tree = null;
        var target = System.Linq.Expressions.Expression.Parameter(typeof(object), "target");
        var context = System.Linq.Expressions.Expression.Parameter(typeof(Context), "context");
        var arguments = System.Linq.Expressions.Expression.Parameter(typeof(Expression[]), "arguments");
        var argumentsObjectPrm = System.Linq.Expressions.Expression.Parameter(typeof(Arguments), "argumentsObject");

        if (_parameters.Length == 0)
        {
            if (_forceInstance)
            {
                tree = System.Linq.Expressions.Expression.Call(methodInfo,
                    System.Linq.Expressions.Expression.Convert(target, typeof(JSValue)));
            }
            else
            {
                if (methodInfo.IsStatic)
                    tree = System.Linq.Expressions.Expression.Call(methodInfo);
                else
                    tree = System.Linq.Expressions.Expression.Call(
                        System.Linq.Expressions.Expression.Convert(target, methodInfo.DeclaringType), methodInfo);
            }
        }
        else
        {
            if ((_parameters.Length == 1 || (_parameters.Length == 2 && _forceInstance))
                && _parameters[_parameters.Length - 1].ParameterType == typeof(Arguments))
            {
                var argumentsObject = System.Linq.Expressions.Expression.Condition(
                    System.Linq.Expressions.Expression.NotEqual(argumentsObjectPrm,
                        System.Linq.Expressions.Expression.Constant(null)),
                    argumentsObjectPrm,
                    System.Linq.Expressions.Expression.Call(
                        ((Func<Expression[], Context, Arguments>)Tools.CreateArguments).GetMethodInfo(),
                        arguments,
                        context));

                if (_forceInstance)
                {
                    tree = System.Linq.Expressions.Expression.Call(methodInfo,
                        System.Linq.Expressions.Expression.Convert(target, typeof(JSValue)), argumentsObject);
                }
                else
                {
                    if (methodInfo.IsStatic)
                        tree = System.Linq.Expressions.Expression.Call(methodInfo, argumentsObject);
                    else
                        tree = System.Linq.Expressions.Expression.Call(
                            System.Linq.Expressions.Expression.Convert(target, methodInfo.DeclaringType), methodInfo,
                            argumentsObject);
                }
            }
            else
            {
                var processArg = ((Func<Expression[], Context, int, object>)processArgument).GetMethodInfo();
                var processArgTail = ((Func<Expression[], Context, int, object>)processArgumentsTail).GetMethodInfo();
                var convertArg = ((Func<int, JSValue, object>)convertArgument).GetMethodInfo();

                var prms = new System.Linq.Expressions.Expression[_parameters.Length + (_forceInstance ? 1 : 0)];

                if (_restPrmsArrayCreator != null)
                    prms[prms.Length - 1] =
                        System.Linq.Expressions.Expression.Convert(
                            System.Linq.Expressions.Expression.Call(
                                System.Linq.Expressions.Expression.Constant(this),
                                ((Func<Context, Expression[], Arguments, object>)callRestPrmsConverter).GetMethodInfo(),
                                context,
                                arguments,
                                argumentsObjectPrm),
                            _parameters[_parameters.Length - 1].ParameterType);

                var targetPrmIndex = 0;
                if (_forceInstance)
                    prms[targetPrmIndex++] = System.Linq.Expressions.Expression.Convert(target, typeof(JSValue));

                for (var i = 0; targetPrmIndex < prms.Length; i++, targetPrmIndex++)
                {
                    if (targetPrmIndex == prms.Length - 1 && _restPrmsArrayCreator != null)
                        continue;

                    prms[targetPrmIndex] = System.Linq.Expressions.Expression.Convert(
                        System.Linq.Expressions.Expression.Call(
                            System.Linq.Expressions.Expression.Constant(this),
                            targetPrmIndex + 1 < prms.Length ? processArg : processArgTail,
                            arguments,
                            context,
                            System.Linq.Expressions.Expression.Constant(i)),
                        _parameters[i].ParameterType);
                }

                if (methodInfo.IsStatic)
                    tree = System.Linq.Expressions.Expression.Call(methodInfo, prms);
                else
                    tree = System.Linq.Expressions.Expression.Call(
                        System.Linq.Expressions.Expression.Convert(target, methodInfo.DeclaringType), methodInfo, prms);

                targetPrmIndex = 0;
                if (_forceInstance)
                    targetPrmIndex++;

                for (var i = 0; targetPrmIndex < prms.Length; i++, targetPrmIndex++)
                {
                    if (targetPrmIndex == prms.Length - 1 && _restPrmsArrayCreator != null)
                        continue;

                    prms[targetPrmIndex] = System.Linq.Expressions.Expression.Convert(
                        System.Linq.Expressions.Expression.Call(
                            System.Linq.Expressions.Expression.Constant(this),
                            convertArg,
                            System.Linq.Expressions.Expression.Constant(i),
                            System.Linq.Expressions.Expression.Call(argumentsObjectPrm, _argumentsGetItemMethod,
                                System.Linq.Expressions.Expression.Constant(i))),
                        _parameters[i].ParameterType);
                }

                System.Linq.Expressions.Expression treeWithObjectAsSource;
                if (methodInfo.IsStatic)
                    treeWithObjectAsSource = System.Linq.Expressions.Expression.Call(methodInfo, prms);
                else
                    treeWithObjectAsSource = System.Linq.Expressions.Expression.Call(
                        System.Linq.Expressions.Expression.Convert(target, methodInfo.DeclaringType), methodInfo, prms);

                tree = System.Linq.Expressions.Expression.Condition(
                    System.Linq.Expressions.Expression.Equal(argumentsObjectPrm,
                        System.Linq.Expressions.Expression.Constant(null)),
                    tree,
                    treeWithObjectAsSource);
            }
        }

        if (methodInfo.ReturnType == typeof(void))
            tree = System.Linq.Expressions.Expression.Block(tree, System.Linq.Expressions.Expression.Constant(null));

        return System.Linq.Expressions.Expression
            .Lambda<WrapperDelegate>(
                System.Linq.Expressions.Expression.Convert(tree, typeof(object)),
                methodInfo.Name,
                new[]
                {
                    target,
                    context,
                    arguments,
                    argumentsObjectPrm
                })
            .Compile();
    }

    private WrapperDelegate makeFastWrapper(ConstructorInfo constructorInfo)
    {
        System.Linq.Expressions.Expression tree = null;
        var target = System.Linq.Expressions.Expression.Parameter(typeof(object), "target");
        var context = System.Linq.Expressions.Expression.Parameter(typeof(Context), "context");
        var arguments = System.Linq.Expressions.Expression.Parameter(typeof(Expression[]), "arguments");
        var argumentsObjectPrm = System.Linq.Expressions.Expression.Parameter(typeof(Arguments), "argumentsObject");

        if (_parameters.Length == 0)
        {
            tree = System.Linq.Expressions.Expression.New(constructorInfo);
        }
        else
        {
            if (_parameters.Length == 1 && _parameters[0].ParameterType == typeof(Arguments))
            {
                System.Linq.Expressions.Expression argumentsObject = System.Linq.Expressions.Expression.Condition(
                    System.Linq.Expressions.Expression.NotEqual(argumentsObjectPrm,
                        System.Linq.Expressions.Expression.Constant(null)),
                    argumentsObjectPrm,
                    System.Linq.Expressions.Expression.Call(
                        ((Func<Expression[], Context, Arguments>)Tools.CreateArguments).GetMethodInfo(),
                        arguments,
                        context));

                tree = System.Linq.Expressions.Expression.New(constructorInfo, argumentsObject);
            }
            else
            {
                var processArg = ((Func<Expression[], Context, int, object>)processArgument).GetMethodInfo();
                var processArgTail = ((Func<Expression[], Context, int, object>)processArgumentsTail).GetMethodInfo();
                var convertArg = ((Func<int, JSValue, object>)convertArgument).GetMethodInfo();

                var prms = new System.Linq.Expressions.Expression[_parameters.Length];
                for (var i = 0; i < prms.Length; i++)
                    prms[i] = System.Linq.Expressions.Expression.Convert(
                        System.Linq.Expressions.Expression.Call(
                            System.Linq.Expressions.Expression.Constant(this),
                            i + 1 < prms.Length ? processArg : processArgTail,
                            arguments,
                            context,
                            System.Linq.Expressions.Expression.Constant(i)),
                        _parameters[i].ParameterType);

                tree = System.Linq.Expressions.Expression.New(constructorInfo, prms);

                var argumentsObject = argumentsObjectPrm;

                for (var i = 0; i < prms.Length; i++)
                    prms[i] = System.Linq.Expressions.Expression.Convert(
                        System.Linq.Expressions.Expression.Call(
                            System.Linq.Expressions.Expression.Constant(this),
                            convertArg,
                            System.Linq.Expressions.Expression.Constant(i),
                            System.Linq.Expressions.Expression.Call(argumentsObject, _argumentsGetItemMethod,
                                System.Linq.Expressions.Expression.Constant(i))),
                        _parameters[i].ParameterType);

                System.Linq.Expressions.Expression treeWithObjectAsSource;
                treeWithObjectAsSource = System.Linq.Expressions.Expression.New(constructorInfo, prms);

                tree = System.Linq.Expressions.Expression.Condition(
                    System.Linq.Expressions.Expression.Equal(argumentsObjectPrm,
                        System.Linq.Expressions.Expression.Constant(null)),
                    tree,
                    treeWithObjectAsSource);
            }
        }

        return System.Linq.Expressions.Expression
            .Lambda<WrapperDelegate>(
                System.Linq.Expressions.Expression.Convert(tree, typeof(object)),
                constructorInfo.DeclaringType.Name,
                new[]
                {
                    target,
                    context,
                    arguments,
                    argumentsObjectPrm
                })
            .Compile();
    }

    private object callRestPrmsConverter(Context initiator, Expression[] arguments, Arguments argumentsObject)
    {
        return _restPrmsArrayCreator(initiator, arguments, argumentsObject);
    }

    internal override JSValue InternalInvoke(JSValue targetValue, Expression[] argumentsSource, Context initiator,
        bool withSpread, bool withNew)
    {
        if (withNew)
        {
            if (RequireNewKeywordLevel == RequireNewKeywordLevel.WithoutNewOnly)
                ExceptionHelper.ThrowTypeError(string.Format(Strings.InvalidTryToCreateWithNew, name));
        }
        else
        {
            if (RequireNewKeywordLevel == RequireNewKeywordLevel.WithNewOnly)
                ExceptionHelper.ThrowTypeError(string.Format(Strings.InvalidTryToCreateWithoutNew, name));
        }

        var value = invokeMethod(targetValue, argumentsSource, null, initiator);

        if (value is JSValue jsval)
            return jsval;

        if (value is not null
            && targetValue is not null
            && value == targetValue.Value)
            return targetValue;

        return Context.GlobalContext.ProxyValue(value);
    }

    private object invokeMethod(JSValue targetValue, Expression[] argumentsSource, Arguments argumentsObject,
        Context initiator)
    {
        object value;
        var target = GetTargetObject(targetValue, _hardTarget);
        if (_parameters.Length == 0 && argumentsSource != null)
            for (var i = 0; i < argumentsSource.Length; i++)
                argumentsSource[i].Evaluate(initiator);

        value = _fastWrapper(target, initiator, argumentsSource, argumentsObject);

        if (_returnConverter != null)
            value = _returnConverter.From(value);

        return value;
    }

    private object processArgumentsTail(Expression[] arguments, Context context, int index)
    {
        var result = processArgument(arguments, context, index);

        while (++index < arguments.Length)
            arguments[index].Evaluate(context);

        return result;
    }

    internal object GetTargetObject(JSValue targetValue, object hardTarget)
    {
        var target = hardTarget;
        if (target == null)
        {
            if (_forceInstance)
            {
                if (targetValue != null && targetValue._valueType >= JSValueType.Object)
                {
                    // Объект нужно развернуть до основного значения. Даже если это обёртка над примитивным значением
                    target = targetValue.Value;

                    var proxy = target as Proxy;
                    if (proxy != null)
                        target = proxy.PrototypeInstance ?? target;

                    // ForceInstance работает только если первый аргумент типа JSValue
                    if (!(target is JSValue))
                        target = targetValue;
                }
                else
                {
                    target = targetValue ?? undefined;
                }
            }
            else if (!_method.IsStatic && !_method.IsConstructor)
            {
                target = convertTargetObject(targetValue ?? undefined, _method.DeclaringType);
                if (target == null)
                {
                    // Исключительная ситуация. Я не знаю почему Function.length обобщённое свойство, а не константа. Array.length работает по-другому.
                    if (_method.Name == "get_length" && typeof(Function).IsAssignableFrom(_method.DeclaringType))
                        return Empty;

                    ExceptionHelper.Throw(new TypeError("Cannot call function \"" + name +
                                                        "\" for object of another type."));
                }
            }
        }

        return target;
    }

    private static object convertTargetObject(JSValue target, Type targetType)
    {
        if (target == null)
            return null;

        target = target._oValue as JSValue ?? target; // это может быть лишь ссылка на какой-то другой контейнер
        var res = Tools.ConvertJStoObj(target, targetType, false);
        return res;
    }

#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private object processArgument(Expression[] arguments, Context initiator, int index)
    {
        var value = arguments.Length > index
            ? Tools.EvalExpressionSafe(initiator, arguments[index])
            : notExists;

        return convertArgument(index, value);
    }

#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private object convertArgument(int index, JSValue value)
    {
        var cvtArgs = ConvertArgsOptions.ThrowOnError | ConvertArgsOptions.AllowDefaultValues;
        if (_strictConversion)
            cvtArgs |= ConvertArgsOptions.StrictConversion;

        return convertArgument(
            index,
            value,
            cvtArgs);
    }

    private object convertArgument(int index, JSValue value, ConvertArgsOptions options)
    {
        if (_paramsConverters != null && _paramsConverters[index] != null)
            return _paramsConverters[index].To(value);

        var strictConversion = (options & ConvertArgsOptions.StrictConversion) == ConvertArgsOptions.StrictConversion;
        var processRest = _restPrmsArrayCreator != null && index >= _parameters.Length - 1 &&
                          (index >= _parameters.Length || value.ValueType != JSValueType.Object ||
                           !(value.Value is Array));
        var parameterInfo = processRest ? _parameters[_parameters.Length - 1] : _parameters[index];
        var parameterType = processRest ? parameterInfo.ParameterType.GetElementType() : parameterInfo.ParameterType;
        object result = null;

        if (value._valueType >= JSValueType.Object && value._oValue == null &&
            parameterType.GetTypeInfo().IsClass) return null;

        if (value._valueType > JSValueType.Undefined)
        {
            result = Tools.ConvertJStoObj(value, parameterType, !strictConversion);
            if (strictConversion && result == null)
            {
                if ((options & ConvertArgsOptions.ThrowOnError) != 0)
                    ExceptionHelper.ThrowTypeError("Unable to convert " + value + " to type " + parameterType);

                if ((options & ConvertArgsOptions.AllowDefaultValues) == 0)
                    return null;
            }
        }
        else
        {
            if (parameterType.IsAssignableFrom(value.GetType()))
                return value;
        }

        if (result == null
            && _restPrmsArrayCreator == null
            && (options & ConvertArgsOptions.AllowDefaultValues) != 0
            && ((parameterInfo.Attributes & ParameterAttributes.HasDefault) != 0
                || parameterInfo.ParameterType.IsValueType))
        {
            result = parameterInfo.DefaultValue;

#if (PORTABLE || NETCORE)
            if (result != null && result.GetType().FullName == "System.DBNull")
            {
#else
            if (result is DBNull)
            {
#endif
                if (strictConversion && options.HasFlag(ConvertArgsOptions.ThrowOnError))
                    ExceptionHelper.ThrowTypeError("Unable to convert " + value + " to type " + parameterType);

                if (parameterType.GetTypeInfo().IsValueType)
                    result = Activator.CreateInstance(parameterType);
                else
                    result = null;
            }
        }

        return result;
    }

    internal object[] ConvertArguments(Arguments arguments, ConvertArgsOptions options)
    {
        if (_parameters.Length == 0)
            return null;

        if (_forceInstance)
            ExceptionHelper.Throw(new InvalidOperationException());

        object[] res = null;
        var targetCount = _parameters.Length;
        for (var i = targetCount; i-- > 0;)
        {
            var jsValue = arguments?[i] ?? undefined;

            var value = convertArgument(i, jsValue, options);

            if (value == null && !jsValue.IsNull)
                return null;

            if (res == null)
                res = new object[targetCount];

            res[i] = value;
        }

        return res;
    }

    protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
    {
        if (arguments == null)
            arguments = new Arguments();

        var result = invokeMethod(targetObject, null, arguments, Context);

        if (result == null)
            return undefined;

        return result as JSValue ?? Context.GlobalContext.ProxyValue(result);
    }

    public override Function bind(Arguments args)
    {
        if (_hardTarget != null || args.Length == 0)
            return this;

        if (args.Length > 1)
            return new BindedFunction(this, args);

        var target = args[0];
        var result = new MethodProxy(
            Context,
            convertTargetObject(target, _method.DeclaringType) ??
            target.Value as JSObject ?? (target.Defined ? target : null),
            _method,
            _parameters,
            _fastWrapper,
            _forceInstance);

        return result;
    }

#if !NET40
    public override Delegate MakeDelegate(Type delegateType)
    {
        try
        {
            var methodInfo = _method as MethodInfo;
            return methodInfo.CreateDelegate(delegateType, _hardTarget);
        }
        catch
        {
            return base.MakeDelegate(delegateType);
        }
    }
#endif

    public override string ToString(bool headerOnly)
    {
        var result = "function " + name + "()";

        if (!headerOnly) result += " { [native code] }";

        return result;
    }

    private delegate object RestPrmsConverter(Context initiator, Expression[] arguments, Arguments argumentsObject);
}