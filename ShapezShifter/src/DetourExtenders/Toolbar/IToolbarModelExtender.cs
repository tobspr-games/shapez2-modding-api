namespace ShapezShifter
{
    /// <summary>
    /// Contract for modifying an existing toolbar after it has been built by <see cref="ToolbarBuilder"/> using a
    /// <see cref="ToolbarData"/>
    /// </summary>
    /// <remarks>
    /// Implementing this extender is more indicated when creating toolbar elements with extra functionality that does not
    /// exist in the base game. If are you trying to change the toolbar slot data or adding a new building consider using
    /// <see cref="IToolbarDataExtender"/>. If you are writing a custom scenario, the toolbar data can be specified on it 
    /// </remarks>
    public interface IToolbarModelExtender
    {
        /// <remark>
        /// When completely exchanging the toolbar by a new one, remember to dispose the discarded version. It is suboptimal
        /// but only the extenders can now if the version should and can be safely disposed  
        /// </remark>
        IParentToolbarElement ModifyToolbarData(IParentToolbarElement toolbarData);
    }
}