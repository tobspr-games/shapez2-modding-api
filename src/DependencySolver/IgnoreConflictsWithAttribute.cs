using System;

/// <summary>
/// Indicates that a patch conflict can be ignored
/// </summary>
/// <remarks>
/// Requires a specific mod reference to make sure the developer actually tested against said mod
/// </remarks>
internal class IgnoreConflictsWith : Attribute
{
    public ModRef ModRef { get; }

    public IgnoreConflictsWith(in ModRef modRef)
    {
        ModRef = modRef;
    }
}
