﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksHub.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Cover Type")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
