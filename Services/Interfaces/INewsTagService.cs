using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
