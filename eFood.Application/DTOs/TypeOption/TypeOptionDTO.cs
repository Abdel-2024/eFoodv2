﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.DTOs
{
    public class TypeOptionDTO
    {
        public int TypeOptionId { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<OptionsDTO> Options { get; set; }            
    }
}
