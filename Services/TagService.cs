using System.Linq;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagsContext;

        public TagService(IRepository<Tag> tagsContext)
        {
            _tagsContext = tagsContext;
        }


        public IQueryable<Tag> GetAll()
        {
            return _tagsContext.Table;
        }

        public int Add(string tagName)
        {
            if (AlreadyExist(tagName))
            {
                var existingTag = GetTagByName(tagName);
                return existingTag != null ? existingTag.Id : -1;
            }
            var addedTag = _tagsContext.Insert(new Tag { Name = tagName });
       
            return addedTag.Id;
        }

        public Tag GetTagByName(string tagName)
        {
            return _tagsContext.Table.SingleOrDefault(x => x.Name == tagName);
        }

    

        public void Delete(string tagName)
        {
            var removedTag = _tagsContext.Table.SingleOrDefault(x => x.Name == tagName);
            if (removedTag == null) return;

            _tagsContext.Delete(removedTag);
        }



        public bool AlreadyExist(string tag)
        {
            return _tagsContext.Table.Any(x => x.Name == tag);
        }

      
     
    }
}