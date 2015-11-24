using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace TestApp.Models.Domain
{
    public class NewsTagsContext: DbContext
    {
        public NewsTagsContext()
            : base("DefaultConnection")
        {
            
        }

        public DbSet<NewsTag_Mapping> NewsTagMappings { get; set; }

        public DbSet<NewsTag> NewsTags { get; set; }
    }

    [Table("NewsTag")]
    public class NewsTag
    {

        public NewsTag()
        {
            NewsTag_Mapping = new HashSet<NewsTag_Mapping>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<NewsTag_Mapping> NewsTag_Mapping { get; set; }
    }

    [Table("NewsTag_Mapping")]
    public class NewsTag_Mapping
    {

        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int NewsId { get; set; }

        public int NewsTagId { get; set; }

        public virtual News News { get; set; }
        public virtual NewsTag NewsTag { get; set; }
       
    }
}