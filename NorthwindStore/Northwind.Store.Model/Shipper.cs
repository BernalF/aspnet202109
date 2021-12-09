using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Northwind.Store.Model
{
    public partial class Shipper : IObjectWithState
    {
        public Shipper()
        {
            Orders = new HashSet<Order>();
        }

        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        [NotMapped]
        public ModelState State { get; set; }
        [NotMapped]
        public ObservableCollection<string> ModifiedProperties { get; set; }
    }
}
