using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class FileService: IFileService
    {
        private readonly FileContext _fileContext;

        public FileService(FileContext fileContext)
        {
            _fileContext = fileContext;
        }

        public File GetById(int id)
        {
            return _fileContext.Files.SingleOrDefault(x => x.Id == id);
        }

        public int Add(File file)
        {
            var added = _fileContext.Files.Add(file);
            _fileContext.SaveChanges();

            return added.Id;
        }

        public void Delete(int id)
        {
            var deletedItem = _fileContext.Files.SingleOrDefault(x => x.Id == id);
            if (deletedItem != null)
            {
                _fileContext.Files.Remove(deletedItem);
                _fileContext.SaveChanges();
            }
        }
    }
}