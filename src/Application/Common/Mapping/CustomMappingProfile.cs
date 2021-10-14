using AutoMapper;
using System.Collections.Generic;

namespace Application.Common.Mapping
{
    public class CustomMappingProfile : Profile
    {
        public CustomMappingProfile(IEnumerable<IHaveCustomMapping> haveCustomMappings)
        {
            foreach (var item in haveCustomMappings)
            {
                item.CreateMappings(this);
            }
        }
    }
}