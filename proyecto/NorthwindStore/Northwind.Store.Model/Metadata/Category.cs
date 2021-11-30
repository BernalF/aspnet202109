using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Store.Model
{

    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category : ModelBase
    {
        [NotMapped]
        public string PictureBase64 { 
            get 
            {
                var result = "";
                if (Picture != null)
                {
                    var base64 = Convert.ToBase64String(Picture);
                    result = $"data:image/jpg;base64,{base64}";
                }
                return result;
            } 
        }
        public class CategoryMetadata
        {
            [StringLength(15, MinimumLength = 3, ErrorMessage = "Se requiere de {2} a {1} caracteres.")]
            [Required(ErrorMessage = "El {0} es requerido.")]
            [Display(Name = "Nombre")]
            public string CategoryName { get; set; }

            [StringLength(64, MinimumLength = 3, ErrorMessage = "La {0} requiere de {2} a {1} caracteres.")]
            [Required(ErrorMessage = "La {0} es requerida.")]
            [Display(Name = "Descripción")]
            public string Description { get; set; }
        }
    }
}
