namespace ShapezShifterTestsThirdParty;

public sealed class PrivateMembersTestClass(int field)
{
    // ReSharper disable once ReplaceWithPrimaryConstructorParameter
    private readonly int Field = field;

    private int Property => Field * 2;
}

public readonly struct PrivateMembersTestStructure(int field)
{
    private readonly int Field = field;

    private int Property => Field * 2;
}