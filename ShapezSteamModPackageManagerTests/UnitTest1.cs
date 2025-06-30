namespace ShapezSteamModPackageManagerTests;

public class Tests
{
    [Test]
    public void AssertGraphDependenciesTopologicalOrderIsValid()
    {
        var dependencies = new List<DependencyRelation<string, string>>
        {
            new("Mod1", "Harmony"),
            new("Mod2", "Mod1"),
            new("Mod2", "Shapez2API"),
            new("Shapez2API", "Harmony"),
            new("Harmony", "MonoMod"),
            new("Mod2", "Mod3"),
            new("Mod3", "MonoMod")
        };
        using var graphResolver = new GraphResolver<string>(dependencies);

        var result = graphResolver.ResolveTopologicalOrder().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo("MonoMod"));
            Assert.That(result.IndexOf("Mod1"), Is.GreaterThan(result.IndexOf("Harmony")));
            Assert.That(result.IndexOf("Mod2"), Is.GreaterThan(result.IndexOf("Mod1")));
            Assert.That(result.IndexOf("Mod2"), Is.GreaterThan(result.IndexOf("Shapez2API")));
            Assert.That(result.IndexOf("Shapez2API"), Is.GreaterThan(result.IndexOf("Harmony")));
            Assert.That(result.IndexOf("Harmony"), Is.GreaterThan(result.IndexOf("MonoMod")));
            Assert.That(result.IndexOf("Mod2"), Is.GreaterThan(result.IndexOf("Mod3")));
            Assert.That(result.IndexOf("Mod3"), Is.GreaterThan(result.IndexOf("MonoMod")));
        });
    }

    [Test]
    public void AssertGraphDependencyToSubDependencyResolves()
    {
        var dependencies = new List<DependencyRelation<string, string>>
        {
            new("Mod1", "Harmony"),
            new("Mod1", "MonoMod"),
            new("Mod1", "ShapezShifter"),
            new("ShapezShifter", "Harmony"),
            new("Harmony", "MonoMod")
        };

        using var graphResolver = new GraphResolver<string>(dependencies);
        var result = graphResolver.ResolveTopologicalOrder().ToList();

        Assert.That(result[0], Is.EqualTo("MonoMod"));
        Assert.That(result[1], Is.EqualTo("Harmony"));
        Assert.That(result[2], Is.EqualTo("ShapezShifter"));
        Assert.That(result[3], Is.EqualTo("Mod1"));
    }
}