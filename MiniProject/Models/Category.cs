using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniProject.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Category name should not be blank")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

    }
}