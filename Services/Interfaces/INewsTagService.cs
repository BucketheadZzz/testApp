using System.Collections.Generic;
using TestApp.Models;

namespace TestApp.Services.Interfaces
{
    public interface INewsTagService
    {
        void SaveTagsMappingToNews(string tags, int newsId);

        void RemoveMappingByNewsId(int newsId);

        IList<TagWidgetModel> GetNewsTagsList(); 

        string GetTagsByNewsId(int newsId);

        bool IsAlreadyMapped(int tagId, int newsId);
    }
}
