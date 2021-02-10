namespace Services.CatSharingManagement
{
    public interface ICatSharingService
    {
        public ServiceResult<CatSharingModel> Share(CatSharingCreateModel info, int ownerId);
    }
}
