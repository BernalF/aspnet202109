using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ToDoQuiz.Models
{
    [Table("ToDo")]
    public partial class ToDo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Due { get; set; }
    }
}
