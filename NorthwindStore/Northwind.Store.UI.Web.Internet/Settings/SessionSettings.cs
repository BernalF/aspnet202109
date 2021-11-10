using Microsoft.AspNetCore.Http;
using Northwind.Store.UI.Web.Internet.Extensions;
using Northwind.Store.UI.Web.Internet.ViewModels;

namespace Northwind.Store.UI.Web.Internet.Settings
{
    public class SessionSettings
    {
        private readonly ISession _session;
        public SessionSettings(IHttpContextAccessor hca)
        {
            _session = hca?.HttpContext.Session;
        }
        public string Welcome
        {
            get
            {
                return _session.GetString(nameof(Welcome));
            }
            set
            {
                _session.SetString(nameof(Welcome), value);
            }
        }

        public CartViewModel Cart
        {
            get
            {
                var cart = _session.GetObject<CartViewModel>(nameof(Cart));

                if (cart == null)
                {
                    cart = new CartViewModel();
                }

                return cart;
            }
            set
            {
                _session.SetObject(nameof(Cart), value);
            }
        }
    }
}