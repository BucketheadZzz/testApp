using System.Collections.Generic;
using System.Linq;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface ITagService<T>
    {
        Tag Add(string tagName);

        IEnumerable<Tag> Add(IEnumerable<string> tags); 

        void Delete(string tagName);

        void AddMapping(IEnumerable<T> tags);

        void RemoveMapping(int objId);

        IList<Tag> GetTagsByMapping(int objId);

        bool AlreadyMapped(int tagId, int objId);

        ICollection<T> PrepareTagMappingCollection(IEnumerable<Tag> tags, int newsId);

        IList<TagWidgetModel> TagsWidget();

    }
}
