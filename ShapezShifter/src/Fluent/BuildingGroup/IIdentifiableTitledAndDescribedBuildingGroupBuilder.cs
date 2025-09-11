using UnityEngine;

namespace ShapezShifter.Fluent
{
    public interface IIdentifiableTitledAndDescribedBuildingGroupBuilder
    {
        IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(Sprite icon);

        IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(string filePath);

        IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(Texture texture);
    }
}