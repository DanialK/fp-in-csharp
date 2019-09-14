using System;
using System.Collections.Generic;
using System.Linq;
using FPinCSharp.Examples.Common;

namespace FPinCSharp.Example1
{
    class Program
    {
        private static readonly EmployeeApp EmployeeApp = new EmployeeApp();

        static void Main(string[] args)
        {
            Loop();
        }

        private static void Loop()
        {
            EmployeeAppHelpers.PrintWelcomeMessage();

            var action = Console.ReadLine();

            EmployeeApp.Run(action);

            RetryPrompt();
        }

        private static void RetryPrompt()
        {
            Console.WriteLine("Wanna try again?");

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

        public void Run(string action)
        {
            if (action == "Show")
            {
                var id = ReadEmployeeId();
                var employee = DbHelper.GetEmployee(id);
                Console.WriteLine(employee.ToString());
            }
            else if (action == "Raise")
            {
                var id = ReadEmployeeId();
                var employee = DbHelper.GetEmployee(id);
                var happyEmployee = GiveRaise(employee);
                Console.WriteLine("Employee " + employee);
                Console.WriteLine("Happy Employee " + happyEmployee);
            }
            else if (action == "Promote")
            {
                var id = ReadEmployeeId();
                var employee = DbHelper.GetEmployee(id);
                var happyEmployee = GivePromotion(employee);
                Console.WriteLine("Employee " + employee);
                Console.WriteLine("Happy Employee " + happyEmployee);
            }
            else if (action == "ReportAll")
            {
                var employees = DbHelper.GetEmployees();
                var report = new Dictionary<Role, double>();

                foreach (var employee in employees)
                {
                    if (!report.ContainsKey(employee.Role))
                    {
                        report[employee.Role] = 0d;
                    }

                    report[employee.Role] = report[employee.Role] + (employee.Salary / 52);
                }

                EmployeeAppHelpers.PrintReport(report);
            }
            else if (action == "ReportByRole")
            {
                var role = ReadRole();
                var employees = DbHelper.GetEmployees();
                var report = new Dictionary<Role, double>();

                foreach (var employee in employees)
                {
                    if (employee.Role != role)
                    {
                        continue;
                    }

                    if (!report.ContainsKey(employee.Role))
                    {
                        report[employee.Role] = 0d;
                    }

                    report[employee.Role] = report[employee.Role] + (employee.Salary / 52);
                }

                EmployeeAppHelpers.PrintReport(report);
            }
            else
            {
                throw new Exception($"Action of {action} is invalid");
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

        private static Employee GiveRaise(Employee employee)
        {
            employee.Salary *= (1 + RAISE_RATE);
            return employee;
        }

        private static Employee GivePromotion(Employee employee)
        {
            employee.Salary *= (1 + RAISE_RATE);
            employee.Role = EmployeeAppHelpers.NextRole(employee.Role);
            return employee;
        }
    }

    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Salary { get; set; }

        public Role Role { get; set; }

        public override string ToString()
        {
            return $"Employee(Id={Id}, FirstName={FirstName}, LastName={LastName}, Salary={Salary}, Role={Role})";
        }
    }

    public static class DbHelper
    {
        public static List<Employee> Employees = new List<Employee>()
        {
            new Employee { Id = 1, FirstName = "Marry", LastName = "West", Role = Role.AssociateEngineer, Salary = 100 },
            new Employee { Id = 2, FirstName = "John", LastName = "Doe", Role = Role.Engineer, Salary = 150 },
            new Employee { Id = 3, FirstName = "David", LastName = "Brown", Role = Role.SeniorEngineer, Salary = 200 },
            new Employee { Id = 4, FirstName = "Yin", LastName = "Yang", Role = Role.LeadEngineer, Salary = 250 },
        };

        public static List<Employee> GetEmployees()
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