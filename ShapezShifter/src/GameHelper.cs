namespace ShapezShifter
{
    public static class GameHelper
    {
        public static GameCore Core => (GameCore)StaticGameCoreAccessor.G;
    }
}