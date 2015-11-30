using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestApp.DAL;
using TestApp.Extensions;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsContext;
        private readonly IFileService<NewsFileMapping> _fileService;
        private readonly ITagService<NewsTagMapping> _tagService;

        public NewsService(ITagService<NewsTagMapping> tagService, IRepository<News> newsContext, IFileService<NewsFileMapping> fileService)
        {
            _tagService = tagService;
            _newsContext = newsContext;
            _fileService = fileService;
   
        }

        public void Add(NewsModel item)
        {
            var entity = item.ToEntity();

            _newsContext.Insert(entity);

            if (!string.IsNullOrEmpty(item.Tags))
            {
                var tagMapping = PrepareTagMappingCollection(_tagService.Add(item.Tags.Split(',')), entity.Id);
                _tagService.AddMapping(tagMapping);
            }
            if (item.Files.Count > 0 & item.Files[0] != null)
            {
                var fileMapping = PrepareFileMappingCollection(_fileService.Add(item.Files), entity.Id);
              _fileService.AddMapping(fileMapping);
            }
        }

        public void Update(NewsModel item)
        {
            var entity = item.ToEntity();

            _newsContext.Update(entity);

            if (!string.IsNullOrEmpty(item.Tags))
            {
                var tagMapping = PrepareTagMappingCollection(_tagService.Add(item.Tags.Split(',')), entity.Id);
                _tagService.AddMapping(tagMapping);
            }
            if (item.Files.Count > 0 & item.Files[0] != null)
            {
                var mapping = PrepareFileMappingCollection(_fileService.Add(item.Files), entity.Id);
                _fileService.AddMapping(mapping);
            }
        }

        public void Delete(int id)
        {
            var removedItem = _newsContext.GetById(id);
            if (removedItem != null)
            {
                _tagService.RemoveMapping(id);
                _fileService.Delete(id);
                _newsContext.Delete(removedItem);
            }

        }

        public NewsModel GetById(int id)
        {
            var enity = _newsContext.Table.SingleOrDefault(x => x.Id == id);
            if (enity != null)
            {
                var model = enity.ToModel();
                model.Tags = string.Join(",", _tagService.GetTagsByMapping(id).Select(x => x.Name));

                return model;
            }
            return new NewsModel();
        }

        public IList<NewsModel> GetModels()
        {
            return _newsContext.Table.ToListModel();
        }

        public IList<NewsModel> GetModelsByTag(string tag)
        {
            return
                (from news in _newsContext.Table
                    where news.NewsTagMapping.Count(x => x.Tag.Name == tag) > 0
                    select news).ToListModel();
        }

        private ICollection<NewsTagMapping> PrepareTagMappingCollection(IEnumerable<Tag> tags, int newsId)
        {
            var resList = new Collection<NewsTagMapping>();
            foreach (var tag in tags)
            {
                resList.Add(new NewsTagMapping { ObjectId = newsId, TagId = tag.Id });
            }
            return resList;
        } 

        private ICollection<NewsFileMapping> PrepareFileMappingCollection(IEnumerable<File> files, int playListId)
        {
            var resList = new Collection<NewsFileMapping>();
            foreach (var file in files)
            {
                resList.Add(new NewsFileMapping() { FileId = file.Id, ObjectId = playListId });
            }
            return resList;
        } 
    }
}