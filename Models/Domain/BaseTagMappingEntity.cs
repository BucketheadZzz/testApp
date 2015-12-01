namespace TestApp.Models.Domain
{
    public interface IBaseTagMappingEntity
    {
         int Id { get; }
         int ObjectId { get; set; }
         int TagId { get; set; }
    }
}