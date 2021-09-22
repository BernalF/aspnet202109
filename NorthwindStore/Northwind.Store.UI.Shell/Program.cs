using System;
using ConsoleTables;
using Northwind.Store.Data;

namespace Northwind.Store.UI.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            string readLine;
            do
            {
                Console.WriteLine("Seleccione una opción:\n1. Customers List");

                readLine = Console.ReadLine();

                if (!string.IsNullOrEmpty(readLine))
                {
                    int option = Convert.ToInt32(readLine);
                    Console.Clear();

                    switch (option)
                    {
                        case 1:
                            Console.WriteLine("GetCustomers");
                            GetCustomers();
                            break;
                        
                    }
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (!string.IsNullOrEmpty(readLine));
        }

        private static void GetCustomers()
        {
            var customers = new CustomerRepository().Get();

            var table = new ConsoleTable("CompanyName", "ContactName", "Country");

            foreach (var c in customers)
            {
                table.AddRow(c.CompanyName, c.ContactName, c.Country);
            }

            table.Write();
            Console.WriteLine();
        }
    }
}