using System;
using System.Linq;

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
        public Index IndexAtLevel(int level)
        {
            return level != 0 ? throw new Exception() : 0;
        }

        public int Depth()
        {
            return 0;
        }

        public override string ToString()
        {
            return "Root";
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

        public Index IndexAtLevel(int level)
        {
            int maxLevel = Depth() - 1;
            if (level > maxLevel)
            {
                throw new Exception();
            }

            if (level < maxLevel)
            {
                return ParentLocator.IndexAtLevel(level);
            }

            return Index;
        }

        public int Depth()
        {
            return ParentLocator.Depth() + 1;
        }

        public override string ToString()
        {
            return $"{ParentLocator}\n" +
                   new string(Enumerable.Repeat('\t', Depth()).ToArray()) +
                   $"{Index}";
        }
    }
}