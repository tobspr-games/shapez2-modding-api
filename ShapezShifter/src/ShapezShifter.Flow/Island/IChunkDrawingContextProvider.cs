using Game.Core.Coordinates;

namespace ShapezShifter.Flow
{
    public interface IChunkDrawingContextProvider
    {
        ChunkPlatformDrawingContext DrawingContextForChunk(ChunkVector chunk);
    }
}