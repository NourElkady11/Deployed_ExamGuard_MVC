﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Range(0,500,ErrorMessage ="you are out of the range")]
        public int? Code { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string? Name { get; set; }
        [Display(Name ="Created at")]
        public DateTime DateTime { get; set; }

        public ICollection<Employee> employees { get; set; }=new List<Employee>();

    }
}
