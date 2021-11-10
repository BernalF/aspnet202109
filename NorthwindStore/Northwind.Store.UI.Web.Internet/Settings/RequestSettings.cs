using Microsoft.AspNetCore.Mvc;
using Northwind.Store.Model;
using Northwind.Store.UI.Web.Internet.Extensions;

namespace Northwind.Store.UI.Web.Internet
{
    public class RequestSettings
    {
        readonly Controller _c;
        public RequestSettings(Controller c)
        {
            _c = c;
        }

        public Product ProductAdded
        {
            get
            {
                return _c.TempData.GetFromJson<Product>(nameof(ProductAdded)); ;
            }
            set
            {
                _c.TempData.SetAsJson(nameof(ProductAdded), value);
            }
        }
    }
}