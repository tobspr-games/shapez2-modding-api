using System;

/// <summary>
/// Indicates that the pre or post fix can be executed even when the original method was replaced
/// </summary>
/// <remarks>
/// This is incompatible with the <see cref="ReplaceAttribute"/> as it would just replace, not acknowledge the conflict
/// and disregard the previous patch
/// </remarks>
public class AcceptReplacements : Attribute
{

}
