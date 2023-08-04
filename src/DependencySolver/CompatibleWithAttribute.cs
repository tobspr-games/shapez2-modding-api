using System;

/// <summary>
/// Indicates that two mods are compatible when they do not have hard requirements in place
/// </summary>
/// <remarks>
/// When two mods modify the same method, by prefix, postfix and they have no attributes to resolve conflicts, a
/// conflict will happen. Use this in both to acknowledge that there are no hard requirements between the two. I.e.,
/// just a callback
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public class CompatibleWithAttribute : Attribute
{
    public ModRef ModRef { get; }

    public CompatibleWithAttribute(ModRef modRef)
    {
        ModRef = modRef;
    }
}
