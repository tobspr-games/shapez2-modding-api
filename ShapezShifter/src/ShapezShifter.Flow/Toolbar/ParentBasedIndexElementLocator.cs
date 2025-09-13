using System;

namespace ShapezShifter.Flow.Toolbar
{
    public static class ToolbarElementLocator
    {
        public static IToolbarElementLocator Root()
        {
            return new RootElementLocator();
        }

        public static IToolbarElementLocator ChildAt(this IToolbarElementLocator elementLocator,
            Index index)
        {
            return new ParentBasedIndexElementLocator(elementLocator, index);
        }
    }

    public class RootElementLocator : IToolbarElementLocator
    {
        public Index IndexAtLevel(ToolbarData toolbarData, int level)
        {
            return level != 0 ? throw new Exception() : 0;
        }

        public int Depth(ToolbarData toolbarData)
        {
            return 0;
        }
    }

    public class ParentBasedIndexElementLocator : IToolbarElementLocator
    {
        private readonly IToolbarElementLocator ParentLocator;
        private readonly Index Index;

        public ParentBasedIndexElementLocator(IToolbarElementLocator parentLocator, Index index)
        {
            ParentLocator = parentLocator;
            Index = index;
        }

        public Index IndexAtLevel(ToolbarData toolbarData, int level)
        {
            int maxLevel = Depth(toolbarData) - 1;
            if (level > maxLevel)
            {
                throw new Exception();
            }

            if (level < maxLevel)
            {
                return ParentLocator.IndexAtLevel(toolbarData, level);
            }

            return Index;
        }

        public int Depth(ToolbarData toolbarData)
        {
            return ParentLocator.Depth(toolbarData) + 1;
        }
    }
}