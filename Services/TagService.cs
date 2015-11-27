using System.Collections.Generic;
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

        public Tag Add(string tagName)
        {
            var existingTag = _tagsContext.Table.SingleOrDefault(x => x.Name == tagName);
            if (existingTag != null)
            {
                return existingTag;
            }
                
            var addedTag = _tagsContext.Insert(new Tag { Name = tagName });
       
            return addedTag;
        }

        public IEnumerable<Tag> Add(IEnumerable<string> tags)
        {
            return tags.Select(Add).ToList();
        }


        public void Delete(string tagName)
        {
            var removedTag = _tagsContext.Table.SingleOrDefault(x => x.Name == tagName);
            if (removedTag == null) return;

            _tagsContext.Delete(removedTag);
        }


  

      
     
    }
}