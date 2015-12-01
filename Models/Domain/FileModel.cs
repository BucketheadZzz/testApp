using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TestApp.Models.Domain
{
    [Table("File")]
    public class File : EntityTypeConfiguration<File> 
    {
        public File()
        {
            NewsFilesMappings = new HashSet<NewsFileMapping>();

            PlayListFileMappings = new HashSet<PlayListFileMapping>();
        }
        public int Id { get; set; }

        public byte[] BinaryData { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public virtual ICollection<NewsFileMapping> NewsFilesMappings { get; set; }

        public virtual ICollection<PlayListFileMapping> PlayListFileMappings { get; set; } 
    }

   
}