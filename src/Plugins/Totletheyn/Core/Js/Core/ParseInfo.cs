using System;
using System.Collections.Generic;
using Totletheyn.Core.Js.Expressions;

namespace Totletheyn.Core.Js.Core;

public sealed class ParseInfo
{
    public readonly Stack<bool> AllowBreak;
    public readonly Stack<bool> AllowContinue;
    public readonly Dictionary<double, JSValue> DoubleConstants;
    public readonly Dictionary<int, JSValue> IntConstants;
    public readonly InternalCompilerMessageCallback Message;
    public readonly string SourceCode;
    public readonly Dictionary<string, JSValue> StringConstants;
    public readonly List<VariableDescriptor> Variables;
    public int AllowReturn;
    public int BreaksCount;

    public string Code;
    public CodeContext CodeContext;
    public int ContiniesCount;
    public int FunctionScopeLevel;

    public int LabelsCount;
    public int LexicalScopeLevel;

    private ParseInfo(
        string code,
        List<string> labels,
        Stack<bool> allowBreak,
        Stack<bool> allowContinue,
        Dictionary<string, JSValue> stringConstants,
        Dictionary<int, JSValue> intConstants,
        Dictionary<double, JSValue> doubleConstants,
        List<VariableDescriptor> variables)
    {
        Code = code;
        SourceCode = code;
        Labels = labels;
        AllowBreak = allowBreak;
        AllowContinue = allowContinue;
        StringConstants = stringConstants;
        IntConstants = intConstants;
        DoubleConstants = doubleConstants;
        Variables = variables;
    }

    public ParseInfo(string sourceCode, InternalCompilerMessageCallback message)
    {
        Code = Parser.RemoveComments(sourceCode, 0);
        SourceCode = sourceCode;
        Message = message;

        CodeContext |= CodeContext.AllowDirectives;

        Labels = new List<string>();
        AllowBreak = new Stack<bool>();
        AllowContinue = new Stack<bool>();

        StringConstants = new Dictionary<string, JSValue>();
        IntConstants = new Dictionary<int, JSValue>();
        DoubleConstants = new Dictionary<double, JSValue>();

        Variables = new List<VariableDescriptor>();

        AllowContinue.Push(false);
        AllowBreak.Push(false);
    }

    public List<string> Labels { get; private set; }

    public bool Strict => (CodeContext & CodeContext.Strict) != 0;
    public bool AllowDirectives => (CodeContext & CodeContext.AllowDirectives) != 0;

    internal JSValue GetCachedValue(int value)
    {
        if (!IntConstants.ContainsKey(value))
        {
            JSValue jsvalue = value;
            IntConstants[value] = jsvalue;
            return jsvalue;
        }

        return IntConstants[value];
    }

    public ParseInfo AlternateCode(string code)
    {
        var result = new ParseInfo(code, Labels, AllowBreak, AllowContinue, StringConstants, IntConstants,
            DoubleConstants, Variables);
        return result;
    }

    public IDisposable WithCodeContext(CodeContext codeContext = default)
    {
        var result = new ContextReseter(this, CodeContext);
        CodeContext |= codeContext;
        return result;
    }

    public IDisposable WithNewLabelsScope()
    {
        var result = new LabelsReseter(this, Labels);
        Labels = new List<string>();
        return result;
    }

    private class ContextReseter : IDisposable
    {
        private readonly CodeContext _oldCodeContext;
        private readonly ParseInfo _parseInfo;

        public ContextReseter(ParseInfo parseInfo, CodeContext oldCodeContext)
        {
            _parseInfo = parseInfo;
            _oldCodeContext = oldCodeContext;
        }

        public void Dispose()
        {
            _parseInfo.CodeContext = _oldCodeContext;
        }
    }

    private class LabelsReseter : IDisposable
    {
        private readonly List<string> _oldLabels;
        private readonly ParseInfo _parseInfo;

        public LabelsReseter(ParseInfo parseInfo, List<string> oldLabels)
        {
            _parseInfo = parseInfo;
            _oldLabels = oldLabels;
        }

        public void Dispose()
        {
            _parseInfo.Labels = _oldLabels;
        }
    }
}

#if !NETCORE
[Serializable]
#endif
public sealed class FunctionInfo
{
    public readonly List<Expression> Returns = new();
    public bool ContainsArguments;
    public bool ContainsDebugger;
    public bool ContainsEval;
    public bool ContainsInnerEntities;
    public bool ContainsRestParameters;
    public bool ContainsThis;
    public bool ContainsTry;
    public bool ContainsWith;
    public bool NeedDecompose;
    public PredictedType ResultType;
    public bool UseCall;
    public bool UseGetMember;
    public bool WithLexicalEnvironment;
}