using Game.Core.Rendering.MeshGeneration;

namespace ShapezShifter
{
    public interface IBuildingsExtender : IExtender
    {
        GameBuildings ModifyGameBuildings(MetaGameModeBuildings metaBuildings, GameBuildings gameBuildings,
            IMeshCache meshCache, VisualThemeBaseResources theme);
    }
}