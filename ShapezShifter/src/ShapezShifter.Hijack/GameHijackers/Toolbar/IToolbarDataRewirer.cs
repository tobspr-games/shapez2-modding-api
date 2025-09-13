namespace ShapezShifter.Hijack
{
    /// <summary>
    /// Contract for modifying a <see cref="ToolbarData"/> before it is built by the <see cref="ToolbarBuilder"/> resulting
    /// in a <see cref="IParentToolbarElement"/> that populates a <see cref="ToolbarModel"/> 
    /// </summary>
    /// <remarks>
    /// Implementing this rewirer is more indicated when you trying to change the toolbar slot data or adding a new
    /// building. When creating toolbar elements with extra functionality that does not exist in the base game consider
    /// using <see cref="IToolbarModelRewirer"/>. If you are writing a custom scenario, the toolbar data can be specified on
    /// it 
    /// </remarks>
    public interface IToolbarDataRewirer : IRewirer
    {
        ToolbarData ModifyToolbarData(ToolbarData toolbarData);
    }
}