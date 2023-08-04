/// <summary>
/// Indicates that this patch must be the first to execute
/// </summary>
/// <remarks>
/// In case two mods must be first to execute, a conflict is fired. Conflicts do not prevent player from playing, but
/// the user will be warned and one of the mods will execute in second (do not rely on the order). If it is fatal, this
/// mod is not loaded
/// </remarks>
public class MustBeFirstAttribute : PrefixAttribute
{
    public bool Fatal { get; }

    public MustBeFirstAttribute(bool fatal)
    {
        Fatal = fatal;
    }
}
