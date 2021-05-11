using ArtSupplies.Data.Entities;
using ArtSupplies.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSupplies.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            this.CreateMap<Order, OrderModel>();
            this.CreateMap<OrderModel, Order>();
        }
    }
}
