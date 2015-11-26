using System.Linq;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _TagsContext;

        public TagService(IRepository<Tag> TagsContext)
        {
            _TagsContext = TagsContext;
        }


        public IQueryable<Tag> GetAll()
        {
            return _TagsContext.Table;
        }

        public int Add(string tagName)
        {
            if (IsTagAlreadyExit(tagName)) return GetTagIdByName(tagName);

            var addedTag = _TagsContext.Insert(new Tag { Name = tagName });
       
            return addedTag.Id;
        }

        public int GetTagIdByName(string tagName)
        {
            var tag = _TagsContext.Table.SingleOrDefault(x => x.Name == tagName);

            return tag != null ? tag.Id : -1;
        }

    

        public void Delete(string tagName)
        {
            var removedTag = _TagsContext.Table.SingleOrDefault(x => x.Name == tagName);
            if (removedTag == null) return;

            _TagsContext.Delete(removedTag);
        }



        public bool IsTagAlreadyExit(string tag)
        {
            return _TagsContext.Table.Any(x => x.Name == tag);
        }

      
     
    }
}