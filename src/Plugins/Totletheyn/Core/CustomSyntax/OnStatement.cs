using System.Collections.Generic;
using Extism;
using Totletheyn.Core.Js.BaseLibrary;
using Totletheyn.Core.Js.Core;
using Totletheyn.Core.Js.Expressions;
using Totletheyn.Core.Js.Statements;

namespace Totletheyn.Core.CustomSyntax;

[CustomCodeFragment]
public sealed class OnStatement(string interval, string aliasName, FunctionDefinition callable)
    : CodeNode
{
    public static bool Validate(string code, int position)
    {
        return Parser.Validate(code, "on", position);
    }

    public static CodeNode Parse(ParseInfo state, ref int position)
    {
        if (!Parser.Validate(state.Code, "on", ref position))
            return null;

        while (char.IsWhiteSpace(state.Code, position))
            position++;

        if (state.Code[position] != '"')
            throw new JSException(new SyntaxError("Expected '\"' at " +
                                                  CodeCoordinates.FromTextPosition(state.Code, position, 1)));

        position++;
        var start = position;
        while (state.Code[position] != '"')
            position++;

        var interval = state.Code.Substring(start, position - start);
        position++;

        while (char.IsWhiteSpace(state.Code, position))
            position++;

        if (!Parser.Validate(state.Code, "do", ref position))
            throw new JSException(new SyntaxError("Expected \"do\" at " +
                                                  CodeCoordinates.FromTextPosition(state.Code, position, 2)));

        while (char.IsWhiteSpace(state.Code, position))
            position++;

        var callableBlock = (CodeBlock)CodeBlock.Parse(state, ref position);
        var callable = new FunctionDefinition
        {
            _body = callableBlock
        };

        while (char.IsWhiteSpace(state.Code, position))
            position++;

        if (!Parser.Validate(state.Code, "as", ref position))
            throw new JSException(new SyntaxError("Expected \"as\" at " +
                                                  CodeCoordinates.FromTextPosition(state.Code, position, 2)));

        while (char.IsWhiteSpace(state.Code, position))
            position++;

        start = position;
        if (!Parser.ValidateName(state.Code, ref position))
            throw new JSException(new SyntaxError("Expected identifier name at " +
                                                  CodeCoordinates.FromTextPosition(state.Code, position, 0)));

        var aliasName = state.Code.Substring(start, position - start);

        if (!Parser.Validate(state.Code, ";", ref position))
            throw new JSException(new SyntaxError("Expected \";\" at " +
                                                  CodeCoordinates.FromTextPosition(state.Code, position, 2)));

        return new OnStatement(interval, aliasName, callable);
    }

    public override JSValue Evaluate(Context context)
    {
        var src = $"""
                   _on("{interval}")
                   .name("{aliasName}")
                   .do({callable});
                   """;

        return context.Eval(src);
    }

    public override void Decompose(ref CodeNode self)
    {
    }

    public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> newVariables,
        int scopeBias)
    {
    }
}