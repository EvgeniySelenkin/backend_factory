using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Odt;

namespace WebApi.Profiles
{
    public class TankProfile : Profile
    {
        public TankProfile()
        {
            //Основной mapper
            CreateMap<Tank, TankOdt>().ForMember("Unit", opt => opt.Ignore());
            //Для поля Unit
            CreateMap<Tank, TankListOdt>();
        }
    }
}
