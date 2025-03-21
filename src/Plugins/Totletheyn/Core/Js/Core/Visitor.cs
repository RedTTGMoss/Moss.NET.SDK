using System;
using Totletheyn.Core.Js.Expressions;
using Totletheyn.Core.Js.Statements;

namespace Totletheyn.Core.Js.Core;

/// <summary>
///     AST nodes visitor.
/// </summary>
/// <typeparam name="T">Type of return value</typeparam>
#if !(PORTABLE || NETCORE)
[Serializable]
#endif
public abstract class Visitor<T>
{
    protected internal abstract T Visit(CodeNode node);

    protected internal virtual T Visit(Addition node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(BitwiseConjunction node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(ArrayDefinition node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Assignment node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Call node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(ClassDefinition node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Constant node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Decrement node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Delete node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(DeleteProperty node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Division node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Equal node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Expression node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(FunctionDefinition node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Property node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Variable node)
    {
        return Visit(node as VariableReference);
    }

    protected internal virtual T Visit(VariableReference node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(In node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Import node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Increment node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(InstanceOf node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(ObjectDefinition node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Less node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(LessOrEqual node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(LogicalConjunction node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(LogicalNegation node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(LogicalDisjunction node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Modulo node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(More node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(MoreOrEqual node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Multiplication node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Negation node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(New node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Comma node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(BitwiseNegation node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(NotEqual node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(NumberAddition node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(NumberLess node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(NumberLessOrEqual node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(NumberMore node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(NumberMoreOrEqual node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(BitwiseDisjunction node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(RegExpExpression node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(SetProperty node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(SignedShiftLeft node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(SignedShiftRight node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(StrictEqual node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(StrictNotEqual node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(StringConcatenation node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Substract node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(Conditional node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(ConvertToBoolean node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(ConvertToInteger node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(ConvertToNumber node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(ConvertToString node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(ConvertToUnsignedInteger node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(TypeOf node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(UnsignedShiftRight node)
    {
        return Visit(node as Expression);
    }

    protected internal virtual T Visit(BitwiseExclusiveDisjunction node)
    {
        return Visit(node as Expression);
    }
#if !(PORTABLE || NETCORE)
    protected internal virtual T Visit(Yield node)
    {
        return Visit(node as Expression);
    }
#endif
    protected internal virtual T Visit(Break node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(CodeBlock node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(Continue node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(Debugger node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(DoWhile node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(Empty node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(ForIn node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(ForOf node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(For node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(IfElse node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(InfinityLoop node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(LabeledStatement node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(Return node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(Switch node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(Throw node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(TryCatch node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(VariableDefinition node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(While node)
    {
        return Visit(node as CodeNode);
    }

    protected internal virtual T Visit(With node)
    {
        return Visit(node as CodeNode);
    }
}