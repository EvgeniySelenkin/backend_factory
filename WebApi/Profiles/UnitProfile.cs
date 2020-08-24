using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Odt;

namespace WebApi.Profiles
{
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            //Основной mapper
            CreateMap<Unit, UnitOdt>().ForMember("Factory", opt => opt.Ignore()).ForMember("Tanks", opt => opt.Ignore());
            //Для полей Factory и Tank
            CreateMap<Unit, UnitListOdt>();
        }
        
    }
}
