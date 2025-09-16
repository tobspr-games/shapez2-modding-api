using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Localization;

namespace ShapezShifter.Flow
{
    internal static class ToolbarQueryExceptionUtils
    {
        public static Exception CreateDetailedException(IReadOnlyList<NavigatedToolbarElement> route,
            Index failedIndex, Exception innerException)
        {
            StringBuilder messageBuilder = new();

            messageBuilder.AppendLine(
                "Failed to navigate through the entire toolbar hierarchy. Successful navigated through:");

            int identLevel = 1;

            foreach (NavigatedToolbarElement navigationNode in route)
            {
                Ident(messageBuilder, identLevel++);
                messageBuilder.AppendLine(
                    $"{LogElement(navigationNode.ParentToolbarElement)} ({navigationNode.Index})");
            }

            IToolbarElementData last = route[^1].ParentToolbarElement;
            if (last is IParentToolbarElementData parentToolbarElementData)
            {
                messageBuilder.AppendLine($"Failed to navigate to children {failedIndex}. Options were:");
                foreach (IToolbarElementData child in parentToolbarElementData.Children.Where(x =>
                             x is not ToolbarSlotSeparator))
                {
                    messageBuilder.Append($"{LogElement(child)}\t");
                }

                messageBuilder.AppendLine();
            }
            else
            {
                messageBuilder.AppendLine("Last element is a leaf and query should have stopped before");
            }


            return new ToolbarQueryException(messageBuilder.ToString(), innerException);

            string LogElement(IToolbarElementData toolbarElement)
            {
                return toolbarElement switch
                {
                    RootToolbarElementData => "Root",
                    IPresentableToolbarElementData presentableData => LogText(presentableData.Title),
                    _ => toolbarElement.ToString()
                };
            }

            string LogText(IText text)
            {
                return text switch
                {
                    LazyLocalizedText lazyLocalizedText => lazyLocalizedText.Id.Id,
                    RawText presentableToolbarElementData => presentableToolbarElementData.Text,
                    _ => text.ToString()
                };
            }
        }

        private static void Ident(StringBuilder stringBuilder, int i)
        {
            for (int j = 0; j < i; j++)
            {
                stringBuilder.Append("\t");
            }
        }
    }

    internal struct NavigatedToolbarElement
    {
        public readonly Index Index;
        public readonly IToolbarElementData ParentToolbarElement;

        public NavigatedToolbarElement(Index index, IParentToolbarElementData parentToolbarElement)
        {
            Index = index;
            ParentToolbarElement = parentToolbarElement;
        }
    }
}