using AssetProject.Models;
using AssetProject.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Asset, AssetVm2>();
            CreateMap<AssetVm2, Asset>();
        }
    }
}
