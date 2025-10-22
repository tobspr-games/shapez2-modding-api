using System.Collections.Generic;
using Core.Factory;
using Game.Core.Coordinates;
using Game.Core.Rendering.Islands.PlayingField;
using Game.Core.Research;

namespace ShapezShifter.Flow
{
    internal class IslandBuilder :
        IIslandBuilder,
        IIdentifiableIslandBuilder,
        IIdentifiableMappableIslandBuilder,
        IIdentifiableMappableConnectableIslandBuilder,
        IIdentifiableMappableConnectableInteractableIslandBuilder,
        IIdentifiableMappableConnectableInteractableCostingIslandBuilder
    {
        private readonly IslandDefinitionId DefinitionId;
        private ChunkLayoutLookup<ChunkVector, IslandChunkData> Layout;

        private IslandDefinition IslandDefinition;

        public IslandBuilder(IslandDefinitionId id)
        {
            DefinitionId = id;
        }

        public IIdentifiableMappableIslandBuilder WithLayout(ChunkLayoutLookup<ChunkVector, IslandChunkData> layout)
        {
            Layout = layout;
            IslandDefinition = new IslandDefinition(DefinitionId, Layout);
            IslandDefinition.CustomData.Attach(new LambdaFactory<IIslandConfiguration>(() => null));

            return this;
        }

        public IIdentifiableMappableConnectableIslandBuilder WithConnectorData(
            IIslandConnectorData connectorData)
        {
            IslandDefinition.CustomData.Attach(connectorData);
            return this;
        }

        public IslandDefinition BuildAndRegister(IslandDefinitionGroup islandGroup,
            GameIslands gameIslands)
        {
            BindGroupToIsland(IslandDefinition, islandGroup);
            BindIslandToGroup(islandGroup, IslandDefinition);

            gameIslands.AllDefinitions.Add(IslandDefinition);
            gameIslands.DefinitionsById.Add(IslandDefinition.Id, IslandDefinition);

            return IslandDefinition;
        }

        private static void BindGroupToIsland(IslandDefinition island, IslandDefinitionGroup group)
        {
            IPresentationData groupPresentationData = group.CustomData.Get<IPresentationData>();
            IslandPresentationData islandPresentationData = new(groupPresentationData.Title,
                groupPresentationData.Description,
                WikiEntryId.Empty,
                groupPresentationData.Icon,
                groupPresentationData.ShowAsReward,
                false,
                UnlockableStoreContentId.Empty);


            island.CustomData.AttachOrReplace(islandPresentationData);
            island.CustomData.AttachOrReplace(group);
            island.CustomData.AttachOrReplace(group.Id);
        }

        private static void BindIslandToGroup(IslandDefinitionGroup group, IslandDefinition island)
        {
            IslandGroupCollection groupCollection = group.CustomData.TryGet(out IslandGroupCollection collection)
                ? collection
                : new IslandGroupCollection();
            groupCollection.AddIslandDefinition(island);
            group.CustomData.AttachOrReplace(groupCollection);
        }


        public IIdentifiableMappableConnectableInteractableIslandBuilder WithInteraction(
            bool flippable,
            bool canHoldBuildings,
            bool allowNonForcingReplacement = false,
            bool skipReplacementConnectorChecks = false,
            bool isTransportBuilding = false,
            bool selectable = true,
            bool buildable = true)
        {
            IslandDefinition.CustomData.Attach(new IslandInteractionConfig(selectable, buildable, flippable));
            IslandDefinition.CustomData.Attach(new EntityReplacementPreferenceData(allowNonForcingReplacement,
                isTransportBuilding,
                skipReplacementConnectorChecks));

            if (canHoldBuildings)
            {
                IslandDefinition.CustomData.AddFlag<CanHoldBuildingsIslandTag>();
            }

            return this;
        }

        public IIdentifiableMappableConnectableInteractableCostingIslandBuilder WithCustomChunkCost(
            ChunkLimitCurrency totalChunkCost)
        {
            IslandDefinition.CustomData.Attach(new ChunkCostProvider(totalChunkCost));
            return this;
        }

        public IIdentifiableMappableConnectableInteractableCostingIslandBuilder WithDefaultChunkCost()
        {
            int chunks = IslandDefinition.Layout.ChunkPositions.Length;
            const int costPerChunk = 1;
            return WithCustomChunkCost(new ChunkLimitCurrency(chunks * costPerChunk));
        }

        public IIslandBuilder WithRenderingOptions(IChunkDrawingContextProvider drawingContextProvider,
            bool drawPlayingField)
        {
            var dictionary = new Dictionary<ChunkVector, ChunkPlatformDrawingContext>();
            foreach (ChunkVector chunkVector in IslandDefinition.Layout.ChunkPositions)
            {
                dictionary.Add(chunkVector, drawingContextProvider.DrawingContextForChunk(chunkVector));
            }

            IslandDefinition.CustomData.Attach(new IslandFrameDrawData(dictionary));
            if (drawPlayingField)
            {
                IslandDefinition.CustomData.AddFlag<DrawPlayingFieldFlag>();
            }

            return this;
        }
    }

    public interface IIdentifiableIslandBuilder
    {
        IIdentifiableMappableIslandBuilder WithLayout(ChunkLayoutLookup<ChunkVector, IslandChunkData> layout);
    }

    public interface IIdentifiableMappableIslandBuilder
    {
        IIdentifiableMappableConnectableIslandBuilder WithConnectorData(
            IIslandConnectorData connectorData);
    }

    public interface IIdentifiableMappableConnectableIslandBuilder
    {
        IIdentifiableMappableConnectableInteractableIslandBuilder WithInteraction(bool flippable,
            bool canHoldBuildings,
            bool allowNonForcingReplacement = false,
            bool skipReplacementConnectorChecks = false,
            bool isTransportBuilding = false,
            bool selectable = true,
            bool buildable = true);
    }

    public interface IIdentifiableMappableConnectableInteractableIslandBuilder
    {
        IIdentifiableMappableConnectableInteractableCostingIslandBuilder WithCustomChunkCost(
            ChunkLimitCurrency totalChunkCost);

        IIdentifiableMappableConnectableInteractableCostingIslandBuilder WithDefaultChunkCost();
    }

    public interface IIdentifiableMappableConnectableInteractableCostingIslandBuilder
    {
        IIslandBuilder WithRenderingOptions(IChunkDrawingContextProvider drawingContextProvider, bool drawPlayingField);
    }
}