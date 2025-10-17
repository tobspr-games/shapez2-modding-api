using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifterTests;

public class DetourHelperTests
{
    [Test]
    public void AssertArgumentLessMethodDetoursToPrefixBeforeOriginalExecution()
    {
        TestClass testClass = new(10);
        using Hook? hook = DetourHelper.CreatePrefixHook<TestClass>(t => t.Double(), Sum);

        testClass.Double();

        Assert.That(testClass.Val,
            Is.Not.EqualTo(24),
            () => "Hook failed to execute before method");
        Assert.That(testClass.Val, Is.EqualTo(28), () => "Hook failed to execute");

        return;

        void Sum(TestClass cls)
        {
            cls.Val += 4;
        }
    }

    [Test]
    public void AssertArgumentLessMethodDetoursToPostfixAfterOriginalExecution()
    {
        TestClass testClass = new(5);
        using Hook? hook = DetourHelper.CreatePostfixHook<TestClass>(t => t.Double(), Sum);

        testClass.Double();

        Assert.That(testClass.Val, Is.Not.EqualTo(16), () => "Hook failed to execute after method");
        Assert.That(testClass.Val, Is.EqualTo(13), () => "Hook failed to execute");

        return;

        void Sum(TestClass cls)
        {
            cls.Val += 3;
        }
    }

    [Test]
    public void AssertMethodWithArgumentsPrefixPatchesArgumentBeforeExecution()
    {
        TestClass2 testClass = new();
        using Hook? hook =
            DetourHelper.CreatePrefixHook<TestClass2, int>((tc, value) => tc.Store(value),
                Patch);

        testClass.Store(1);

        Assert.That(testClass.Value, Is.EqualTo(10), () => "Hook failed to execute before method");

        return;

        int Patch(int arg)
        {
            return arg * 10;
        }
    }

    [Test]
    public void AssertMethodWithReturnValuePostfixPatchesResultAfterExecution()
    {
        TestClass3 testClass = new(16);
        using Hook? hook =
            DetourHelper.CreatePostfixHook<TestClass3, int>(tc => tc.Double(), Patch);

        int value = testClass.Double();

        Assert.That(value, Is.EqualTo(16), () => "Hook failed to execute before method");

        return;

        int Patch(TestClass3 testClass3, int v)
        {
            return v / 2;
        }
    }

    private class TestClass(int val)
    {
        public int Val = val;

        public void Double()
        {
            Val *= 2;
        }
    }

    private class TestClass2
    {
        public int Value;

        public void Store(int value)
        {
            Value = value;
        }
    }

    private class TestClass3(int val)
    {
        public readonly int Val = val;

        public int Double()
        {
            return Val * 2;
        }
    }
}