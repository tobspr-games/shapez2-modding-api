using System;
using System.Collections.Generic;
using Core.Localization;
using Game.Core.Research;
using UnityEngine;

namespace ShapezShifter.Flow
{
    public class BuildingGroupBuilder : IIdentifiableBuildingGroupBuilder,
        IIdentifiableAndTitledBuildingGroupBuilder,
        IIdentifiableTitledAndDescribedBuildingGroupBuilder,
        IIdentifiableAndPresentableBuildingGroupBuilder,
        IIdentifiablePresentableAndCategorizedBuildingGroupBuilder,
        IBuildingGroupBuilder
    {
        internal BuildingGroupBuilder(BuildingDefinitionGroupId groupId)
        {
            GroupId = groupId;
        }

        public BuildingDefinitionGroupId GroupId { get; }
        private Sprite Icon;
        private IText Title;
        private IText Description;
        private bool IsTransportBuilding;
        private DefaultPreferredPlacementMode DefaultPreferredPlacement;
        private bool IsRemovable = true;
        private bool IsSelectable = true;
        private bool IsBuildable = true;
        private bool AllowPlaceOnNonFilledTiles;
        private BuildingDefinitionGroupId PipetteOverrideId;
        private bool AllowPlaceOnNotch;
        private int AutoAttractIOScoreMultiplier;
        private bool AutoConnect = true;
        private bool AutoRotateToFitStructures = true;
        private bool AllowNonForcingReplacementByOtherBuildings;
        private bool ShouldSkipReplacementIOChecks;
        private bool AlwaysProducesConflictIndicators;
        private bool RenderConflictIndicatorMeshes;
        private bool RenderConflictIndicatorVisualization;
        private bool RenderConnectorIndicators;
        private bool RenderConflictingConnectorIndicators;
        private bool ShowStatBeltProcessingTime;
        private bool ShowStatBuildingsPerFullBelt;
        private bool ShowInSpeedOverview;
        private bool ShowAsResearchReward;
        private UnlockableStoreContentId RequireStoreContentId;
        private WikiEntryId LinkedEntryId;
        private readonly List<Type> PlacementIndicators = new();
        private IEnumerable<IBuildingPlacementRequirement> PlacementRequirements;
        private MetaBuildingThroughputDisplayHelper ThroughputDisplayHelper;

        public IIdentifiableAndPresentableBuildingGroupBuilder WithPresentation(IText title,
            IText description,
            Sprite icon)
        {
            Title = title;
            Description = description;
            Icon = icon;
            return this;
        }

        public IIdentifiableAndTitledBuildingGroupBuilder WithTitle(IText title)
        {
            Title = title;
            return this;
        }

        public IIdentifiableTitledAndDescribedBuildingGroupBuilder WithDescription(
            IText description)
        {
            Description = description;
            return this;
        }

        public IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(Sprite icon)
        {
            Icon = icon;
            return this;
        }

        public IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(string filePath)
        {
            throw new NotImplementedException();
        }

        public IIdentifiableAndPresentableBuildingGroupBuilder WithIcon(Texture texture)
        {
            throw new NotImplementedException();
        }

        public IIdentifiablePresentableAndCategorizedBuildingGroupBuilder AsTransportableBuilding()
        {
            IsTransportBuilding = true;
            return this;
        }

        public IIdentifiablePresentableAndCategorizedBuildingGroupBuilder
            AsNonTransportableBuilding()
        {
            IsTransportBuilding = false;
            return this;
        }

        public IBuildingGroupBuilder WithPreferredPlacement(
            DefaultPreferredPlacementMode defaultPreferredPlacementMode)
        {
            DefaultPreferredPlacement = defaultPreferredPlacementMode;
            return this;
        }

        public IBuildingGroupBuilder WithConfig(bool isTransportBuilding,
            DefaultPreferredPlacementMode defaultPreferredPlacementMode)
        {
            IsTransportBuilding = isTransportBuilding;
            DefaultPreferredPlacement = defaultPreferredPlacementMode;
            return this;
        }

        public IBuildingGroupBuilder Removable()
        {
            IsRemovable = true;
            return this;
        }

        public IBuildingGroupBuilder NotRemovable()
        {
            IsRemovable = false;
            return this;
        }

        public IBuildingGroupBuilder Selectable()
        {
            IsSelectable = true;
            return this;
        }

        public IBuildingGroupBuilder NotSelectable()
        {
            IsSelectable = false;
            return this;
        }

        public IBuildingGroupBuilder Buildable()
        {
            IsBuildable = true;
            return this;
        }

        public IBuildingGroupBuilder NotBuildable()
        {
            IsBuildable = false;
            return this;
        }

        public IBuildingGroupBuilder AllowedOnNonFilledTiles()
        {
            AllowPlaceOnNonFilledTiles = true;
            return this;
        }

        public IBuildingGroupBuilder NotAllowedOnNonFilledTiles()
        {
            AllowPlaceOnNonFilledTiles = false;
            return this;
        }

        public IBuildingGroupBuilder AllowedOnNotches()
        {
            AllowPlaceOnNotch = true;
            return this;
        }

        public IBuildingGroupBuilder NotAllowedOnNotches()
        {
            AllowPlaceOnNotch = false;
            return this;
        }

        public IBuildingGroupBuilder AutoConnected()
        {
            AutoConnect = true;
            return this;
        }

        public IBuildingGroupBuilder NotAutoConnected()
        {
            AutoConnect = false;
            return this;
        }

        public IBuildingGroupBuilder AutoRotated()
        {
            AutoRotateToFitStructures = true;
            return this;
        }

        public IBuildingGroupBuilder NotAutoRotated()
        {
            AutoRotateToFitStructures = false;
            return this;
        }

        public IBuildingGroupBuilder AllowedToBeReplacedWithoutForce()
        {
            AllowNonForcingReplacementByOtherBuildings = true;
            return this;
        }

        public IBuildingGroupBuilder NotAllowedToBeReplacedWithoutForce()
        {
            AllowNonForcingReplacementByOtherBuildings = false;
            return this;
        }

        public IBuildingGroupBuilder RenderingConflictingIndicatorMeshes()
        {
            RenderConflictIndicatorMeshes = true;
            return this;
        }

        public IBuildingGroupBuilder NotRenderingConflictingIndicatorMeshes()
        {
            RenderConflictIndicatorMeshes = false;
            return this;
        }

        public IBuildingGroupBuilder RenderingConflictingIndicatorVisualization()
        {
            RenderConflictIndicatorVisualization = true;
            return this;
        }

        public IBuildingGroupBuilder NotRenderingConflictingIndicatorVisualization()
        {
            RenderConflictIndicatorVisualization = false;
            return this;
        }

        public IBuildingGroupBuilder ProducingConflictingIndicatorsAlways()
        {
            AlwaysProducesConflictIndicators = true;
            return this;
        }

        public IBuildingGroupBuilder NotProducingConflictingIndicatorsAlways()
        {
            AlwaysProducesConflictIndicators = false;
            return this;
        }

        public IBuildingGroupBuilder RenderingConnectorIndicators()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder NotRenderingConnectorIndicator()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder RenderingConnectorConflictIndicators()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder NotRenderingConnectorConflictIndicator()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder ShowingBeltProcessingTimeStat()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder NotShowingBeltProcessingTimeStat()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder ShowingBuildingsPerFullBeltStat()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder NotShowingBuildingsPerFullBeltStat()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder DisplayableAsReward()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder NotDisplayableAsReward()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder SkippingReplacementConnectorChecks()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder NotSkippingReplacementConnectorChecks()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder WithConnectionMultiplier(int autoAttractScore)
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder WithPipetteOverride(
            BuildingDefinitionGroupId overrideGroup)
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder WithoutPipetteOverride()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder WithPlacementIndicator<TPlacementIndicator>()
            where TPlacementIndicator : IBuildingPlacementIndicator
        {
            PlacementIndicators.Add(typeof(TPlacementIndicator));
            return this;
        }

        public IBuildingGroupBuilder WithoutPlacementIndicators()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder WithPlacementRequirements()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder WithoutPlacementRequirements()
        {
            throw new NotImplementedException();
        }

        public IBuildingGroupBuilder WithCustomThroughputDisplayHelper(
            MetaBuildingThroughputDisplayHelper throughputDisplayHelper)
        {
            ThroughputDisplayHelper = throughputDisplayHelper;
            return this;
        }

        public IBuildingGroupBuilder WithDefaultThroughputDisplayHelper()
        {
            return WithCustomThroughputDisplayHelper(new MetaBuildingThroughputDisplayHelper
            {
                LockToFirstDefinition = false,
                MeshSpacing = -0.1f,
                Slots = Array.Empty<MetaBuildingThroughputDisplayHelper.IOData>()
            });
        }

        public IBuildingGroupBuilder WithoutThroughputDisplayHelper()
        {
            throw new NotImplementedException();
        }


        public BuildingDefinitionGroup BuildAndRegister(GameBuildings gameBuildings)
        {
            Debugging.Logger.Info?.Log($"Registering {GroupId} to buildings");
            BuildingDefinitionGroup buildingGroup = new(GroupId,
                Icon,
                Title,
                Description,
                IsTransportBuilding,
                IsRemovable,
                IsSelectable,
                IsBuildable,
                AllowPlaceOnNonFilledTiles,
                PipetteOverrideId,
                DefaultPreferredPlacement,
                AllowPlaceOnNotch,
                AutoAttractIOScoreMultiplier,
                AutoConnect,
                AutoRotateToFitStructures,
                AllowNonForcingReplacementByOtherBuildings,
                ShouldSkipReplacementIOChecks,
                AlwaysProducesConflictIndicators,
                RenderConflictIndicatorMeshes,
                RenderConflictIndicatorVisualization,
                RenderConnectorIndicators,
                RenderConflictingConnectorIndicators,
                ShowStatBeltProcessingTime,
                ShowStatBuildingsPerFullBelt,
                ShowInSpeedOverview,
                ShowAsResearchReward,
                RequireStoreContentId,
                LinkedEntryId,
                PlacementIndicators.ToArray() ?? Array.Empty<Type>(),
                PlacementRequirements ?? Array.Empty<IBuildingPlacementRequirement>(),
                ThroughputDisplayHelper);

            gameBuildings._All.Add(buildingGroup);
            gameBuildings._VariantsById.Add(buildingGroup.Id, buildingGroup);
            return buildingGroup;
        }

        public IBuildingGroupBuilder WithConfig()
        {
            throw new NotImplementedException();
        }

        public IIdentifiableAndPresentableBuildingGroupBuilder WithDescription(Sprite icon)
        {
            throw new NotImplementedException();
        }
    }
}