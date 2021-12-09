using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Northwind.Store.Model
{
    public partial class Category : IObjectWithState
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }

        [StringLength(32, MinimumLength = 3, ErrorMessage = "Se requiere de {2} a {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        [Display(Name = "Nombre")]
        public string CategoryName { get; set; }

        [StringLength(64, MinimumLength = 3, ErrorMessage = "La {0} requiere de {2} a {1} caracteres.")]
        [Required(ErrorMessage = "La {0} es requerida.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        [NotMapped]
        public ModelState State { get; set; }
        [NotMapped]
        public ObservableCollection<string> ModifiedProperties { get; set; }
    }
}
