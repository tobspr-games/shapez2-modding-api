using System;

namespace ShapezShifter.Flow.Toolbar
{
    public interface IToolbarElementLocator
    {
        public Index IndexAtLevel(int level);

        public int Depth();

        public Index LeafIndex()
        {
            return IndexAtLevel(Depth() - 1);
        }
    }
}