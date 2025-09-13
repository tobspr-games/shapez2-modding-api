using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using MonoMod.RuntimeDetour;

namespace ShapezShifter.SharpDetour
{
    /// <summary>
    /// Collection of type-safe (ish) methods designed to help creating detours
    /// </summary>
    /// <remarks>
    /// Notice that the implementation uses a lot of reflection (so does MonoMod) and thus we are very limited in what we
    /// can validate statically
    /// </remarks>
    [PublicAPI]
    public static class DetourHelper
    {
        #region Prefix with no return

        public static Hook CreatePrefixHook<TObject>(Expression<Action<TObject>> original,
            Action<TObject> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody, (Action<Action<TObject>, TObject>)Patch);

            void Patch(Action<TObject> orig, TObject self)
            {
                prefix(self);
                orig(self);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0>(
            Expression<Action<TObject, TArg0>> original,
            Func<TArg0, TArg0> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0>, TObject, TArg0>)Patch);

            void Patch(Action<TObject, TArg0> orig, TObject self, TArg0 arg0)
            {
                arg0 = prefix(arg0);
                orig(self, arg0);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1>(
            Expression<Action<TObject, TArg0, TArg1>> original,
            Func<TObject, TArg0, TArg1, (TArg0, TArg1)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1>, TObject, TArg0, TArg1>)Patch);

            void Patch(Action<TObject, TArg0, TArg1> orig, TObject self, TArg0 arg0, TArg1 arg1)
            {
                (arg0, arg1) = prefix(self, arg0, arg1);
                orig(self, arg0, arg1);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original,
            Func<TObject, TArg0, TArg1, TArg2, (TArg0, TArg1, TArg2)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1, TArg2>, TObject, TArg0, TArg1, TArg2>)Patch);

