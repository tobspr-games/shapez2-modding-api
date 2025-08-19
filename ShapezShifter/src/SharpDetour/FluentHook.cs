using System;
using System.Reflection;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace ShapezShifter
{
    public readonly struct FluentHook
    {
        public static HookPromiseInstance<TObject> Targeting<TObject>()
        {
            return new HookPromiseInstance<TObject>();
        }
    }

    public static class FluentHookTest
    {
        public static void Test()
        {
            MethodInfo original = typeof(Test).GetMethod("MultiplyAndCeilToInt",
                                      BindingFlags.Public | BindingFlags.Instance) ??
                                  throw new InvalidOperationException();

            using Hook hook = new(original,
                typeof(FluentHookTest).GetMethod(nameof(Patch)) ??
                throw new InvalidOperationException());

            using Hook hook2 = DetourHelper.CreatePostfixHook<Test, float, float, int>(
                (test, f1, f2) => test.MultiplyAndCeilToInt(f1, f2),
                AddOne);

            using Hook hook3 = FluentHook
                .Targeting<Test>()
                .WithArg<float>()
                .WithArg<float>()
                .Returning<int>()
                .Detour((test, f1, f2) => test.MultiplyAndCeilToInt(f1, f2))
                .Postfix(AddOne);

            int Patch(Func<Test, float, float, int> orig, Test self, float arg0,
                float arg1)
            {
                int value = orig(self, arg0, arg1);
                return AddOne(self, arg0, arg1, value);
            }
        }

        private static int AddOne(Test arg1, float arg2, float arg3, int result)
        {
            return result + 1;
        }
    }

    public class Test
    {
        public int MultiplyAndCeilToInt(float a, float b)
        {
            return Mathf.CeilToInt(a * b);
        }
    }
}