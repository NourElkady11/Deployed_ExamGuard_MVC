﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CourseStudent
    {
        public int CourseId { get; set; }

        public int StudentId { get; set; }

        public Student? Student { get; set; }

        public Course? Course { get; set; }

   

    }
}
