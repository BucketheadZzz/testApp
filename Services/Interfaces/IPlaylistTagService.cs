using System.Collections.Generic;
using TestApp.Models;

namespace TestApp.Services.Interfaces
{
    public interface IPlaylistTagService
    {
        void SaveMapping(string tags, int playListId);
        void RemoveMapping(int playListId);
       // void RemoveMapping(int playListId, int tagId);
        bool AlreadyMapped(int playListId, int tagId);

        IList<TagWidgetModel> GetPlaylistTagsList(); 
    }
}