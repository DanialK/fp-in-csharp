using System;
using System.Collections.Generic;
using System.Linq;
using FPinCSharp.Examples.Common;
using LanguageExt;
using static LanguageExt.Prelude;

namespace FPinCSharp.Example4
{
    class Program
    {
        private static readonly EmployeeApp EmployeeApp = new EmployeeApp();

        static void Main(string[] args)
        {
            Loop()
                .IfLeft(() =>
                {
                    Console.WriteLine("Shutting down ...");
                    Environment.Exit(1);
                    return unit;
                });
        }

        private static Either<Error, Unit> Loop()
        {
            EmployeeAppHelpers.PrintWelcomeMessage();

            return ReadAction()
                .Bind(action =>
                    EmployeeApp.Run(action)
                )
                .Match(
                    Right: _ => RetryPrompt(),
                    Left: error =>
                    {
                        Console.WriteLine("Error: " + error.Message);
                        return RetryPrompt();
                    }
                );

            //return (
            //    from action in ReadAction()
            //    from result in EmployeeApp.Run(action)
            //    select result
            //).Match(
            //    Right: _ => RetryPrompt(),
            //    Left: error =>
            //    {
            //        Console.WriteLine("Error: " + error.Message);
            //        return RetryPrompt();
            //    }
            //);
        }

        private static Either<Error, Unit> RetryPrompt()
        {
            Console.WriteLine("Wanna try again? (say yes)");

            if (Console.ReadLine() == "yes")
            {
                return Loop();
            }

            Console.WriteLine("Bye!");
            return unit;
        }

        private static Either<Error, UserAction> ReadAction()
        {
            return Parsers.ParseEnum<UserAction>(Console.ReadLine())
                .ToEither<Error>(new InvalidAction());
        }
    }

    public class EmployeeApp
    {
        private const double RAISE_RATE = 0.2;

        public Either<Error, Unit> Run(UserAction action)
        {
            switch (action)
            {
                case UserAction.Show:
                {
                    return ReadEmployeeId()
                        .Bind(DbHelper.GetEmployee)
                        .Iter(employee =>
                        {
                            Console.WriteLine(employee.ToString());
                        });

                    //return (
                    //    from id in ReadEmployeeId()
                    //    from employee in DbHelper.GetEmployee(id)
                    //    select Log(employee)
                    //);
                }

                case UserAction.Raise:
                {
                    return ReadEmployeeId()
                        .Bind<Unit>(id =>
                        {
                            return DbHelper
                                .GetEmployee(id)
                                .Map(employee =>
                                {
                                    var happyEmployee = GiveRaise(employee, RAISE_RATE);
                                    return (employee, happyEmployee);
                                })
                                .Iter(x =>
                                {
                                    Console.WriteLine("Employee " + x.employee);
                                    Console.WriteLine("Happy Employee " + x.happyEmployee);
                                });
                        });

                        //return (
                        //    from id in ReadEmployeeId()
                        //    from employee in DbHelper.GetEmployee(id)
                        //    let happyEmployee = GiveRaise(employee, RAISE_RATE)
                        //    let _ = (
                        //        Log(employee),
                        //        Log(happyEmployee)
                        //    )
                        //    select unit
                        //);
                    }

                case UserAction.Promote:
                {
                    return ReadEmployeeId()
                        .Bind<Unit>(id =>
                        {
                            return DbHelper
                                .GetEmployee(id)
                                .Map(employee =>
                                {
                                    var happyEmployee = GivePromotion(employee, RAISE_RATE);
                                    return (employee, happyEmployee);
                                })
                                .Iter(x =>
                                {
                                    Console.WriteLine("Employee " + x.employee);
                                    Console.WriteLine("Happy Employee " + x.happyEmployee);
                                });
                        });

                    //return (
                    //    from id in ReadEmployeeId()
                    //    from employee in DbHelper.GetEmployee(id)
                    //    let happyEmployee = GivePromotion(employee, RAISE_RATE)
                    //    let _ = (
                    //        Log(employee),
                    //        Log(happyEmployee)
                    //    )
                    //    select unit
                    //);
                }

                case UserAction.ReportAll:
                {
                     var report = DbHelper
                        .GetEmployees()
                        .GroupBy(x => x.Role)
                        .ToDictionary(
                            xs => xs.Key,
                            xs => xs.Select(x => x.Salary / 52).Sum()
                        );

                     return PrintReport(report);
                }

                case UserAction.ReportByRole:
                {
                    var employees = DbHelper.GetEmployees();

                    return ReadRole()
                        .Bind<Unit>(role =>
                        {
                            var report = employees
                                .GroupBy(x => x.Role)
                                .Where(x => x.Key == role)
                                .ToDictionary(
                                    xs => xs.Key,
                                    xs => xs.Select(x => x.Salary / 52).Sum()
                                );

                            return PrintReport(report);
                        });
                }
                default:
                    // NOTE: still throwing here since this is a error that should never make its way into prod
                    // and should be picked up by the engineer during the development
                    // e.g introducing a new action in the Enum and not handling it here
                    throw new Exception($"Something really bad has happened, Action of {action} is invalid");
            }
        }

