using System.Collections.Generic;
using System.Linq;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface IPlaylistService
    {
        int Add(PlaylistModel playlist);

        void Update(PlaylistModel playlist);

        void Delete(int id);

        PlaylistModel GetById(int id);

        IQueryable<Playlist> GetAll();

        IList<PlaylistModel> GetPlaylistModels();

    }
}