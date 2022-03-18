namespace core.hybrid.repository.Mapper.Package
{
    using AutoMapper;
    using core.hybrid.repository.Models;

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Test, Test>();
            CreateMap<Test, data.hybrid.repository.Entities.Test>();
            CreateMap<Test, data.hybrid.repository.Entities.Test>();
            CreateMap<data.hybrid.repository.Entities.Test, Test>();
        }
    }
}
