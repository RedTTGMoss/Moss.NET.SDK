using System;

namespace Totletheyn.Core.Js.Core;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CustomCodeFragment : Attribute
{
    public CustomCodeFragment()
        : this(CodeFragmentType.Statement)
    {
    }

    public CustomCodeFragment(CodeFragmentType codeFragmentType)
        : this(codeFragmentType, null)
    {
    }

    public CustomCodeFragment(CodeFragmentType codeFragmentType, params string[] reservedWords)
    {
        Type = codeFragmentType;
        ReservedWords = reservedWords ?? new string[0];
    }

    public CodeFragmentType Type { get; private set; }
    public string[] ReservedWords { get; private set; }
}