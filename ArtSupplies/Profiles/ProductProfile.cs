using ArtSupplies.Data.Entities;
using ArtSupplies.Models; 
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSupplies
{
    class ProductProfile : Profile
    {
        public ProductProfile()
        {
            this.CreateMap<Product, ProductModel>();
            this.CreateMap<ProductModel, Product>();
        }
    }
}
