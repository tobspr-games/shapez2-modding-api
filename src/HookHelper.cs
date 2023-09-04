using MonoMod.RuntimeDetour;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

public static class HookHelper
{
    public static Hook CreatePrefixHook<T>(Expression<Action<T>> original, Action postfix)
    {
        return new Hook(original.Body, (Action<Action<T>, T>)Patch);

        void Patch(Action<T> orig, T self)
        {
            postfix();
            orig(self);
        }
    }


    public static MethodInfo EmptyMethod(Type type)
    {
        return type.GetMethod(nameof(Empty)).MakeGenericMethod(type);
    }

    public static MethodInfo EmptyMethod<T>()
    {
        return EmptyMethod(typeof(T));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Empty<T>()
    {

    }

    public static Hook Delete<T>(Expression<Action<T>> original)
    {
        static void Patch(Action<T> orig, T self)
        {
        }
        return new Hook(original.Body, (Action<Action<T>, T>)Patch);
    }

    public static Hook ReplaceHook<T>(Expression<Action<T>> original, Action replacement)
    {
        return new Hook(original.Body, replacement);
    }

    public static Hook CreatePostfixHook<T>(Expression<Action<T>> original, Action postfix)
    {
        return new Hook(original.Body, (Action<Action<T>, T>)Patch);

        void Patch(Action<T> orig, T self)
        {
            orig(self);
            postfix();
        }
    }
}
