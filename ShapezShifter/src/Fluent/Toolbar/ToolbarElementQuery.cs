using System;

namespace ShapezShifter.Fluent.Toolbar
{
    public interface IToolbarElementLocator
    {
        public Index IndexAtLevel(ToolbarData toolbarData, int level);

        public int Depth(ToolbarData toolbarData);

        public Index LeafIndex(ToolbarData toolbarData)
        {
            return IndexAtLevel(toolbarData, Depth(toolbarData) - 1);
        }
    }
}