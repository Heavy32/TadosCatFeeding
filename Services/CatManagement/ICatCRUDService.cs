namespace Services.CatManagement
{
    public interface ICatCRUDService
    {
        public ServiceResult<CatServiceModel> Create(CatCreateServiceModel info);
        public ServiceResult<CatGetServiceModel> Get(int id);
    }
}
