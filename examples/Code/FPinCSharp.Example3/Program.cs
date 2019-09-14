using System;
using System.Collections.Generic;
using System.Linq;
using FPinCSharp.Examples.Common;
using LanguageExt;
using static LanguageExt.Prelude;

namespace FPinCSharp.Example3
{
    class Program
    {
        private static readonly EmployeeApp EmployeeApp = new EmployeeApp();

        static void Main(string[] args)
        {
            Loop()
                .IfNone(() =>
                {
                    Console.WriteLine("Shutting down ...");
                    Environment.Exit(1);
                    return unit;
                });
        }

        private static Option<Unit> Loop()
        {
            EmployeeAppHelpers.PrintWelcomeMessage();
            
            return ReadAction()
                .Bind((action) => EmployeeApp.Run(action))
                .Match(
                    Some: _ => RetryPrompt(),
                    None: () =>
                    {
                        Console.WriteLine("Error occured!");
                        return RetryPrompt();
                    }
                );
        }

        private static Option<UserAction> ReadAction()
        {
            return Parsers.ParseEnum<UserAction>(Console.ReadLine());
        }

        private static Option<Unit> RetryPrompt()
        {
            Console.WriteLine("Wanna try again? (say yes)");

            if (Console.ReadLine() == "yes")
            {
                return Loop();
            }

            Console.WriteLine("Bye!");

            return unit;
        }
    }

    public class EmployeeApp
    {
        private const double RAISE_RATE = 0.2;

        public Option<Unit> Run(UserAction action)
        {
            switch (action)
            {
                case UserAction.Show:
                {
                    // NOTE: Since all these actions have side effect of printing
                    // We can use .Iter() and avoid the return unit boilerplate
                    //ReadEmployeeId()
                    //    .Iter(id =>
                    //    {
                    //        DbHelper
                    //            .GetEmployee(id)
                    //            .Iter(employee =>
                    //            {
                    //                Console.WriteLine(employee.ToString());
                    //            });
                    //    });

                    return ReadEmployeeId()
                        .Bind<Unit>(id =>
                        {
                            return DbHelper
                                .GetEmployee(id)
                                .Map(employee =>
                                {
                                    Console.WriteLine(employee.ToString());
                                    return unit;
                                });
                        });
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
                                    // Higher order functions allow us reuse our functions (GiveRaise)
                                    // without the need of changing their types
                                    // e.g no need to change all of our code base form
                                    // Employee to Option<Employee>
                                    var happyEmployee = GiveRaise(employee, RAISE_RATE);
                                    Console.WriteLine("Employee " + employee);
                                    Console.WriteLine("Happy Employee " + happyEmployee);
                                    return unit;
                                });
                        });
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
                                    Console.WriteLine("Employee " + employee);
                                    Console.WriteLine("Happy Employee " + happyEmployee);
                                    return unit;
                                });
                        });
                }

                case UserAction.ReportAll:
                {
                    var employees = DbHelper.GetEmployees();

                    var report = employees
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
                    throw new Exception($"Something really bad has happened, Action of {action} is invalid");
            }
        }

        private static Option<int> ReadEmployeeId()
        {
            Console.WriteLine("Employee Id:");
            return Parsers.ParseInt(Console.ReadLine());
        }

        private static Option<Role> ReadRole()
        {
            Console.WriteLine("Role:");
            return Parsers.ParseEnum<Role>(Console.ReadLine());
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
    }

    // NOTE: With attribute from LanguageExt uses code generation to generate 
    // With function with all of the attributes of this class
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

        public static Option<Employee> GetEmployee(int id)
        {
            var employee = Employees.SingleOrDefault(x => x.Id == id);
            return employee != null
                ? Some(employee)
                : None;
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
}

