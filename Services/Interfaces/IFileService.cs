﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface IFileService
    {

        IQueryable<File> GetAll();
        File GetById(int id);

        File Add(HttpPostedFileWrapper file);

        IEnumerable<File> Add(IEnumerable<HttpPostedFileWrapper> files); 

        void Delete(int id);

        void Delete(int[] ids);
    }
}
