using Northwind.Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Store.UI.Web.PWA.Client.Services
{
    public interface IProductService
    {
        Task<List<Product>> Search(string filter);
        Task<Product> Create(Product p);
        Task Update(Product p);
        Task Delete(int id);
    }
}
