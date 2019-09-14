using System;

namespace FPinCSharp.LINQSyntax
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initial");
            var box = new Box<string>("D");
            Console.WriteLine(box);
            Console.WriteLine("---------------------------");


            // Example 1
            Console.WriteLine("Example 1");
            Console.WriteLine(
                from x in box
                select x + "anial"
            );
            Console.WriteLine("---------------------------");


            // Example 2
            Console.WriteLine("Example 2");
            var rest = new Box<string>("anial");
            Console.WriteLine(
                from x in box
                from xs in rest
                select x + xs
            );
            Console.WriteLine("---------------------------");


            // Example 3
            Console.WriteLine("Example 3");
            var box1 = new Box<string>("D");
            var box2 = new Box<string>("a");
            var box3 = new Box<string>("n");
            var box4 = new Box<string>("i");
            var box5 = new Box<string>("a");
            var box6 = new Box<string>("l");

            var boxedResult = (
                from x1 in box1
                from x2 in box2
                from x3 in box3
                from x4 in box4
                from x5 in box5
                from x6 in box6
                select x1 + x2 + x3 + x4 + x5 + x6
            );

            Console.WriteLine(boxedResult);

            boxedResult.Iter(result =>
            {
                Console.WriteLine($"Result = {result}");
            });

            var resultUpper = boxedResult
                .Match(result => result.ToUpper());
            Console.WriteLine($"RESULT = {resultUpper}");

            // Mapping (staying in the elevated world of Box) as opposed to
            // Matching and taking the value out of the elevated world
            boxedResult
                .Map(x => x.ToUpper())
                .Iter(x =>
                {
                    Console.WriteLine($"RESULT = {x}");
                });
        }
    }

    public struct Box<T>
    {
        private T Value { get; }

        public Box(T value)
        {
            Value = value;
        }

        public R Match<R>(Func<T, R> func)
            => func(Value);

        public void Iter(Action<T> func)
            => func(Value);

        public override string ToString() => $"Box({Value})";
    }

    public static class BoxExt
    {
        public static Box<R> Map<T, R>(this Box<T> optT, Func<T, R> f)
        {
            return optT.Match(x => new Box<R>(f(x)));
        }

        public static Box<R> Select<T, R>(this Box<T> @this, Func<T, R> func)
        {
            return @this.Map(func);
        }

        public static Box<RR> SelectMany<T, R, RR>(
            this Box<T> opt,
            Func<T, Box<R>> bind,
            Func<T, R, RR> project)
        {
            return opt.Match(
                (t) => bind(t).Match(
                    (r) => new Box<RR>(project(t, r))
                )
            );
        }

    }
}
