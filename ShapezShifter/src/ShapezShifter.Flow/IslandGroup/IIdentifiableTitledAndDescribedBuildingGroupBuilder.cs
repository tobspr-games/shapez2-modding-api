using UnityEngine;

namespace ShapezShifter.Flow
{
    public interface IIdentifiableTitledAndDescribedIslandGroupBuilder
    {
        IIdentifiableAndPresentableIslandGroupBuilder WithIcon(Sprite icon);

        IIdentifiableAndPresentableIslandGroupBuilder WithIcon(string filePath);

        IIdentifiableAndPresentableIslandGroupBuilder WithIcon(Texture texture);
    }
}