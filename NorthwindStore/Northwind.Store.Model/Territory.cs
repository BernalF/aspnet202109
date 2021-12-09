using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Northwind.Store.Model
{
    public partial class Territory : IObjectWithState
    {
        public Territory()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritory>();
        }

        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<EmployeeTerritory> EmployeeTerritories { get; set; }

        [NotMapped]
        public ModelState State { get; set; }
        [NotMapped]
        public ObservableCollection<string> ModifiedProperties { get; set; }
    }
}
