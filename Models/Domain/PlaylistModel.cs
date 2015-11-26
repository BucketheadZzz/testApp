
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TestApp.Models.Domain
{
    [Table("Playlist")]
    public class Playlist : EntityTypeConfiguration<Playlist> 
    {
        public Playlist()
        {
            PlayListFileMapping = new HashSet<PlayListFileMapping>();

            PlayListTagMapping = new HashSet<PlayListTagMapping>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public virtual ICollection<PlayListFileMapping> PlayListFileMapping { get; set; }

        public virtual ICollection<PlayListTagMapping> PlayListTagMapping { get; set; }

    }

    [Table("PlayListFile_Mapping")]
    public class PlayListFileMapping : EntityTypeConfiguration<PlayListFileMapping>
    {
        public int Id { get; set; }

        public int PlaylistId { get; set; }

        public int FileId { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual File File { get; set; }
    }


    [Table("PlayListTag_Mapping")]
    public class PlayListTagMapping : EntityTypeConfiguration<PlayListTagMapping>
    {
        public int Id { get; set; }

        public int PlaylistId { get; set; }

        public int TagId { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual Tag Tag { get; set; }
    }
}