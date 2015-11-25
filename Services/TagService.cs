using System.Linq;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class TagService : ITagService
    {
        private readonly NewsTagsContext _newsTagsContext;

        public TagService(NewsTagsContext newsTagsContext)
        {
            _newsTagsContext = newsTagsContext;
        }

    

        public int Add(string tagName)
        {
            if (IsTagAlreadyExit(tagName)) return GetTagIdByName(tagName);

            var addedTag = _newsTagsContext.NewsTags.Add(new NewsTag { Name = tagName });
            _newsTagsContext.SaveChanges();

            return addedTag.Id;
        }

        public int GetTagIdByName(string tagName)
        {
            var tag = _newsTagsContext.NewsTags.SingleOrDefault(x => x.Name == tagName);

            return tag != null ? tag.Id : -1;
        }

    

        public void Delete(string tagName)
        {
            var removedTag = _newsTagsContext.NewsTags.SingleOrDefault(x => x.Name == tagName);
            if (removedTag == null) return;

            _newsTagsContext.NewsTags.Remove(removedTag);
            _newsTagsContext.SaveChanges();
        }



        public bool IsTagAlreadyExit(string tag)
        {
            return _newsTagsContext.NewsTags.Any(x => x.Name == tag);
        }

      
     
    }
}