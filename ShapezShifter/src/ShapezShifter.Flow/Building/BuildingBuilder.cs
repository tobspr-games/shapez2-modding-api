using Game.Core.Simulation;

namespace ShapezShifter.Flow
{
    internal class BuildingBuilder :
        IBuildingBuilder,
        IIdentifiableBuildingBuilder,
        IIdentifiableConnectableBuildingBuilder,
        IIdentifiableConnectableDynamicallyRenderableBuildingBuilder,
        IIdentifiableConnectableRenderableBuildingBuilder,
        IIdentifiableConnectableRenderablePredictableBuildingBuilder,
        IIdentifiableConnectableRenderablePredictableAudibleBuildingBuilder,
        IIdentifiableConnectableRenderablePredictableAudibleConfigurableBuildingBuilder
    {
        private readonly BuildingDefinitionId DefinitionId;
        private IBuildingConnectorData ConnectorData;

        private BuildingDefinition BuildingDefinition;
        private BuildingDefinitionId? DefinitionToCopyFrom;

        public BuildingBuilder(BuildingDefinitionId id)
        {
            DefinitionId = id;
        }

        public IIdentifiableConnectableBuildingBuilder WithConnectorData(
            IBuildingConnectorData connectorData)
        {
            ConnectorData = connectorData;
            BuildingDefinition runtimeDefinition = new(DefinitionId, ConnectorData);
            runtimeDefinition.CustomData.Attach(ConnectorData);
            BuildingDefinition = runtimeDefinition;
            return this;
        }

        public IIdentifiableConnectableDynamicallyRenderableBuildingBuilder
            DynamicallyRendering<TRenderer, TSimulation, TDrawData>(
                TDrawData drawData)
            where TRenderer : StatelessBuildingSimulationRenderer<TSimulation, TDrawData>
            where TSimulation : ISimulation
            where TDrawData : IBuildingCustomDrawData
        {
            BuildingDefinition.CustomData.Attach(drawData);
            return this;
        }

        public IIdentifiableConnectableRenderableBuildingBuilder WithStaticDrawData(
            BuildingDrawData drawData)
        {
            BuildingDefinition.CustomData.Attach(drawData);
            return this;
        }

        public IIdentifiableConnectableRenderableBuildingBuilder WithCopiedStaticDrawData(
            BuildingDefinitionId definitionId)
        {
            DefinitionToCopyFrom = definitionId;
            return this;
        }

        public IIdentifiableConnectableRenderablePredictableBuildingBuilder WithPrediction(
            IBuildingOutputPredictor predictor)
        {
            BuildingDefinition.CustomData.Attach(predictor);
            return this;
        }

        public IIdentifiableConnectableRenderablePredictableBuildingBuilder WithoutPrediction()
        {
            BuildingDefinition.CustomData.Attach(new NoOutputPredictor());
            return this;
        }

        public IIdentifiableConnectableRenderablePredictableAudibleBuildingBuilder WithSound(
            BuildingSoundDefinition buildingSoundDefinition)
        {
            BuildingDefinition.CustomData.Attach(buildingSoundDefinition);
            return this;
        }

        public IIdentifiableConnectableRenderablePredictableAudibleBuildingBuilder WithoutSound()
        {
            BuildingDefinition.CustomData.Attach(
                new BuildingSoundDefinition(SoundLOD.None, SoundPriority.Disabled, null));
            return this;
        }

        public IIdentifiableConnectableRenderablePredictableAudibleConfigurableBuildingBuilder
            WithSimulationConfiguration(
                ICustomSimulationConfiguration customSimulationConfiguration)
        {
            BuildingDefinition.CustomData.Attach(customSimulationConfiguration);
            return this;
        }

        public IIdentifiableConnectableRenderablePredictableAudibleConfigurableBuildingBuilder
            WithoutSimulationConfiguration()
        {
            BuildingDefinition.CustomData.Attach(new EmptyCustomSimulationConfiguration());

            return this;
        }

        public IBuildingBuilder WithEfficiencyData(BuildingEfficiencyData buildingEfficiencyData)
        {
            BuildingEfficiencyData efficiencyData = new(2.0f, 1);
            BuildingDefinition.CustomData.Attach(efficiencyData);
            return this;
        }

        public BuildingDefinition BuildAndRegister(BuildingDefinitionGroup group,
            GameBuildings gameBuildings)
        {
            CopyBuildingRenderingFromExisting(gameBuildings);
            BindBuildingToGroup(group);

            gameBuildings._DefinitionsById.Add(BuildingDefinition.Id, BuildingDefinition);

            return BuildingDefinition;
        }

        private void CopyBuildingRenderingFromExisting(GameBuildings gameBuildings)
        {
            if (!DefinitionToCopyFrom.HasValue)
            {
                return;
            }

            IBuildingDefinition definitionToCopyFrom = gameBuildings.GetDefinition(DefinitionToCopyFrom.value);
            IBuildingDrawData drawDataReference = definitionToCopyFrom.CustomData.Get<IBuildingDrawData>();
            IBuildingCustomDrawData dynamicDrawData = BuildingDefinition.CustomData.Get<IBuildingCustomDrawData>();
            BuildingDrawData drawData = new(
                drawDataReference.RenderVoidBelow,
                drawDataReference.MainMeshPerLayer,
                drawDataReference.IsolatedBlueprintMesh,
                drawDataReference.CombinedBlueprintMesh,
                drawDataReference.PreviewMesh,
                drawDataReference.GlassMesh,
                drawDataReference.Colliders,
                dynamicDrawData,
                drawDataReference.HasCustomOverviewMesh,
                drawDataReference.CustomOverviewMesh,
                drawDataReference.SimulationRendererDrawsMainMesh);
            BuildingDefinition.CustomData.AttachOrReplace(drawData);
        }


        private void BindBuildingToGroup(BuildingDefinitionGroup group)
        {
            EntityPlacementPreferenceData placementPreferenceData = new(group.AutoConnect,
                group.AutoAttractIOScoreMultiplier);

            EntityReplacementPreferenceData replacementPreferenceData =
                new(
                    group.AllowNonForcingReplacementByOtherBuildings,
                    group.IsTransportBuilding,
                    group.ShouldSkipReplacementIOChecks);

            BuildingDefinition.CustomData.AttachOrReplace(group);
            BuildingDefinition.CustomData.AttachOrReplace(placementPreferenceData);
            BuildingDefinition.CustomData.AttachOrReplace(replacementPreferenceData);
            BuildingDefinition.CustomData.AttachOrReplace(group.DefaultPreferredPlacementMode);

            group.AddInternalVariant(BuildingDefinition);
        }
    }

