using System;
using System.Collections.Generic;
using System.Linq;
using FPinCSharp.Examples.Common;

namespace FPinCSharp.Example2
{
    class Program
    {
        private static readonly EmployeeApp EmployeeApp = new EmployeeApp();

        static void Main(string[] args)
        {
            try
            {
                Loop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                RetryPrompt();
            }
        }

        private static void Loop()
        {
            EmployeeAppHelpers.PrintWelcomeMessage();

            var action = Enum.Parse<UserAction>(Console.ReadLine());

            EmployeeApp.Run(action);

            RetryPrompt();
        }

        private static void RetryPrompt()
        {
            Console.WriteLine("Wanna try again? (say yes)");

            if (Console.ReadLine() == "yes")
            {
                Loop();
            }
            else
            {
                Console.WriteLine("Bye!");
                Environment.Exit(1);
            }
        }
    }

    public class EmployeeApp
    {
        private const double RAISE_RATE = 0.2;

        public void Run(UserAction action)
        {
            switch (action)
            {
                case UserAction.Show:
                {
                    var id = ReadEmployeeId();
                    var employee = DbHelper.GetEmployee(id);
                    Console.WriteLine(employee.ToString());
                    break;
                }

                case UserAction.Raise:
                {
                    var id = ReadEmployeeId();
                    var employee = DbHelper.GetEmployee(id);
                    var happyEmployee = GiveRaise(employee, RAISE_RATE);
                    Console.WriteLine("Employee " + employee);
                    Console.WriteLine("Happy Employee " + happyEmployee);
                    break;
                }

                case UserAction.Promote:
                {
                    var id = ReadEmployeeId();
                    var employee = DbHelper.GetEmployee(id);
                    var happyEmployee = GivePromotion(employee, RAISE_RATE);
                    Console.WriteLine("Employee " + employee);
                    Console.WriteLine("Happy Employee " + happyEmployee);
                    break;
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

                        EmployeeAppHelpers.PrintReport(report);
                    break;
                }

                case UserAction.ReportByRole:
                {
                    var employees = DbHelper.GetEmployees();
                    var role = ReadRole();
                    // NOTE: there many ways to do this
                    var report = employees
                        .GroupBy(x => x.Role)
                        .Where(x => x.Key == role)
                        .ToDictionary(
                            xs => xs.Key,
                            xs => xs.Select(x => x.Salary / 52).Sum()
                        );

                    EmployeeAppHelpers.PrintReport(report);
                    break;
                }

                default:
                    throw new Exception($"Something really bad has happened, Action of {action} is invalid");
            }
        }

        private static int ReadEmployeeId()
        {
            Console.WriteLine("Employee Id:");
            var id = int.Parse(Console.ReadLine());
            return id;
        }

        private static Role ReadRole()
        {
            Console.WriteLine("Role:");
            var role = Enum.Parse<Role>(Console.ReadLine());
            return role;
        }

        private static Employee GiveRaise(Employee employee, double rate)
        {
            return employee.With(salary: employee.Salary * (1 + rate));
        }

        private static Employee GivePromotion(Employee employee, double rate)
        {
            return employee.With(
                salary: employee.Salary * (1 + rate),
                role: EmployeeAppHelpers.NextRole(employee.Role)
            );
        }
    }

    public class Employee
    {
        public int Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public double Salary { get; }

        public Role Role { get; }

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

        public Employee With(
            int? id = null,
            string firstName = null,
            string lastName = null,
            double? salary = null,
            Role? role = null)
        {
            return new Employee(
                id ?? Id,
                firstName ?? FirstName,
                lastName ?? LastName,
                salary ?? Salary,
                role ?? Role
            );
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

        public static Employee GetEmployee(int id)
        {
            return Employees.SingleOrDefault(x => x.Id == id);
        }
    }
}


























//var employee = DbHelper.GetEmployee(id);
//var happyEmployee = GivePromotion(employee);
//var happyEmployee1 = GivePromotion(employee);
//var happyEmployee2 = GivePromotion(employee);
//var happyEmployee3 = GivePromotion(employee);
//Console.WriteLine("Before " + employee);
//Console.WriteLine("happyEmployee1 " + happyEmployee1);
//Console.WriteLine("happyEmployee2 " + happyEmployee2);
//Console.WriteLine("happyEmployee3 " + happyEmployee3);
//Console.WriteLine("After " + happyEmployee);