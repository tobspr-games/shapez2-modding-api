using System;
using System.Collections.Generic;
using Core.Localization;
using Game.Core.Research;
using UnityEngine;

namespace ShapezShifter.Buildings
{
    public static class BuildingHelper
    {
        public static BuildingDefinitionGroup CreateBuildingGroup(string id, Sprite icon,
            IText title,
            IText description,
            bool isTransportBuilding,
            DefaultPreferredPlacementMode defaultPreferredPlacementMode,
            bool removable = true,
            bool selectable = true,
            bool playerBuildable = true,
            bool allowPlaceOnNonFilledTiles = false,
            BuildingDefinitionGroupId pipetteOverrideId = default,
            bool allowPlaceOnNotch = false,
            int autoAttractIOScoreMultiplier = 100,
            bool autoConnect = true,
            bool autoRotateToFitStructures = true,
            bool allowNonForcingReplacementByOtherBuildings = false,
            bool shouldSkipReplacementIOChecks = false,
            bool alwaysProducesConflictIndicators = true,
            bool renderConflictIndicatorMeshes = true,
            bool renderConflictIndicatorVisualization = true,
            bool renderConnectorIndicators = true,
            bool renderConflictingConnectorIndicators = true,
            bool showStatBeltProcessingTime = true,
            bool showStatBuildingsPerFullBelt = true,
            bool showInSpeedOverview = true,
            bool showAsResearchReward = true,
            UnlockableStoreContentId requireStoreContentId = default,
            WikiEntryId linkedEntryId = default,
            IEnumerable<Type> placementIndicators = null,
            IEnumerable<IBuildingPlacementRequirement> placementRequirements = null,
            MetaBuildingThroughputDisplayHelper throughputDisplayHelper = null)
        {
            BuildingDefinitionGroup definition = new(new BuildingDefinitionGroupId(id), icon, title,
                description,
                isTransportBuilding, removable, selectable, playerBuildable,
                allowPlaceOnNonFilledTiles,
                pipetteOverrideId, defaultPreferredPlacementMode, allowPlaceOnNotch,
                autoAttractIOScoreMultiplier,
                autoConnect, autoRotateToFitStructures, allowNonForcingReplacementByOtherBuildings,
                shouldSkipReplacementIOChecks, alwaysProducesConflictIndicators,
                renderConflictIndicatorMeshes,
                renderConflictIndicatorVisualization, renderConnectorIndicators,
                renderConflictingConnectorIndicators,
                showStatBeltProcessingTime, showStatBuildingsPerFullBelt, showInSpeedOverview,
                showAsResearchReward,
                requireStoreContentId, linkedEntryId,
                placementIndicators ?? Array.Empty<Type>(),
                placementRequirements ?? Array.Empty<IBuildingPlacementRequirement>(),
                throughputDisplayHelper ?? new ThroughputDisplayHelperMock());

            return definition;
        }
    }

    public class ThroughputDisplayHelperMock : MetaBuildingThroughputDisplayHelper
    {
    }
}