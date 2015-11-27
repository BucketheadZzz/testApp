using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.Models
{
    public class PlaylistModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Max length - 200")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Max length - 500")]
        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public string Tags { get; set; }

        public IList<HttpPostedFileWrapper> Files { get; set; }

        public ICollection<PlayListFileMapping> PlayListFilesMappings { get; set; } 
    }
}