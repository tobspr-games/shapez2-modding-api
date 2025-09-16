using Core.Localization;
using UnityEngine;

namespace ShapezShifter.Flow
{
    public interface IIdentifiableIslandGroupBuilder
    {
        IIdentifiableAndPresentableIslandGroupBuilder WithPresentation(IText title,
            IText description, Sprite icon);

        IIdentifiableAndTitledIslandGroupBuilder WithTitle(IText title);
    }
}