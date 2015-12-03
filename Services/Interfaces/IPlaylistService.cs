using System.Collections.Generic;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface IPlaylistService
    {
        void Add(Playlist playlist);

        void Update(Playlist playlist);

        void Delete(int id);

        Playlist GetById(int id);

        IList<Playlist> GetPlayListsByTag(string tag);

        IList<Playlist> GetPlayLists();

    }
}