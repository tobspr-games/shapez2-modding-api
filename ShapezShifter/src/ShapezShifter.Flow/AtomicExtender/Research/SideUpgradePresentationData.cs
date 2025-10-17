using System.Collections.Generic;
using System.Linq;
using Core.Localization;
using Game.Core.Research;

namespace ShapezShifter.Flow.Research
{
    public class SideUpgradePresentationData
    {
        public readonly ResearchUpgradeId Id;
        public readonly string PreviewImageId;
        public readonly string VideoId;
        public readonly IText Title;
        public readonly IText Description;
        public readonly bool Hidden = false;
        public readonly string Category;

        private SideUpgradePresentationData(ResearchUpgradeId id, string previewImageId, string videoId, IText title,
            IText description,
            bool hidden, string category)
        {
            Id = id;
            PreviewImageId = previewImageId;
            VideoId = videoId;
            Title = title;
            Description = description;
            Hidden = hidden;
            Category = category;
        }
    }
}