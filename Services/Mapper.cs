using AutoMapper;

namespace Services
{
    public class Mapper : IMapper
    {
        public Tdestination Map<Tdestination, TSource>(TSource model)
        {
            var config = new MapperConfiguration(config => config.CreateMap(typeof(TSource), typeof(Tdestination)));
            var mapper = new AutoMapper.Mapper(config);
            return mapper.Map<Tdestination>(model);
        }
    }
}
