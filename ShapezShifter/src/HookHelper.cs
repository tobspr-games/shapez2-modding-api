using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using MonoMod.RuntimeDetour;

[PublicAPI]
public static class HookHelper
{
    public static Hook CreatePrefixHook<T>(Expression<Action<T>> original, Action prefix)
    {
        var actualMethodBody = GetRuntimeMethod(original);

        return new Hook(actualMethodBody, (Action<Action<T>, T>)Patch);

        void Patch(Action<T> orig, T self)
        {
            prefix();
            orig(self);
        }
    }

    public static Hook Delete<T>(Expression<Action<T>> original)
    {
        var actualMethodBody = GetRuntimeMethod(original);
        return new Hook(actualMethodBody, (Action<Action<T>, T>)Patch);

        static void Patch(Action<T> orig, T self)
        {
        }
    }


    public static Hook ReplaceHook<T>(Expression<Action<T>> original, Action<T> replacement)
    {
        var actualMethodBody = GetRuntimeMethod(original);
        return new Hook(actualMethodBody, replacement);
    }

    public static Hook CreatePostfixHook<T>(Expression<Action<T>> original, Action postfix)
    {
        var actualMethodBody = GetRuntimeMethod(original);
        return new Hook(actualMethodBody, (Action<Action<T>, T>)Patch);

        void Patch(Action<T> orig, T self)
        {
            orig(self);
            postfix();
        }
    }

    private static MethodInfo GetRuntimeMethod<T>(Expression<Action<T>> original)
    {
        var name = ((MethodCallExpression)original.Body).Method.Name;
        var actualMethodBody = typeof(T).GetMethod(name,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        if (actualMethodBody == null)
        {
            throw new Exception($"Could not find method {name} in type {typeof(T)} during runtime");
        }

        return actualMethodBody;
    }
}