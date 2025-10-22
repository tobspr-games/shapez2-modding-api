using Core.Localization;
using UnityEngine;

namespace ShapezShifter.Flow
{
    public interface IIdentifiableBuildingGroupBuilder
    {
        IIdentifiableAndPresentableBuildingGroupBuilder WithPresentation(IText title,
            IText description, Sprite icon);

        IIdentifiableAndTitledBuildingGroupBuilder WithTitle(IText title);
    }
}