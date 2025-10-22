using UnityEngine;

namespace ShapezShifter.Flow
{
    public interface IIdentifiableTitledAndDescribedBuildingGroupBuilder
    {
        IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(Sprite icon);

        IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(string filePath);

        IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(Texture texture);
    }
}