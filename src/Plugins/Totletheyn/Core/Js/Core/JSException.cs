using System;
using Totletheyn.Core.Js.BaseLibrary;

namespace Totletheyn.Core.Js.Core;

/// <summary>
///     Представляет ошибки, возникшие во время выполнения скрипта.
/// </summary>
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public sealed class JSException : Exception
{
    private ExceptionHelper.StackTraceState _stackTraceData;

    private JSException()
    {
        _stackTraceData = ExceptionHelper.GetJsStackTrace();
    }

    public JSException(Error data)
        : this(Context.CurrentGlobalContext.ProxyValue(data))
    {
    }

    public JSException(JSValue data)
        : this()
    {
        Error = data;
    }

    public JSException(Error data, CodeNode exceptionMaker)
        : this(Context.CurrentGlobalContext.ProxyValue(data), exceptionMaker)
    {
    }

    public JSException(JSValue data, CodeNode exceptionMaker)
        : this()
    {
        Error = data;
        ExceptionMaker = exceptionMaker;
    }

    public JSException(JSValue data, Exception innerException)
        : base("External error", innerException)
    {
        Error = data;

        _stackTraceData = ExceptionHelper.GetJsStackTrace();
    }

    public JSException(Error avatar, Exception innerException)
        : this(Context.CurrentGlobalContext.ProxyValue(avatar), innerException)
    {
    }

    public JSValue Error { get; }
    public CodeNode ExceptionMaker { get; }
    public string SourceCode { get; internal set; }
    public CodeCoordinates CodeCoordinates { get; internal set; }

    public override string StackTrace => _stackTraceData?.ToString(this) ?? base.StackTrace;

    public override string Message
    {
        get
        {
            var result = CodeCoordinates != null ? " at " + CodeCoordinates : null;
            if (Error?._oValue is Error)
            {
                var n = Error.GetProperty("name");
                if (n._valueType == JSValueType.Property)
                    n = (n._oValue as PropertyPair).getter.Call(Error, null).ToString();

                var m = Error.GetProperty("message");
                if (m._valueType == JSValueType.Property)
                    result = n + ": " + (m._oValue as PropertyPair).getter.Call(Error, null) + result;
                else
                    result = n + ": " + m + result;
            }
            else
            {
                result = Error + result;
            }

            return result ?? "JavaScript Error";
        }
    }
}