namespace ShapezShifter.Kit
{
    public static class GameHelper
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static IGameSessionManagers Core => StaticGameCoreAccessor.G;
#pragma warning restore CS0618 // Type or member is obsolete
    }
}