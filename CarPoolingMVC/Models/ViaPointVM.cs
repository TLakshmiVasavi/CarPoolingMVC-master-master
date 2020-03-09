﻿using System.ComponentModel.DataAnnotations;

namespace CarPoolingMVC.Models
{
    public class ViaPointVM
    {
        [Required]
        public string Location { get; set; }

        [Required]
        public float Distance { get; set; }
    }
}