        private static Either<Error, int> ReadEmployeeId()
        {
            Console.WriteLine("Employee Id:");
            return Parsers.ParseInt(Console.ReadLine())
                .ToEither<Error>(new InvalidEmployeeId());
        }

        private static Either<Error, Role> ReadRole()
        {
            Console.WriteLine("Role:");
            return Parsers.ParseEnum<Role>(Console.ReadLine())
                .ToEither<Error>(new InvalidRole());
        }

        private static Employee GiveRaise(Employee employee, double rate)
        {
            return employee.With(Salary: employee.Salary * (1 + rate));
        }

        private static Employee GivePromotion(Employee employee, double rate)
        {
            return employee.With(
                Salary: employee.Salary * (1 + rate),
                Role: EmployeeAppHelpers.NextRole(employee.Role)
            );
        }

        private static Unit PrintReport(IDictionary<Role, double> report)
        {
            EmployeeAppHelpers.PrintReport(report);
            return unit;
        }

        private static Unit Log<T>(T x)
        {
            Console.WriteLine(x);
            return unit;
        }
    }

    [With]
    public partial class Employee
    {
        public readonly int Id;

        public readonly string FirstName;

        public readonly string LastName;

        public readonly double Salary;

        public readonly Role Role;

        public override string ToString()
        {
            return $"Employee(Id={Id}, FirstName={FirstName}, LastName={LastName}, Salary={Salary}, Role={Role})";
        }

        public Employee(int id, string firstName, string lastName, double salary, Role role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Salary = salary;
            Role = role;
        }
    }

    public enum UserAction
    {
        Show,
        Raise,
        Promote,
        ReportAll,
        ReportByRole
    }

    public static class DbHelper
    {
        private static readonly List<Employee> Employees = new List<Employee>()
        {
            new Employee(id: 1, firstName: "Marry", lastName: "West", salary: 100, role: Role.AssociateEngineer),
            new Employee(id: 2, firstName: "John", lastName: "Doe", salary: 150, role: Role.Engineer),
            new Employee(id: 3, firstName: "Sara", lastName: "Brown", salary: 200, role: Role.SeniorEngineer),
            new Employee(id: 4, firstName: "Yin", lastName: "Yang", salary: 250, role: Role.LeadEngineer),
        };

        public static IReadOnlyList<Employee> GetEmployees()
        {
            return Employees;
        }

        public static Either<Error, Employee> GetEmployee(int id)
        {
            var employee = Employees.SingleOrDefault(x => x.Id == id);

            if (employee != null)
            {
                return Right(employee);
            }

            return Left<Error>(new EmployeeNotFound(id));;
        }
    }

    // Utility parsers that are total functions
    public static class Parsers
    {
        public static Option<int> ParseInt(string s)
        {
            return int.TryParse(s, out var result)
                ? Some(result)
                : None;
        }

        public static Option<T> ParseEnum<T>(string s) where T : struct
        {
            var isInt = int.TryParse(s, out var value);
            var result = (T?)Enum.GetValues(typeof(T)).OfType<object>()
                .FirstOrDefault(v => v.ToString() == s
                                     || (isInt & (int)v == value));
            return result != null
                ? Some(result)
                : None;
        }
    }

    // Errors

    public class Error
    {
        public virtual string Message { get; }
    }

    public sealed class InvalidEmployeeId : Error
    {
        public override string Message { get; } =
            "Invalid employee ID";
    }

    public sealed class InvalidAction : Error
    {
        public override string Message { get; } =
            "Invalid action";
    }

    public sealed class InvalidRole : Error
    {
        public override string Message { get; } =
            "Invalid role";
    }

    public sealed class EmployeeNotFound : Error
    {
        private readonly int _employeeId;

        public EmployeeNotFound(int employeeId)
        {
            _employeeId = employeeId;
        }

        public override string Message 
            => $"No Employee with id={_employeeId} was found";
    }
}
