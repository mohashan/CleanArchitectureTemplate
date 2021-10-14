using AutoMapper;

namespace Application.Common.Mapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile profile);
    }
}