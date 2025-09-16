using Game.Core.Coordinates;

namespace ShapezShifter.Flow
{
    public class HomogeneousChunkDrawing : IChunkDrawingContextProvider
    {
        private readonly ChunkPlatformDrawingContext ChunkPlatformDrawingContext;

        public HomogeneousChunkDrawing(ChunkPlatformDrawingContext chunkPlatformDrawingContext)
        {
            ChunkPlatformDrawingContext = chunkPlatformDrawingContext;
        }

        public ChunkPlatformDrawingContext DrawingContextForChunk(ChunkVector chunk)
        {
            return ChunkPlatformDrawingContext;
        }
    }
}