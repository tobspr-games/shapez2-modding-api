using System;

/// <summary>
/// Indicates that this mod should be executed before another mod
/// </summary>
/// <remarks>
/// Requires the other mod to acknowledge such restriction with <see cref="AcceptPostfixes"/> or
/// <see cref="MustExecuteAfterAttribute"/> referencing this mod. Otherwise, a mod could just bypass all others and
/// execute first
/// </remarks>
public class MustExecuteBeforeAttribute : Attribute
{
    public ModRef ModRef { get; }

    public MustExecuteBeforeAttribute(ModRef modRef)
    {
        ModRef = modRef;
    }
}
