using Core.Events;

namespace ShapezShifter
{
    public static class ShapezCallbackExt
    {
        public static IEvent OnPreGameStart { get; internal set; }

        public static IEvent OnPostGameStart { get; internal set; }
    }
}