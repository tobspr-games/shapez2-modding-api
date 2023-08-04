using System;

/// <summary>
/// Express hard dependency on the other mod being available. Ideally those dependencies will be solved automatically if
/// the reference can be found, otherwise mod won't load
/// </summary>
internal class RequiresModAttribute : Attribute
{
    public ModRef ModRef { get; }

    public RequiresModAttribute(in ModRef modModRef)
    {
        ModRef = modModRef;
    }
}
