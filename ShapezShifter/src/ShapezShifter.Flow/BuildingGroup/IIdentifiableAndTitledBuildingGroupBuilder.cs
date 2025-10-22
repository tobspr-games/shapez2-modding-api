using Core.Localization;

namespace ShapezShifter.Flow
{
    public interface IIdentifiableAndTitledBuildingGroupBuilder
    {
        IIdentifiableTitledAndDescribedBuildingGroupBuilder WithDescription(IText description);
    }
}