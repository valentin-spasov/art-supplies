using ArtSupplies.Data.Entities;
using ArtSupplies.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSupplies
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            this.CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.Street, o => o.MapFrom(m => m.Address.Street))
                .ForMember(cm => cm.StreetNumber, o => o.MapFrom(m => m.Address.StreetNumber))
                .ForMember(cm => cm.ZipCode, o => o.MapFrom(m => m.Address.ZipCode))
                .ForMember(cm => cm.City, o => o.MapFrom(m => m.Address.City))
                .ForMember(cm => cm.Country, o => o.MapFrom(m => m.Address.Country));

            this.CreateMap<CustomerModel, Customer>()
                .ForMember(c => c.Address, o => o.MapFrom(m => new Address() 
                    { 
                        Street = m.Street,
                        StreetNumber = m.StreetNumber,
                        ZipCode = m.ZipCode,
                        City = m.City, 
                        Country = m.Country
                    }));
        }
    }
}
