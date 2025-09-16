using System;
using System.Collections.Generic;
using System.Linq;
using Core.Collections.Scoped;

namespace ShapezShifter.Flow.Toolbar
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
                IParentToolbarElementData parent = ElementLocator.FindElementParent(toolbarData);
                Index relativeIndex = ElementLocator.LeafIndex(toolbarData);

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
                IParentToolbarElementData parent = ElementLocator.FindElementParent(toolbarData);
                Index relativeIndex = ElementLocator.LeafIndex(toolbarData);

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

            IParentToolbarElementData lastParent = toolbarData.RootToolbarElement;

            using ScopedList<NavigatedToolbarElement> navigatedParents =
                ScopedList<NavigatedToolbarElement>.Get();

            navigatedParents.Add(new NavigatedToolbarElement(0, lastParent));
            for (int i = 0; i < depth - 1; i++)
            {
                Index relativeIndex = elementLocator.IndexAtLevel(toolbarData, i);
                IEnumerable<IToolbarElementData> children =
                    lastParent.Children.Where(x => x is not ToolbarSlotSeparator);
                try
                {
                    if (!relativeIndex.IsFromEnd)
                    {
                        lastParent = (IParentToolbarElementData)children.ElementAt(relativeIndex.Value);
                    }
                    else
                    {
                        lastParent = (IParentToolbarElementData)
                            lastParent.Children.Reverse()
                               .Where(x => x is not ToolbarSlotSeparator)
                               .ElementAt(relativeIndex.Value - 1);
                    }

                    navigatedParents.Add(new NavigatedToolbarElement(relativeIndex, lastParent));
                }
                catch (Exception e)
                {
                    throw ToolbarQueryExceptionUtils.CreateDetailedException(navigatedParents, relativeIndex, e);
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
            using ScopedList<IToolbarElementData> childrenList = ScopedList<IToolbarElementData>.Get(parent.Children);
            childrenList.Insert(index, element);
            IToolbarElementData[] newChildren = childrenList.ToArray();

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