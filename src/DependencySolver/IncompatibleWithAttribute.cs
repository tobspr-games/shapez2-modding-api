using System;

/// <summary>
/// Indicates that this mod does not work with another specific mod. Use this in any method
/// </summary>
public class IncompatibleWithAttribute : Attribute
{
    private readonly bool Fatal;
    public ModRef ModRef { get; }

    public IncompatibleWithAttribute(ModRef modRef, bool fatal)
    {
        Fatal = fatal;
        ModRef = modRef;
    }
}
