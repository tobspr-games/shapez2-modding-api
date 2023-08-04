using System;

/// <summary>
/// Indicates that this mod should be executed after another mod
/// </summary>
/// <remarks>
/// Requires the other mod to acknowledge such restriction with <see cref="AcceptPrefixes"/> or
/// <see cref="MustExecuteAfterAttribute"/>. Otherwise, a mod could just bypass all others and execute first
/// </remarks>
public class MustExecuteAfterAttribute : Attribute
{
    public ModRef ModRef { get; }

    public MustExecuteAfterAttribute(ModRef modRef)
    {
        ModRef = modRef;
    }
}
