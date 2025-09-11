using Core.Localization;

namespace ShapezShifter.Fluent
{
    public interface IIdentifiableAndTitledBuildingGroupBuilder
    {
        IIdentifiableTitledAndDescribedBuildingGroupBuilder WithDescription(IText description);
    }
}