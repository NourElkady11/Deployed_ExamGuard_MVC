﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class SuperVisor
    {
        public int Id { get; set; }
    
        public string? FirstName { get; set; }
       
        public string? LastName { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? ImageName { get; set; }

        public ICollection<Course>? courses { get; set; }=new List<Course>();

    }
}
