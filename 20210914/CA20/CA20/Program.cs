using System;
using System.Linq;

namespace CA20
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new NWContext())
            {
                foreach (var p in db.Products)
                {
                    Console.WriteLine(p.ProductName);
                }

                // Change Tracking
                var p1 = db.Products.Single(p => p.ProductId == 1);
                p1.ProductName += " M";

                var newProduct = new Product() { ProductName = "Demostración" };
                db.Products.Add(newProduct);

                //var deleteProduct = db.Products.Single(p => p.ProductId == 78);
                //db.Remove(deleteProduct);
                //db.Products.Remove(deleteProduct);

                var et = db.ChangeTracker.Entries<Product>();

                db.SaveChanges();
            }



            Console.ReadLine();
        }
    }
}
