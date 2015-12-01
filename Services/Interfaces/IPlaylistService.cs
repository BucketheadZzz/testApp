using System.Collections.Generic;
using System.Linq;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface IPlaylistService
    {
        void Add(Playlist playlist);

        void Update(Playlist playlist);

        void Delete(int id);

        Playlist GetById(int id);

        IList<PlaylistModel> GetModelsByTag(string tag);

        IList<PlaylistModel> GetModels();

    }
}