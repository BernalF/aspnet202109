using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Northwind.Store.Model
{
    public partial class Region : IObjectWithState
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        public int RegionId { get; set; }
        public string RegionDescription { get; set; }

        public virtual ICollection<Territory> Territories { get; set; }

        [NotMapped]
        public ModelState State { get; set; }
        [NotMapped]
        public ObservableCollection<string> ModifiedProperties { get; set; }
    }
}
