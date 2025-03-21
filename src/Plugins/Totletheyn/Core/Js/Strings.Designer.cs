﻿using System.Reflection;

namespace Totletheyn.Core.Js;

/// <summary>
///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
/// </summary>
// Этот класс создан автоматически классом StronglyTypedResourceBuilder
// с помощью такого средства, как ResGen или Visual Studio.
// Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
// с параметром /str или перестройте свой проект VS.
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
internal class Strings {
    
    private static global::System.Resources.ResourceManager resourceMan;
    
    private static global::System.Globalization.CultureInfo resourceCulture;
    
    [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Strings() {
    }
    
    /// <summary>
    ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Resources.ResourceManager ResourceManager {
        get {
            if (object.ReferenceEquals(resourceMan, null)) {
                var packageName = "Totletheyn.Core.Js";
                global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(packageName + ".Strings", typeof(Strings).GetTypeInfo().Assembly);
                resourceMan = temp;
            }
            return resourceMan;
        }
    }
    
    /// <summary>
    ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
    ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Globalization.CultureInfo Culture {
        get {
            return resourceCulture;
        }
        set {
            resourceCulture = value;
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Cannot assign to readonly property &quot;{0}&quot;.
    /// </summary>
    internal static string CannotAssignReadOnly {
        get {
            return ResourceManager.GetString("CannotAssignReadOnly", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Constructor cannot be static.
    /// </summary>
    internal static string ConstructorCannotBeStatic {
        get {
            return ResourceManager.GetString("ConstructorCannotBeStatic", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Do not declare function in nested blocks.
    /// </summary>
    internal static string DoNotDeclareFunctionInNestedBlocks {
        get {
            return ResourceManager.GetString("DoNotDeclareFunctionInNestedBlocks", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Do not define a function inside a loop.
    /// </summary>
    internal static string FunctionInLoop {
        get {
            return ResourceManager.GetString("FunctionInLoop", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Identifier &quot;{0}&quot; has already been declared.
    /// </summary>
    internal static string IdentifierAlreadyDeclared {
        get {
            return ResourceManager.GetString("IdentifierAlreadyDeclared", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Cannot increment property &quot;{0}&quot; without setter.
    /// </summary>
    internal static string IncrementPropertyWOSetter {
        get {
            return ResourceManager.GetString("IncrementPropertyWOSetter", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Cannot increment readonly &quot;{0}&quot;.
    /// </summary>
    internal static string IncrementReadonly {
        get {
            return ResourceManager.GetString("IncrementReadonly", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Invalid left-hand side in assignment..
    /// </summary>
    internal static string InvalidLefthandSideInAssignment {
        get {
            return ResourceManager.GetString("InvalidLefthandSideInAssignment", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Invalid property name at {0}.
    /// </summary>
    internal static string InvalidPropertyName {
        get {
            return ResourceManager.GetString("InvalidPropertyName", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Unable to process regular expression {0}.
    /// </summary>
    internal static string InvalidRegExp {
        get {
            return ResourceManager.GetString("InvalidRegExp", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Method &quot;{0}&quot; can not be called with new keyword.
    /// </summary>
    internal static string InvalidTryToCallWithNew {
        get {
            return ResourceManager.GetString("InvalidTryToCallWithNew", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Method &quot;{0}&quot; can not be called without new keyword.
    /// </summary>
    internal static string InvalidTryToCallWithoutNew {
        get {
            return ResourceManager.GetString("InvalidTryToCallWithoutNew", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Type &quot;{0}&quot; can not be created with new keyword.
    /// </summary>
    internal static string InvalidTryToCreateWithNew {
        get {
            return ResourceManager.GetString("InvalidTryToCreateWithNew", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Type &quot;{0}&quot; can not be created without new keyword.
    /// </summary>
    internal static string InvalidTryToCreateWithoutNew {
        get {
            return ResourceManager.GetString("InvalidTryToCreateWithoutNew", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Too many arguments for function &quot;{0}&quot;.
    /// </summary>
    internal static string TooManyArgumentsForFunction {
        get {
            return ResourceManager.GetString("TooManyArgumentsForFunction", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Can&apos;t get property &quot;{0}&quot; of &quot;{1}&quot;.
    /// </summary>
    internal static string TryingToGetProperty {
        get {
            return ResourceManager.GetString("TryingToGetProperty", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Can&apos;t set property &quot;{0}&quot; of &quot;{1}&quot;.
    /// </summary>
    internal static string TryingToSetProperty {
        get {
            return ResourceManager.GetString("TryingToSetProperty", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Unexpected end of source.
    /// </summary>
    internal static string UnexpectedEndOfSource {
        get {
            return ResourceManager.GetString("UnexpectedEndOfSource", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Unexpected token.
    /// </summary>
    internal static string UnexpectedToken {
        get {
            return ResourceManager.GetString("UnexpectedToken", resourceCulture);
        }
    }
    
    /// <summary>
    ///   Ищет локализованную строку, похожую на Unknown identifier &quot;{0}&quot; at {1}.
    /// </summary>
    internal static string UnknowIdentifier {
        get {
            return ResourceManager.GetString("UnknowIdentifier", resourceCulture);
        }
    }

    /// <summary>
    ///   Ищет локализованную строку, похожую на Variable &quot;{0}&quot; is not defined.
    /// </summary>
    internal static string VariableNotDefined
    {
        get
        {
            return ResourceManager.GetString("VariableNotDefined", resourceCulture);
        }
    }

    /// <summary>
    ///   Ищет локализованную строку, похожую на Variable &quot;{0}&quot; is not defined.
    /// </summary>
    internal static string LogicalNullishCoalescing
    {
        get
        {
            return ResourceManager.GetString("LogicalNullishCoalescing", resourceCulture);
        }
    }
}
