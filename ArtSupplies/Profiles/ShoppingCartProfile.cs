using ArtSupplies.Data.Entities;
using ArtSupplies.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSupplies
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            this.CreateMap<ShoppingCart, ShoppingCartModel>();
            this.CreateMap<ShoppingCartModel, ShoppingCart>();
        }
    }
}
