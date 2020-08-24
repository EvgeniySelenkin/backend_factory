using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Odt;

namespace WebApi.Profiles
{
    public class FactoryProfile : Profile 
    {
        public FactoryProfile()
        {
            //Для общего случая
            CreateMap<Factory, FactoryOdt>().ForMember("Units", opt => opt.Ignore());
            //Для поля Unit
            CreateMap<Factory, FactoryForUnitOdt>();

        }
        
    }
}
