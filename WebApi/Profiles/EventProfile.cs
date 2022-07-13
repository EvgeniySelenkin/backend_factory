using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Odt;
using WebApi.Models;

namespace WebApi.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventOdt>().ForMember("Tags", opt => opt.MapFrom(u => u.SerializeTags()))
                                        .ForMember("ResponsibleOperators", opt => opt.MapFrom(u => u.SerializeOperators()))
                                        .ForMember("Id", opt => opt.Ignore())
                                        .ForMember("EventId", opt => opt.MapFrom(u => u.Id));
        }
    }
}
