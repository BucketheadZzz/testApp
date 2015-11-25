using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace TestApp.Models.Domain
{
    public class FileContext: DbContext
    {
        public FileContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<File> Files { get; set; }

        public DbSet<NewsFileMapping> NewsFilesMappings { get; set; }
    }

    [Table("File")]
    public class File
    {
        public File()
        {
            NewsFilesMappings = new HashSet<NewsFileMapping>();
        }
        public int Id { get; set; }

        public byte[] BinaryData { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public virtual ICollection<NewsFileMapping> NewsFilesMappings { get; set; } 
       // public ICollection<> 
    }

    [Table("NewsFile_Mapping")]
    public class NewsFileMapping
    {
        public int Id { get; set; }

        public int NewsId { get; set; }

        public int FileId { get; set; }

        public virtual File File { get; set; }

        public virtual News News { get; set; }


    }
}