using ShapezShifterTestsThirdParty;

namespace ShapezShifterTests;

public class Tests
{
    [Test]
    public void AssertPublicClassFieldCanBeTypeSafelySetToArbitraryValue()
    {
        var testClass = new PublicMembersTestClass(0);

        using var hook = testClass.Set(x => x.Field, 42);

        Assert.That(testClass.Field, Is.EqualTo(42));
    }

    [Test]
    public void AssertPublicStructFieldCanBeTypeSafelySetToArbitraryValue()
    {
        var testStructure = new PublicMembersTestStructure(0);

        using var hook = testStructure.Set(x => x.Field, 42);

        Assert.That(testStructure.Field, Is.EqualTo(42));
    }

    [Test]
    public void AssertPublicClassPropertyCanBeTypeSafelyReplaceWithArbitraryGetter()
    {
        var testClass = new PublicMembersTestClass(10);

        Assert.That(testClass.Property, Is.EqualTo(20));

        using var hook = testClass.Set(x => x.Property, 21);

        Assert.That(testClass.Property, Is.EqualTo(21));
    }

    [Test]
    public void AssertPublicStructPropertyCanBeTypeSafelyReplaceWithArbitraryGetter()
    {
        var testStruct = new PublicMembersTestStructure(10);

        Assert.That(testStruct.Property, Is.EqualTo(20));

        using var hook = testStruct.Set(x => x.Property, 21);

        Assert.That(testStruct.Property, Is.EqualTo(21));
    }

    [Test]
    public void AssertPrivateClassFieldCanBeTypeSafelySetToArbitraryValue()
    {
        var testClass = new PrivateMembersTestClass(0);

        using var hook = testClass.Set(x => x.Field, 42);

        Assert.That(testClass.Field, Is.EqualTo(42));
    }

    [Test]
    public void AssertPrivateStructFieldCanBeTypeSafelySetToArbitraryValue()
    {
        var testStructure = new PrivateMembersTestStructure(0);

        using var hook = testStructure.Set(x => x.Field, 42);

        Assert.That(testStructure.Field, Is.EqualTo(42));
    }

    [Test]
    public void AssertPrivateClassPropertyCanBeTypeSafelyReplaceWithArbitraryGetter()
    {
        var testClass = new PrivateMembersTestClass(10);

        Assert.That(testClass.Property, Is.EqualTo(20));

        using var hook = testClass.Set(x => x.Property, 21);

        Assert.That(testClass.Property, Is.EqualTo(21));
    }

    [Test]
    public void AssertPrivateStructPropertyCanBeTypeSafelyReplaceWithArbitraryGetter()
    {
        var testStruct = new PublicMembersTestStructure(10);

        Assert.That(testStruct.Property, Is.EqualTo(20));

        using var hook = testStruct.Set(x => x.Property, 21);

        Assert.That(testStruct.Property, Is.EqualTo(21));
    }

    private class PublicMembersTestClass(int field)
    {
        public readonly int Field = field;

        public int Property => Field * 2;
    }

    private readonly struct PublicMembersTestStructure(int field)
    {
        public readonly int Field = field;

        public int Property => Field * 2;
    }
}