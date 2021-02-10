namespace DataBaseManagement.CatManagement
{
    public interface ICatRepository
    {
        public int Create(CatCreateInDbModel info);
        public CatInDbModel Get(int id);
    }
}
