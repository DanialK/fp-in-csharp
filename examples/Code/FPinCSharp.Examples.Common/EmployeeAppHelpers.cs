using System;
using System.Collections.Generic;
using System.Linq;

namespace FPinCSharp.Examples.Common
{
    public static class EmployeeAppHelpers
    {
        public static void PrintWelcomeMessage()
        {
            Console.WriteLine("");
            Console.WriteLine("============================================");
            Console.WriteLine("=====  Welcome to Employee Directory   =====");
            Console.WriteLine("============================================");

            Console.WriteLine("Select an action (Show, Raise, Promote, ReportAll, ReportByRole):");
        }

        public static Role NextRole(Role role)
        {
            switch (role)
            {
                case Role.AssociateEngineer:
                    return Role.Engineer;
                case Role.Engineer:
                    return Role.SeniorEngineer;
                case Role.SeniorEngineer:
                    return Role.LeadEngineer;
                case Role.LeadEngineer:
                    return Role.LeadEngineer;
                default:
                    throw new Exception($"Role {role} is not supported");
            }
        }

        public static void PrintReport(IDictionary<Role, double> report)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("==== Weekly salary cost by role report ====");
            Console.WriteLine("===========================================");

            var indexedValues = report
                .Zip(
                    Enumerable.Range(0, report.Count),
                    (x, index) => (key: x.Key, value: x.Value, index)
                );

            foreach (var (key, value, index) in indexedValues)
            {
                Console.WriteLine($"{key}: ${Math.Round(value, 2)}K");

                if (index != report.Count - 1)
                {
                    Console.WriteLine("-------------------------------------------");
                }
            }

            Console.WriteLine("===========================================");
        }
    }
}
