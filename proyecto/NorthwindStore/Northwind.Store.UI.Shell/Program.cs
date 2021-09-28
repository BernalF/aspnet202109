using System;

namespace Northwind.Store.UI.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Northwind.Store.Data.NWContext())
            {
                foreach (var item in db.Products)
                {
                    Console.WriteLine(item.ProductName);
                }
            }
        }
    }
}
