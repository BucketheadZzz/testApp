using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TestApp.Models.Domain
{
    [Table("Tag")]
    public class Tag : EntityTypeConfiguration<Tag> 
    {
        public Tag()
        {
            NewsTagMapping = new HashSet<NewsTagMapping>();

            PlayListTagMapping = new HashSet<PlayListTagMapping>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<NewsTagMapping> NewsTagMapping { get; set; }

        public virtual ICollection<PlayListTagMapping> PlayListTagMapping { get; set; }
    }

   
}