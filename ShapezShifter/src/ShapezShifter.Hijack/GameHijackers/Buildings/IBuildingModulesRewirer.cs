namespace ShapezShifter.Hijack
{
    public interface IBuildingModulesRewirer : IRewirer
    {
        void AddModules(BuildingsModulesLookup modulesLookup);
    }
}