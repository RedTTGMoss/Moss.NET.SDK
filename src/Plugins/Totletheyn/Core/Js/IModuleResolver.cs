namespace Totletheyn.Core.Js;

public interface IModuleResolver
{
    bool TryGetModule(ModuleRequest moduleRequest, out Module result);
}