using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace FPinCSharp.Traversables
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            /*
                List of Options 
            */

            Action<string> optionsExample = (input) =>
            {
                var listOfOptionsOfInt = input
                    .Split(',')
                    .Map(x => x.Trim())
                    .Map(parseInt); // IEnumerable<Option<int>>

                var optionOfListOfInts = listOfOptionsOfInt
                    .Sequence(); // Option<IEnumerable<int>>

                optionOfListOfInts
                    .Match(
                        xs => Console.WriteLine($"Total is {xs.Sum()}"),
                        () => Console.WriteLine("Invalid Input")
                    );
            };

            Console.WriteLine("Options Example");
            optionsExample("1, 2, 3");
            optionsExample("1, Two, Three, 4");
            Console.WriteLine("----------------");

            /*
                List of Eithers 
            */

            Action<string> eithersExample = (input) =>
            {
                var listOfEithersOfInt = input
                    .Split(',')
                    .Map(x => x.Trim())
                    .Map(x => parseInt(x)
                        .ToEither(new Exception($"'{x}' is not a valid integer"))
                    ); // IEnumerable<Either<Exception, int>>

                var eitherOfListOfInts = listOfEithersOfInt
                    .Sequence(); // Either<Exception, IEnumerable<int>>

                eitherOfListOfInts
                    .Match(
                        xs => Console.WriteLine($"Total is {xs.Sum()}"),
                        (ex) => Console.WriteLine(ex.Message)
                    );
            };

            Console.WriteLine("Eithers example");
            eithersExample("1, 2, 3");
            eithersExample("1, Two, Three, 4");
            Console.WriteLine("----------------");


            /*
                List of Validations 
            */

            Action<string> validationsExample = (input) =>
            {
                var listOfValidationsOfInt = input
                    .Split(',')
                    .Map(x => x.Trim())
                    .Map(x => parseInt(x)
                        .ToValidation(new Exception($"'{x}' is not a valid integer"))
                    ); // IEnumerable<Either<Exception, int>>

                var eitherOfListOfInts = listOfValidationsOfInt
                    .Sequence(); // Either<Exception, IEnumerable<int>>

                eitherOfListOfInts
                    .Match(
                        xs => Console.WriteLine($"Total is {xs.Sum()}"),
                        (ex) => Console.WriteLine(string.Join("\n", ex.Map(x => x.Message)))
                    );
            };

            Console.WriteLine("Validations example");
            validationsExample("1, 2, 3");
            validationsExample("1, Two, Three, 4");
            Console.WriteLine("----------------");

            /*
                List of Tasks 
            */

            Func<string, Task> tasksExample = async (input) =>
            {
                var listOfTasksOfInt = input
                    .Split(',')
                    .Map(x => x.Trim())
                    .Map(ParseIntAsync); // IEnumerable<Task<int>>

                var taskOfListOfInts = listOfTasksOfInt
                    .Sequence(); // Task<IEnumerable<int>>

                await taskOfListOfInts
                    .ContinueWith(x =>
                    {
                        if (x.IsFaulted)
                        {
                            Console.WriteLine(x.Exception.Message);
                            Console.WriteLine(x.Exception.InnerException.Message);
                        }
                        else
                        {
                            Console.WriteLine($"Total is {x.Result.Sum()}");
                        }
                    });
            };

            Console.WriteLine("Tasks example");
            var sw1 = new Stopwatch(); sw1.Start();
            await tasksExample("1, 2, 3");
            Console.WriteLine(sw1.ElapsedMilliseconds);

            var sw2 = new Stopwatch(); sw2.Start();
            await tasksExample("1, Two, Three, 4");
            Console.WriteLine(sw2.ElapsedMilliseconds);
            Console.WriteLine("----------------");


            /*
                List of TryAsync 
            */

            Func<string, Task> tryAsyncsExample = async (input) =>
            {
                var listOfTasksOfInt = input
                    .Split(',')
                    .Map(x => x.Trim())
                    .Map((x) =>
                        TryAsync(() => ParseIntAsync(x))
                    ); // IEnumerable<TryAsync<int>>

                var taskOfListOfInts = listOfTasksOfInt
                    .Sequence(); // TryAsync<IEnumerable<int>>

                await taskOfListOfInts
                    .Match(
                        xs => Console.WriteLine($"Total is {xs.Sum()}"),
                        (ex) =>
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.InnerException.Message);
                        }
                    );
            };

            Console.WriteLine("TryAsyncs example");
            var sw3 = new Stopwatch(); sw3.Start();
            await tryAsyncsExample("1, 2, 3");
            Console.WriteLine(sw3.ElapsedMilliseconds);

            var sw4 = new Stopwatch(); sw4.Start();
            await tryAsyncsExample("1, Two, Three, 4");
            Console.WriteLine(sw4.ElapsedMilliseconds);
            Console.WriteLine("----------------");

            /*
                List of Try
            */

            Action<string> trysExample = (input) =>
            {
                var listOfTasksOfInt = input
                    .Split(',')
                    .Map(x => x.Trim())
                    .Map((x) =>
                        Try(() => int.Parse(x))
                    ); // IEnumerable<Try<int>>

                var taskOfListOfInts = listOfTasksOfInt
                    .Sequence(); // Try<IEnumerable<int>>

                taskOfListOfInts
                    .Match(
                        xs => Console.WriteLine($"Total is {xs.Sum()}"),
                        (ex) => Console.WriteLine(ex.Message)
                    );
            };

            Console.WriteLine("Trys example");
            trysExample("1, 2, 3");
            trysExample("1, Two, Three, 4");

        }

        static Task<int> ParseIntAsync(string value)
        {
            return parseInt(value)
                .Match<Task<int>>(
                    async (i) =>
                    {
                        await Task.Delay(1000);
                        return i;
                    },
                    () =>
                    {
                        return Task.FromException<int>(new Exception($"{value} is not a valid integer"));
                    }
                );
        }
    }

}
