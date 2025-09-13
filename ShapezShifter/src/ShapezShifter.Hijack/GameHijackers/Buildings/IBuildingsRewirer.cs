using Game.Core.Rendering.MeshGeneration;

namespace ShapezShifter.Hijack
{
    public interface IBuildingsRewirer : IRewirer
    {
        GameBuildings ModifyGameBuildings(MetaGameModeBuildings metaBuildings, GameBuildings gameBuildings,
            IMeshCache meshCache, VisualThemeBaseResources theme);
    }
}