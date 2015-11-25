using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TestApp.Models.Domain
{
    public class FileContext: DbContext
    {
        public FileContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<File> Files { get; set; }

        public DbSet<NewsFile_Mapping> NewsFilesMappings { get; set; }
    }

    [Table("Picture")]
    public class File
    {
        public File()
        {
            NewsPictures_Mappings = new HashSet<NewsFile_Mapping>();
        }
        public int Id { get; set; }

        public byte[] BynaryImage { get; set; }

        public string ContentType { get; set; }

        public virtual ICollection<NewsFile_Mapping> NewsPictures_Mappings { get; set; } 
       // public ICollection<> 
    }

    [Table("NewsPictures_Mapping")]
    public class NewsFile_Mapping
    {
        public int Id { get; set; }

        public int NewsId { get; set; }

        public int PictureId { get; set; }

        public virtual File Picture { get; set; }

        public virtual News News { get; set; }


    }
}