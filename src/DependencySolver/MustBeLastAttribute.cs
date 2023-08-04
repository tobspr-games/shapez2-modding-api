/// <summary>
/// Indicates that this patch must be the last to execute
/// </summary>
/// <remarks>
/// In case two mods must be last to execute, a conflict is fired. Conflicts do not prevent player from playing, but
/// the user will be warned and one of the mods will execute in second (do not rely on the order). If it is fatal, this
/// mod is not loaded
/// </remarks>
public class MustBeLastAttribute : PostfixAttribute
{
    public bool Fatal { get; }

    public MustBeLastAttribute(bool fatal)
    {
        Fatal = fatal;
    }
}
