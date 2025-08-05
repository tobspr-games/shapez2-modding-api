using System;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class BuildingModulesExtender : IDisposable
    {
        private readonly IExtendersProvider ExtendersProvider;
        private readonly Hook InjectBuildingsModuleProvidersHook;

        public BuildingModulesExtender(IExtendersProvider extendersProvider)
        {
            ExtendersProvider = extendersProvider;
            InjectBuildingsModuleProvidersHook = DetourHelper
                .CreatePostfixHook<GameCore, BuildingsModulesLookup>((core, lookup) =>
                    core.InjectBuildingsModuleProviders(lookup), Postfix);
        }

        private void Postfix(GameCore gameCore, BuildingsModulesLookup modulesLookup)
        {
            var buildingModulesExtenders = ExtendersProvider.ExtendersOfType<IBuildingModulesExtender>();

            foreach (var buildingModulesExtender in buildingModulesExtenders)
            {
                buildingModulesExtender.AddModules(modulesLookup);
            }
        }

        public void Dispose()
        {
            InjectBuildingsModuleProvidersHook.Dispose();
        }
    }

    public interface IBuildingModulesExtender : IExtender
    {
        void AddModules(BuildingsModulesLookup modulesLookup);
    }
}