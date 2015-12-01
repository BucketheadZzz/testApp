using System.Collections.Generic;
using AutoMapper;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Extensions
{
    public static class MapperModels
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<News, NewsModel>().ForMember(x => x.NewsFilesMappings, y => y.MapFrom(z => z.NewsFileMappings));
            Mapper.CreateMap<NewsModel, News>();

            Mapper.CreateMap<Playlist, PlaylistModel>().ForMember(x => x.PlayListFilesMappings, y => y.MapFrom(z => z.PlayListFileMapping));
            Mapper.CreateMap<PlaylistModel, Playlist>();
        }

        public static NewsModel ToModel(this News ent)
        {
            return Mapper.Map<News, NewsModel>(ent);
        }

        public static IList<NewsModel> ToListModel(this IEnumerable<News> ent)
        {
            return Mapper.Map<IEnumerable<News>, IList<NewsModel>>(ent);
        }

        public static News ToEntity(this NewsModel model)
        {
            return Mapper.Map<NewsModel, News>(model);
        }

        public static PlaylistModel ToModel(this Playlist ent)
        {
            return Mapper.Map<Playlist, PlaylistModel>(ent);
        }

        public static Playlist ToEntity(this PlaylistModel model)
        {
            return Mapper.Map<PlaylistModel, Playlist>(model);
        }

        public static IList<PlaylistModel> ToListModel(this IEnumerable<Playlist> ent)
        {
            return Mapper.Map<IEnumerable<Playlist>, IList<PlaylistModel>>(ent);
        }
     
    }
}