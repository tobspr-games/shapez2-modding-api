public interface IMod
{
    /// <summary>
    /// Called when the mod is loaded. Use this to hook to any callback and patch the desired methods
    /// </summary>
    void Init();
}