            void Patch(Action<TObject, TArg0, TArg1, TArg2> orig, TObject self, TArg0 arg0,
                TArg1 arg1, TArg2 arg2)
            {
                (arg0, arg1, arg2) = prefix(self, arg0, arg1, arg2);
                orig(self, arg0, arg1, arg2);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, (TArg0, TArg1, TArg2, TArg3)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3>, TObject, TArg0, TArg1, TArg2,
                    TArg3>)Patch);

            void Patch(Action<TObject, TArg0, TArg1, TArg2, TArg3> orig, TObject self, TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3)
            {
                (arg0, arg1, arg2, arg3) = prefix(self, arg0, arg1, arg2, arg3);
                orig(self, arg0, arg1, arg2, arg3);
            }
        }

        #endregion

        #region Prefix with return

        public static Hook CreatePrefixHook<TObject, TReturn>(
            Expression<Func<TObject, TReturn>> original,
            Action<TObject> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Func<Func<TObject, TReturn>, TObject, TReturn>)Patch);

            TReturn Patch(Func<TObject, TReturn> orig, TObject self)
            {
                prefix(self);
                return orig(self);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TReturn>(
            Expression<Func<TObject, TArg0, TReturn>> original,
            Func<TObject, TArg0, TArg0> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TReturn>, TObject, TArg0, TReturn>)Patch);

            TReturn Patch(Func<TObject, TArg0, TReturn> orig, TObject self, TArg0 arg0)
            {
                arg0 = prefix(self, arg0);
                return orig(self, arg0);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TReturn>> original,
            Func<TObject, TArg0, TArg1, (TArg0, TArg1)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TArg1, TReturn>, TObject, TArg0, TArg1, TReturn>)Patch);

            TReturn Patch(Func<TObject, TArg0, TArg1, TReturn> orig, TObject self, TArg0 arg0,
                TArg1 arg1)
            {
                (arg0, arg1) = prefix(self, arg0, arg1);
                return orig(self, arg0, arg1);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TReturn>> original,
            Func<TObject, TArg0, TArg1, TArg2, (TArg0, TArg1, TArg2)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TArg1, TArg2, TReturn>, TObject, TArg0, TArg1, TArg2,
                    TReturn>)Patch);

            TReturn Patch(Func<TObject, TArg0, TArg1, TArg2, TReturn> orig, TObject self,
                TArg0 arg0, TArg1 arg1,
                TArg2 arg2)
            {
                (arg0, arg1, arg2) = prefix(self, arg0, arg1, arg2);
                return orig(self, arg0, arg1, arg2);
            }
        }

        public static Hook CreatePrefixHook<TObject, TArg0, TArg1, TArg2, TArg3, TReturn>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TReturn>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, (TArg0, TArg1, TArg2, TArg3)> prefix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);

            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TReturn>, TObject, TArg0, TArg1,
                    TArg2, TArg3, TReturn>)
                Patch);

            TReturn Patch(Func<TObject, TArg0, TArg1, TArg2, TArg3, TReturn> orig, TObject self,
                TArg0 arg0, TArg1 arg1,
                TArg2 arg2, TArg3 arg3)
            {
                (arg0, arg1, arg2, arg3) = prefix(self, arg0, arg1, arg2, arg3);
                return orig(self, arg0, arg1, arg2, arg3);
            }
        }

        #endregion

        #region Postfix with no return

        public static Hook CreatePostfixHook<TObject>(Expression<Action<TObject>> original,
            Action<TObject> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, (Action<Action<TObject>, TObject>)Patch);

            void Patch(Action<TObject> orig, TObject self)
            {
                orig(self);
                postfix(self);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0>(
            Expression<Action<TObject, TArg0>> original,
            Action<TObject, TArg0> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0>, TObject, TArg0>)Patch);

            void Patch(Action<TObject, TArg0> orig, TObject self, TArg0 arg0)
            {
                orig(self, arg0);
                postfix(self, arg0);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1>(
            Expression<Action<TObject, TArg0, TArg1>> original,
            Action<TObject, TArg0, TArg1> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1>, TObject, TArg0, TArg1>)Patch);

            void Patch(Action<TObject, TArg0, TArg1> orig, TObject self, TArg0 arg0, TArg1 arg1)
            {
                orig(self, arg0, arg1);
                postfix(self, arg0, arg1);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original,
            Action<TObject, TArg0, TArg1, TArg2> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1, TArg2>, TObject, TArg0, TArg1, TArg2>)Patch);

            void Patch(Action<TObject, TArg0, TArg1, TArg2> orig, TObject self, TArg0 arg0,
                TArg1 arg1, TArg2 arg2)
            {
                orig(self, arg0, arg1, arg2);
                postfix(self, arg0, arg1, arg2);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3>, TObject, TArg0, TArg1, TArg2,
                    TArg3>)Patch);

            void Patch(Action<TObject, TArg0, TArg1, TArg2, TArg3> orig, TObject self, TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2,
                TArg3 arg3)
            {
                orig(self, arg0, arg1, arg2, arg3);
                postfix(self, arg0, arg1, arg2, arg3);
            }
        }

        #endregion

        #region Postfix with return

        public static Hook CreatePostfixHook<TObject, TResult>(
            Expression<Func<TObject, TResult>> original,
            Func<TObject, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TObject, TResult>, TObject, TResult>)Patch);

            TResult Patch(Func<TObject, TResult> orig, TObject self)
            {
                TResult value = orig(self);
                return postfix(self, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TResult>(
            Expression<Func<TObject, TArg0, TResult>> original,
            Func<TObject, TArg0, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TResult>, TObject, TArg0, TResult>)Patch);

            TResult Patch(Func<TObject, TArg0, TResult> orig, TObject self, TArg0 arg0)
            {
                TResult value = orig(self, arg0);
                return postfix(self, arg0, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TResult>> original,
            Func<TObject, TArg0, TArg1, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TArg1, TResult>, TObject, TArg0, TArg1, TResult>)Patch);

            TResult Patch(Func<TObject, TArg0, TArg1, TResult> orig, TObject self, TArg0 arg0,
                TArg1 arg1)
            {
                TResult value = orig(self, arg0, arg1);
                return postfix(self, arg0, arg1, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TArg1, TArg2, TResult>, TObject, TArg0, TArg1, TArg2,
                    TResult>)Patch);

            TResult Patch(Func<TObject, TArg0, TArg1, TArg2, TResult> orig, TObject self,
                TArg0 arg0, TArg1 arg1,
                TArg2 arg2)
            {
                TResult value = orig(self, arg0, arg1, arg2);
                return postfix(self, arg0, arg1, arg2, value);
            }
        }

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult>, TObject, TArg0, TArg1,
                    TArg2, TArg3, TResult>)
                Patch);

            TResult Patch(Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult> orig, TObject self,
                TArg0 arg0, TArg1 arg1,
                TArg2 arg2, TArg3 arg3)
            {
                TResult value = orig(self, arg0, arg1, arg2, arg3);
                return postfix(self, arg0, arg1, arg2, arg3, value);
            }
        }

        #endregion

        #region Static Postfix with no return (non-static class)

        public static Hook CreateStaticPostfixHook<TObject>(
            Expression<Action<TObject>> original,
            Action postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action>)Patch);

            void Patch(Action orig)
            {
                orig();
                postfix();
            }
        }

        public static Hook CreateStaticPostfixHook<TObject, TArg0>(
            Expression<Action<TObject, TArg0>> original,
            Action<TArg0> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TArg0>, TArg0>)Patch);

            void Patch(Action<TArg0> orig, TArg0 arg0)
            {
                orig(arg0);
                postfix(arg0);
            }
        }

        public static Hook CreateStaticPostfixHook<TObject, TArg0, TArg1>(
            Expression<Action<TObject, TArg0, TArg1>> original,
            Action<TArg0, TArg1> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TArg0, TArg1>, TArg0, TArg1>)Patch);

            void Patch(Action<TArg0, TArg1> orig, TArg0 arg0, TArg1 arg1)
            {
                orig(arg0, arg1);
                postfix(arg0, arg1);
            }
        }

        public static Hook CreateStaticPostfixHook<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original,
            Action<TArg0, TArg1, TArg2> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TArg0, TArg1, TArg2>, TArg0, TArg1, TArg2>)Patch);

            void Patch(Action<TArg0, TArg1, TArg2> orig, TArg0 arg0, TArg1 arg1,
                TArg2 arg2)
            {
                orig(arg0, arg1, arg2);
                postfix(arg0, arg1, arg2);
            }
        }

        #endregion

        #region Static Postfix with return (non-static class)

        public static Hook CreateStaticPostfixHook<TObject, TResult>(
            Expression<Func<TObject, TResult>> original,
            Func<TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TResult>, TResult>)Patch);

            TResult Patch(Func<TResult> orig)
            {
                TResult value = orig();
                return postfix(value);
            }
        }

        public static Hook CreateStaticPostfixHook<TObject, TArg0, TResult>(
            Expression<Func<TObject, TArg0, TResult>> original,
            Func<TArg0, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TArg0, TResult>, TArg0, TResult>)Patch);

            TResult Patch(Func<TArg0, TResult> orig, TArg0 arg0)
            {
                TResult value = orig(arg0);
                return postfix(arg0, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TObject, TArg0, TArg1, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TResult>> original,
            Func<TArg0, TArg1, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TArg0, TArg1, TResult>, TArg0, TArg1, TResult>)Patch);

            TResult Patch(Func<TArg0, TArg1, TResult> orig, TArg0 arg0, TArg1 arg1)
            {
                TResult value = orig(arg0, arg1);
                return postfix(arg0, arg1, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TObject, TArg0, TArg1, TArg2, TResult>(
            Expression<Func<TObject, TArg0, TArg1, TArg2, TResult>> original,
            Func<TArg0, TArg1, TArg2, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TArg0, TArg1, TArg2, TResult>, TArg0, TArg1, TArg2, TResult>)Patch);

            TResult Patch(Func<TArg0, TArg1, TArg2, TResult> orig, TArg0 arg0, TArg1 arg1,
                TArg2 arg2)
            {
                TResult value = orig(arg0, arg1, arg2);
                return postfix(arg0, arg1, arg2, value);
            }
        }

        #endregion

        #region Static Postfix with return

        public static Hook CreateStaticPostfixHook<TResult>(
            Type type,
            Expression<Func<TResult>> original,
            Func<TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(type, original);
            return new Hook(actualMethodBody,
                (Func<Func<TResult>, TResult>)Patch);

            TResult Patch(Func<TResult> orig)
            {
                TResult value = orig();
                return postfix(value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TResult>(
            Type type,
            Expression<Func<TArg0, TResult>> original,
            Func<TArg0, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(type, original);
            return new Hook(actualMethodBody,
                (Func<Func<TArg0, TResult>, TArg0, TResult>)Patch);

            TResult Patch(Func<TArg0, TResult> orig, TArg0 arg0)
            {
                TResult value = orig(arg0);
                return postfix(arg0, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TResult>(
            Type type,
            Expression<Func<TArg0, TArg1, TResult>> original,
            Func<TArg0, TArg1, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(type, original);
            return new Hook(actualMethodBody,
                (Func<Func<TArg0, TArg1, TResult>, TArg0, TArg1, TResult>)Patch);

            TResult Patch(Func<TArg0, TArg1, TResult> orig, TArg0 arg0, TArg1 arg1)
            {
                TResult value = orig(arg0, arg1);
                return postfix(arg0, arg1, value);
            }
        }

        public static Hook CreateStaticPostfixHook<TArg0, TArg1, TArg2, TResult>(
            Type type,
            Expression<Func<TArg0, TArg1, TArg2, TResult>> original,
            Func<TArg0, TArg1, TArg2, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod(type, original);
            return new Hook(actualMethodBody,
                (Func<Func<TArg0, TArg1, TArg2, TResult>, TArg0, TArg1, TArg2, TResult>)Patch);

            TResult Patch(Func<TArg0, TArg1, TArg2, TResult> orig, TArg0 arg0, TArg1 arg1,
                TArg2 arg2)
            {
                TResult value = orig(arg0, arg1, arg2);
                return postfix(arg0, arg1, arg2, value);
            }
        }

        #endregion

        #region Static Postfix with return (static class)

        public static Hook CreatePostfixHook<TObject, TArg0, TArg1, TArg2, TArg3, TResult>(
            Type type,
            Expression<Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult>> original,
            Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult, TResult> postfix)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Func<Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult>, TObject, TArg0, TArg1,
                    TArg2, TArg3, TResult>)
                Patch);

            TResult Patch(Func<TObject, TArg0, TArg1, TArg2, TArg3, TResult> orig, TObject self,
                TArg0 arg0, TArg1 arg1,
                TArg2 arg2, TArg3 arg3)
            {
                TResult value = orig(self, arg0, arg1, arg2, arg3);
                return postfix(self, arg0, arg1, arg2, arg3, value);
            }
        }

        #endregion

        #region Skip with no return

        public static Hook Skip<TObject>(Expression<Action<TObject>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, (Action<Action<TObject>, TObject>)Patch);

            static void Patch(Action<TObject> orig, TObject self)
            {
            }
        }

        public static Hook Skip<TObject, TArg0>(Expression<Action<TObject, TArg0>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0>, TObject, TArg0>)Patch);

            static void Patch(Action<TObject, TArg0> orig, TObject self, TArg0 arg0)
            {
            }
        }

        public static Hook Skip<TObject, TArg0, TArg1>(
            Expression<Action<TObject, TArg0, TArg1>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1>, TObject, TArg0, TArg1>)Patch);

            static void Patch(Action<TObject, TArg0, TArg1> orig, TObject self, TArg0 arg0,
                TArg1 arg1)
            {
            }
        }

        public static Hook Skip<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1, TArg2>, TObject, TArg0, TArg1, TArg2>)Patch);

            static void Patch(Action<TObject, TArg0, TArg1, TArg2> orig, TObject self, TArg0 arg0,
                TArg1 arg1,
                TArg2 arg2)
            {
            }
        }

        public static Hook Skip<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3>> original)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody,
                (Action<Action<TObject, TArg0, TArg1, TArg2, TArg3>, TObject, TArg0, TArg1, TArg2,
                    TArg3>)Patch);

            static void Patch(Action<TObject, TArg0, TArg1, TArg2, TArg3> orig, TObject self,
                TArg0 arg0, TArg1 arg1,
                TArg2 arg2, TArg3 arg3)
            {
            }
        }

        #endregion

        #region Replace with no return

        public static Hook Replace<TObject>(Expression<Action<TObject>> original,
            Action<TObject> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0>(Expression<Action<TObject, TArg0>> original,
            Action<TObject, TArg0> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1>(
            Expression<Action<TObject, TArg0, TArg1>> original,
            Action<TObject, TArg0, TArg1> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2>(
            Expression<Action<TObject, TArg0, TArg1, TArg2>> original,
            Action<TObject, TArg0, TArg1, TArg2> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        public static Hook Replace<TObject, TArg0, TArg1, TArg2, TArg3>(
            Expression<Action<TObject, TArg0, TArg1, TArg2, TArg3>> original,
            Action<TObject, TArg0, TArg1, TArg2, TArg3> replacement)
        {
            MethodInfo actualMethodBody = GetRuntimeMethod<TObject>(original);
            return new Hook(actualMethodBody, replacement);
        }

        #endregion

        internal static MethodInfo GetRuntimeMethod<TObject>(LambdaExpression original)
        {
            string name = ((MethodCallExpression)original.Body).Method.Name;
            MethodInfo actualMethodBody = typeof(TObject).GetMethod(name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.Static);
            if (actualMethodBody == null)
            {
                throw new Exception(
                    $"Could not find method {name} in type {typeof(TObject)} during runtime");
            }

            return actualMethodBody;
        }

        private static MethodInfo GetRuntimeMethod(Type type, LambdaExpression original)
        {
            string name = ((MethodCallExpression)original.Body).Method.Name;
            MethodInfo actualMethodBody = type.GetMethod(name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.Static);
            if (actualMethodBody == null)
            {
                throw new Exception(
                    $"Could not find method {name} in type {type} during runtime");
            }

            return actualMethodBody;
        }
    }
}