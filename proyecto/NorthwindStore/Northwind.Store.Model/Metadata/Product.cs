using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Store.Model
{
    [ModelMetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        public class ProductMetadata
        {
            [StringLength(40, MinimumLength = 3, ErrorMessage = "Se requiere de {2} a {1} caracteres.")]
            [Required(ErrorMessage = "El {0} es requerido.")]
            [Display(Name = "Nombre")]
            public string ProductName { get; set; }
        }
    }
}
