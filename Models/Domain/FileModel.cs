using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TestApp.Models.Domain
{

    [Table("File")]
    public class File : EntityTypeConfiguration<File> 
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
    }

    [Table("NewsFile_Mapping")]
    public class NewsFileMapping : EntityTypeConfiguration<NewsFileMapping> 
    {
        public int Id { get; set; }

        public int NewsId { get; set; }

        public int FileId { get; set; }

        public virtual File File { get; set; }

        public virtual News News { get; set; }


    }
}