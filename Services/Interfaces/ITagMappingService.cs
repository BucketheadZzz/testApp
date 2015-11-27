using System.Collections.Generic;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface ITagMappingService<T>
    {
        void AddMapping(IEnumerable<T> tags);

        void RemoveMapping(int newsId);

       // void RemoveMappingByTagId(int tagId);

        IList<Tag> GetTagsByMapping(int objId);

        bool AlreadyMapped(int tagId, int objId);

        //IList<TagWidgetModel> GetNewsTagsList(); 

      
    }
}
