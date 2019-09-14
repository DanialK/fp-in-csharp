using System;
using System.Threading.Tasks;
using LanguageExt;

namespace FPinCSharp.Example7
{
    public static class TaskExtensions
    {
        public static async Task<R> Apply<T, R>
            (this Task<Func<T, R>> f, Task<T> arg)
            //=> (await f)(await arg);
            => (await f.ConfigureAwait(false))(await arg.ConfigureAwait(false));

        public static Task<Func<T2, R>> Apply<T1, T2, R>
            (this Task<Func<T1, T2, R>> f, Task<T1> arg)
            => Apply(f.Map(F.Curry), arg);

        public static Task<Func<T2, T3, R>> Apply<T1, T2, T3, R>
            (this Task<Func<T1, T2, T3, R>> @this, Task<T1> arg)
            => Apply(@this.Map(F.CurryFirst), arg);

        public static Task<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
            (this Task<Func<T1, T2, T3, T4, R>> @this, Task<T1> arg)
            => Apply(@this.Map(F.CurryFirst), arg);

        public static Task<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
            (this Task<Func<T1, T2, T3, T4, T5, R>> @this, Task<T1> arg)
            => Apply(@this.Map(F.CurryFirst), arg);

        public static Task<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
            (this Task<Func<T1, T2, T3, T4, T5, T6, R>> @this, Task<T1> arg)
            => Apply(@this.Map(F.CurryFirst), arg);

        public static Task<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
            (this Task<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Task<T1> arg)
            => Apply(@this.Map(F.CurryFirst), arg);

        public static Task<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
            (this Task<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Task<T1> arg)
            => Apply(@this.Map(F.CurryFirst), arg);

        public static Task<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
            (this Task<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Task<T1> arg)
            => Apply(@this.Map(F.CurryFirst), arg);
    }

    public static class F
    {
        public static Unit Unit() => default(Unit);

        // function manipulation 

        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func)
            => t1 => t2 => func(t1, t2);

        public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> func)
            => t1 => t2 => t3 => func(t1, t2, t3);

        public static Func<T1, Func<T2, T3, R>> CurryFirst<T1, T2, T3, R>
            (this Func<T1, T2, T3, R> @this) => t1 => (t2, t3) => @this(t1, t2, t3);

        public static Func<T1, Func<T2, T3, T4, R>> CurryFirst<T1, T2, T3, T4, R>
            (this Func<T1, T2, T3, T4, R> @this) => t1 => (t2, t3, t4) => @this(t1, t2, t3, t4);

        public static Func<T1, Func<T2, T3, T4, T5, R>> CurryFirst<T1, T2, T3, T4, T5, R>
            (this Func<T1, T2, T3, T4, T5, R> @this) => t1 => (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);

        public static Func<T1, Func<T2, T3, T4, T5, T6, R>> CurryFirst<T1, T2, T3, T4, T5, T6, R>
            (this Func<T1, T2, T3, T4, T5, T6, R> @this) => t1 => (t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6);

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, R>
            (this Func<T1, T2, T3, T4, T5, T6, T7, R> @this) => t1 => (t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7);

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, R>
            (this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> @this) => t1 => (t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8);

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
            (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> @this) => t1 => (t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
    }
}