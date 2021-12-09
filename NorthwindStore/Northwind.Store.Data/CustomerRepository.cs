using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Store.Model;

namespace Northwind.Store.Data
{
    public class CustomerRepository
    {
        public List<Customer> Get()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();

            DbContextOptionsBuilder<NwContext> optionsBuilder = new DbContextOptionsBuilder<NwContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("NW"));

            using var db = new NwContext(optionsBuilder.Options);

            return (from c in db.Customers.OrderBy(c => c.CompanyName) select c).ToList();
        }
    }
}
