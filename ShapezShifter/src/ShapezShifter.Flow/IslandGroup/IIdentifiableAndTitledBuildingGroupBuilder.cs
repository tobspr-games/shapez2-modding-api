using Core.Localization;

namespace ShapezShifter.Flow
{
    public interface IIdentifiableAndTitledIslandGroupBuilder
    {
        IIdentifiableTitledAndDescribedIslandGroupBuilder WithDescription(IText description);
    }
}