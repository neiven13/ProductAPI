using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductAPI.Entities.Enums;

namespace ProductAPI.Entities
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal ProductPrice { get; set; }
        public ProductType Type { get; set; }
    }
}