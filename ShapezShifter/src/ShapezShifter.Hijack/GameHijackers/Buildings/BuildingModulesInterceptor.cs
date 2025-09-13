using System;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class BuildingModulesInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly Hook InjectBuildingsModuleProvidersHook;

        public BuildingModulesInterceptor(IRewirerProvider rewirerProvider)
        {
            RewirerProvider = rewirerProvider;
            InjectBuildingsModuleProvidersHook = DetourHelper
               .CreatePostfixHook<GameCore, BuildingsModulesLookup>((core, lookup) =>
                        core.InjectBuildingsModuleProviders(lookup),
                    Postfix);
        }

        private void Postfix(GameCore gameCore, BuildingsModulesLookup modulesLookup)
        {
            IEnumerable<IBuildingModulesRewirer> buildingModulesRewirers =
                RewirerProvider.RewirersOfType<IBuildingModulesRewirer>();

            foreach (IBuildingModulesRewirer buildingModulesRewirer in buildingModulesRewirers)
            {
                buildingModulesRewirer.AddModules(modulesLookup);
            }
        }

        public void Dispose()
        {
            InjectBuildingsModuleProvidersHook.Dispose();
        }
    }

    public interface IBuildingModulesRewirer : IRewirer
    {
        void AddModules(BuildingsModulesLookup modulesLookup);
    }
}