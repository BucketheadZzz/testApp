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
            Mapper.CreateMap<News, NewsModel>().ForMember(x => x.NewsFilesMappings,y => y.MapFrom(z => z.NewsFileMappings));
            Mapper.CreateMap<NewsModel, News>();
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

    }
}