    public interface IIdentifiableBuildingBuilder
    {
        IIdentifiableConnectableBuildingBuilder WithConnectorData(
            IBuildingConnectorData connectorData);
    }

    public interface IIdentifiableConnectableBuildingBuilder
    {
        IIdentifiableConnectableDynamicallyRenderableBuildingBuilder
            DynamicallyRendering<TRenderer, TSimulation, TDrawData>(
                TDrawData drawData)
            where TRenderer : StatelessBuildingSimulationRenderer<TSimulation, TDrawData>
            where TSimulation : ISimulation
            where TDrawData : IBuildingCustomDrawData;
    }

    public interface IIdentifiableConnectableDynamicallyRenderableBuildingBuilder
    {
        IIdentifiableConnectableRenderableBuildingBuilder WithStaticDrawData(
            BuildingDrawData drawData);

        IIdentifiableConnectableRenderableBuildingBuilder WithCopiedStaticDrawData(
            BuildingDefinitionId definitionId);
    }

    public interface IIdentifiableConnectableRenderableBuildingBuilder
    {
        IIdentifiableConnectableRenderablePredictableBuildingBuilder WithPrediction(
            IBuildingOutputPredictor predictor);

        IIdentifiableConnectableRenderablePredictableBuildingBuilder WithoutPrediction();
    }

    public interface IIdentifiableConnectableRenderablePredictableBuildingBuilder
    {
        IIdentifiableConnectableRenderablePredictableAudibleBuildingBuilder WithSound(
            BuildingSoundDefinition soundDefinition);

        IIdentifiableConnectableRenderablePredictableAudibleBuildingBuilder WithoutSound();
    }

    public interface IIdentifiableConnectableRenderablePredictableAudibleBuildingBuilder
    {
        IIdentifiableConnectableRenderablePredictableAudibleConfigurableBuildingBuilder
            WithSimulationConfiguration(
                ICustomSimulationConfiguration customSimulationConfiguration);

        IIdentifiableConnectableRenderablePredictableAudibleConfigurableBuildingBuilder
            WithoutSimulationConfiguration();
    }

    public interface IIdentifiableConnectableRenderablePredictableAudibleConfigurableBuildingBuilder
    {
        IBuildingBuilder WithEfficiencyData(BuildingEfficiencyData buildingEfficiencyData);
    }
}