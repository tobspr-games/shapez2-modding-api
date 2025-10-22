namespace ShapezShifter.Hijack
{
    public interface IIslandModulesRewirer : IRewirer
    {
        void AddModules(IslandsModulesLookup modulesLookup);
    }
}