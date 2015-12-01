namespace TestApp.Models.Domain
{
    public interface IBaseFileMappingEntity
    {
         int Id { get; }
         int ObjectId { get; set; }
         int FileId { get; set; }
    }
}