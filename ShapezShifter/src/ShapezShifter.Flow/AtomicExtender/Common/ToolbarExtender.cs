using Core.Events;
using Core.Localization;
using ShapezShifter.Flow.Toolbar;
using ShapezShifter.Hijack;
using UnityEngine;

namespace ShapezShifter.Flow.Atomic
{
    public class ToolbarRewirer : IToolbarDataRewirer,
        IChainableRewirer<PlacementToolbarElementData>
    {
        private readonly PlacementInitiatorId Placement;
        private readonly IText Title;
        private readonly IText Description;
        private readonly Sprite Icon;
        private readonly IToolbarEntryInsertLocation EntryInsertLocation;

        public ToolbarRewirer(PlacementInitiatorId placement, IText title, IText description, Sprite icon,
            IToolbarEntryInsertLocation entryInsertLocation)
        {
            Placement = placement;
            Title = title;
            Description = description;
            Icon = icon;
            EntryInsertLocation = entryInsertLocation;
        }

        public ToolbarData ModifyToolbarData(ToolbarData toolbarData)
        {
            PlacementToolbarElementData toolbarElement = new(
                Title,
                Description,
                Placement,
                Icon);

            EntryInsertLocation.AddEntry(toolbarData, toolbarElement);


            _AfterExtensionApplied.Invoke(toolbarElement);
            return toolbarData;
        }

        public IEvent<PlacementToolbarElementData> AfterHijack => _AfterExtensionApplied;

        private readonly MultiRegisterEvent<PlacementToolbarElementData> _AfterExtensionApplied =
            new();
    }
}