using ArtSupplies.Data.Entities;
using ArtSupplies.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSupplies.Profiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            this.CreateMap<CartItem, CartItemModel>();
            this.CreateMap<CartItemModel, CartItem>();
        }
    }
}
