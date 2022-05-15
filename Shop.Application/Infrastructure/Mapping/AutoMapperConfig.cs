namespace Shop.Application.Infrastructure.Mapping
{
    public static class AutoMapperConfig
    {
        public static IMapper Mapper { get; private set; }
        public static void Init(MapperConfiguration config)
        {
            Mapper = config.CreateMapper();
        }
    }
}
