using System.Linq;
using Core.Collections.Scoped;
using UnityEngine;

namespace ShapezShifter.Fluent.Toolbar
{
    public interface IToolbarEntryInsertLocation
    {
        public void AddEntry(ToolbarData toolbarData, IToolbarElementData elementData);
    }

    public static class ToolbarEntryLocation
    {
        public static IToolbarEntryInsertLocation InsertBefore(
            this IToolbarElementLocator elementLocator)
        {
            return new Before(elementLocator);
        }

        public static IToolbarEntryInsertLocation InsertAfter(
            this IToolbarElementLocator elementLocator)
        {
            return new After(elementLocator);
        }


        private class Before : IToolbarEntryInsertLocation
        {
            private readonly IToolbarElementLocator ElementLocator;

            public Before(IToolbarElementLocator elementLocator)
            {
                ElementLocator = elementLocator;
            }

            void IToolbarEntryInsertLocation.AddEntry(ToolbarData toolbarData,
                IToolbarElementData elementData)
            {
                var parent = ElementLocator.FindElementParent(toolbarData);
                var relativeIndex = ElementLocator.LeafIndex(toolbarData);

                int absoluteIndex = relativeIndex.IsFromEnd
                    ? parent.Children.Count() - relativeIndex.Value
                    : relativeIndex.Value;

                parent.InsertAtIndex(elementData, absoluteIndex);
            }
        }

        private class After : IToolbarEntryInsertLocation
        {
            private readonly IToolbarElementLocator ElementLocator;

            public After(IToolbarElementLocator elementLocator)
            {
                ElementLocator = elementLocator;
            }

            void IToolbarEntryInsertLocation.AddEntry(ToolbarData toolbarData,
                IToolbarElementData elementData)
            {
                var parent = ElementLocator.FindElementParent(toolbarData);
                var relativeIndex = ElementLocator.LeafIndex(toolbarData);

                int absoluteIndex = relativeIndex.IsFromEnd
                    ? parent.Children.Count() - relativeIndex.Value
                    : relativeIndex.Value;

                absoluteIndex++;
                parent.InsertAtIndex(elementData, absoluteIndex);
            }
        }
    }

    public static class ToolbarEntryExtensions
    {
        public static IParentToolbarElementData FindElementParent(
            this IToolbarElementLocator elementLocator, ToolbarData toolbarData)
        {
            int depth = elementLocator.Depth(toolbarData);
            Debug.Log(depth);

            IParentToolbarElementData lastParent = toolbarData.RootToolbarElement;

            for (int i = 0; i < depth - 1; i++)
            {
                var relativeIndex = elementLocator.IndexAtLevel(toolbarData, i);
                if (!relativeIndex.IsFromEnd)
                {
                    lastParent = (IParentToolbarElementData)
                        lastParent.Children.ElementAt(relativeIndex.Value);
                }
                else
                {
                    lastParent = (IParentToolbarElementData)
                        lastParent.Children.Reverse().ElementAt(relativeIndex.Value - 1);
                }
            }

            return lastParent;
        }
    }

    public static class ToolbarInsertLocationExtensions
    {
        public static void InsertAtIndex(this IParentToolbarElementData parent,
            IToolbarElementData element, int index)
        {
            using var childrenList = ScopedList<IToolbarElementData>.Get(parent.Children);
            childrenList.Insert(index, element);
            var newChildren = childrenList.ToArray();

            switch (parent)
            {
                case RootToolbarElementData rootToolbarElementData:
                    rootToolbarElementData.Children = newChildren;
                    break;
                case ParentToolbarElementData parentToolbarElementData:
                    parentToolbarElementData.Children = newChildren;
                    break;
            }
        }
    }
}