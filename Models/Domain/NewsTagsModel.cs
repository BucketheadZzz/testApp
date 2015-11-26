using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TestApp.Models.Domain
{
    [Table("Tag")]
    public class Tag : EntityTypeConfiguration<Tag> 
    {

        public Tag()
        {
            NewsTagMapping = new HashSet<NewsTagMapping>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<NewsTagMapping> NewsTagMapping { get; set; }
    }

    [Table("NewsTag_Mapping")]
    public class NewsTagMapping : EntityTypeConfiguration<NewsTagMapping> 
    {
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int NewsId { get; set; }

        public int TagId { get; set; }

        public virtual News News { get; set; }
        public virtual Tag Tag { get; set; }
       
    }
}