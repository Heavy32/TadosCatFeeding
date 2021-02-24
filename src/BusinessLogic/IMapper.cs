namespace BusinessLogic
{
    public interface IMapper
    {
        public Tdestination Map<Tdestination, TSource>(TSource model);
    }
}
