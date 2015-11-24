using AutoMapper;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Extensions
{
    public static class MapperModels
    {

        public static void CreateMaps()
        {
            Mapper.CreateMap<News, NewsModel>();
            Mapper.CreateMap<NewsModel, News>();
        }

        public static NewsModel ToModel(this News ent)
        {
            return Mapper.Map<News, NewsModel>(ent);
        }

        public static News ToEntity(this NewsModel model)
        {
            return Mapper.Map<NewsModel, News>(model);
        }
    }
}