using CA21.Data;
using System;
using System.Threading.Tasks;

namespace CA21
{
    class Program
    {
        static async Task Main(string[] args)
        {

            using (var db = new NWContext())
            {
                //foreach (var p in db.Products)
                //{
                //    Console.WriteLine(p.ProductName);
                //}

                var sales = await db.GetProcedures().SalesbyYearAsync(new DateTime(1990, 1, 1), DateTime.Today);
                foreach (var s in sales)
                {
                    Console.WriteLine(s.OrderID);
                }
            }

            Console.ReadLine();
        }
    }
}
