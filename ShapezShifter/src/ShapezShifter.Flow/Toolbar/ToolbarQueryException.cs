using System;

namespace ShapezShifter.Flow
{
    public class ToolbarQueryException : Exception
    {
        public ToolbarQueryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}