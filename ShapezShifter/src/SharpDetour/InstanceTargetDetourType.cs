using System;
using System.Linq.Expressions;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    public readonly struct HookPromiseInstance<TObject>
    {
        public HookVoidMethodSignaturePromise<TObject, TArg0> WithArg<TArg0>()
        {
            return new HookVoidMethodSignaturePromise<TObject, TArg0>();
        }

        public HookVoidMethodSignaturePromise<TObject, TArg0, TArg1> WithArgs<TArg0, TArg1>()
        {
            return new HookVoidMethodSignaturePromise<TObject, TArg0, TArg1>();
        }

        public HookVoidMethodSignaturePromise<TObject, TArg0, TArg1, TArg2> WithArgs<TArg0, TArg1,
            TArg2>()
        {
            return new HookVoidMethodSignaturePromise<TObject, TArg0, TArg1, TArg2>();
        }

        public HookMethodSignaturePromise<TObject, TResult> Returning<TResult>()
        {
            return new HookMethodSignaturePromise<TObject, TResult>();
        }
    }

    public readonly struct HookVoidMethodSignaturePromise<TObject, TArg0>
    {
        public HookVoidMethodSignaturePromise<TObject, TArg0, TArg1> WithArg<TArg1>()
        {
            return new HookVoidMethodSignaturePromise<TObject, TArg0, TArg1>();
        }

        public HookMethodSignaturePromise<TObject, TArg0, TResult> Returning<TResult>()
        {
            return new HookMethodSignaturePromise<TObject, TArg0, TResult>();
        }
    }

    public readonly struct HookVoidMethodSignaturePromise<TObject, TArg0, TArg1>
    {
        public HookVoidMethodSignaturePromise<TObject, TArg0, TArg1, TArg2> WithArg<TArg2>()
        {
            return new HookVoidMethodSignaturePromise<TObject, TArg0, TArg1, TArg2>();
        }

        public HookMethodSignaturePromise<TObject, TArg0, TArg1, TResult> Returning<TResult>()
        {
            return new HookMethodSignaturePromise<TObject, TArg0, TArg1, TResult>();
        }
    }

    public readonly struct HookVoidMethodSignaturePromise<TObject, TArg0, TArg1, TArg2>
    {
        public HookVoidMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TArg3> WithArg<TArg3>()
        {
            return new HookVoidMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TArg3>();
        }

        public HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TResult>
            Returning<TResult>()
        {
            return new HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TResult>();
        }
    }

    public readonly struct HookVoidMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TArg3>
    {
        public HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TArg3, TResult>
            Returning<TResult>()
        {
            return new HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TArg3, TResult>();
        }
    }

    public readonly struct HookMethodSignaturePromise<TObject, TResult>
    {
        public HookMethodSignaturePromise<TObject, TArg0, TResult> WithArg<TArg0>()
        {
            return new HookMethodSignaturePromise<TObject, TArg0, TResult>();
        }
    }

    public readonly struct HookMethodSignaturePromise<TObject, TArg0, TResult>
    {
        public HookMethodSignaturePromise<TObject, TArg0, TArg1, TResult> WithArg<TArg1>()
        {
            return new HookMethodSignaturePromise<TObject, TArg0, TArg1, TResult>();
        }
    }

    public readonly struct HookMethodSignaturePromise<TObject, TArg0, TArg1, TResult>
    {
        public HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TResult> WithArg<TArg2>()
        {
            return new HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TResult>();
        }

        public HookMethodSignatureTarget<TObject, TArg0, TArg1, TResult> Detour(
            Expression<Func<TObject, TArg0, TArg1, TResult>> original)
        {
            return new HookMethodSignatureTarget<TObject, TArg0, TArg1, TResult>(original);
        }
    }

    public readonly struct HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TResult>
    {
        public HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TArg3, TResult>
            WithArg<TArg3>()
        {
            return new HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TArg3, TResult>();
        }
    }

    public readonly struct HookMethodSignaturePromise<TObject, TArg0, TArg1, TArg2, TArg3, TResult>
    {
    }

    public readonly struct HookMethodSignatureTarget<TObject, TArg0, TArg1, TResult>
    {
        private readonly Expression<Func<TObject, TArg0, TArg1, TResult>> Original;
        private readonly MethodInfo TargetMethodBody;

        public HookMethodSignatureTarget(Expression<Func<TObject, TArg0, TArg1, TResult>> original)
        {
            Original = original;
            TargetMethodBody = DetourHelper.GetRuntimeMethod<TObject>(original);
        }

        public Hook Postfix(Func<TObject, TArg0, TArg1, TResult, TResult> postfix)
        {
            return DetourHelper.CreatePostfixHook(Original, postfix);
        }
    }
}