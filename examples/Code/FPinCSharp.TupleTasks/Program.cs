using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaskTupleAwaiter;

namespace FPinCSharp.TupleTasks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
                Fetching A's in Parallel
            */
            var sw1 = Stopwatch.StartNew();
            var tasks1 = new[]
            {
                Helper.FetchA(1),
                Helper.FetchA(2),
                Helper.FetchA(3),
            };

            var result1 = await Task.WhenAll(tasks1);
            Console.WriteLine(sw1.Elapsed);

            /*
                Fetching a A and a B in Parallel
            */
            var sw2 = Stopwatch.StartNew();
            // Starting the tasks
            var taskA = Helper.FetchA(1);
            var taskB = Helper.FetchB(10);
            // Awaiting, should take as long as the longest task
            var resultA = await taskA;
            var resultB = await taskB;
            Console.WriteLine(sw2.Elapsed);


            /*
                Fetching A's and B's in Parallel
            */
            var sw3 = Stopwatch.StartNew();
            // Starting the tasks
            // NOTE: IEnumerable<Task<A>> is lazy, to start the tasks, we need to call ToArray() on them
            // otherwise the taskBs does not start until the code reaches to Task.WhenAll(taskBs)
            var taskAs = Enumerable.Range(1, 3).Select(Helper.FetchA).ToArray();
            var taskBs = Enumerable.Range(10, 13).Select(Helper.FetchB).ToArray();
            // Awaiting, should take as long as the longest task
            var resultAs = await Task.WhenAll(taskAs);
            var resultBs = await Task.WhenAll(taskBs);
            Console.WriteLine(sw3.Elapsed);

            /*
                Fetching a A and a B in Parallel using Tuples and TaskTupleAwaiter
            */

            var sw4 = Stopwatch.StartNew();
            var (resA, resB) = await (Helper.FetchA(1), Helper.FetchB(10));
            Console.WriteLine(sw4.Elapsed);

            /*
                Fetching A's and B's in Parallel using Tuples and TaskTupleAwaiter
            */
            var sw5 = Stopwatch.StartNew();
            var (resAs, resBs) = await (
                Task.WhenAll(Enumerable.Range(1, 3).Select(Helper.FetchA)),
                Task.WhenAll(Enumerable.Range(10, 13).Select(Helper.FetchB))
            );
            Console.WriteLine(sw5.Elapsed);
        }
    }

    public static class Helper
    {
        public static async Task<A> FetchA(int id)
        {
            await Task.Delay(1000);
            return new A { Value = id };
        }

        public static async Task<B> FetchB(int id)
        {
            await Task.Delay(2000);
            return new B { Value = id };
        }
    }

    public class A
    {
        public int Value { get; set; }
    }

    public class B
    {
        public int Value { get; set; }
    }
}
