﻿using System.Collections.Generic;
using TestApp.Models;

namespace TestApp.Services.Interfaces
{
    public interface INewsTagService
    {
        void SaveTagsMapping(string tags, int newsId);

        void RemoveMapping(int newsId);

        void RemoveMappingByTagId(int tagId);

        IList<TagWidgetModel> GetNewsTagsList(); 

        string GetTagsByNewsId(int newsId);

        bool AlreadyMapped(int tagId, int newsId);

      
    }
}
