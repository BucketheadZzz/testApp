using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.Models
{
    public class NewsModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Max length - 200")]
        public string Title { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Max length - 500")]
        public string ShortDescrpition { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public string Tags { get; set; }

        public IList<HttpPostedFileWrapper> Files { get; set; }

        public ICollection<NewsFileMapping> NewsFilesMappings { get; set; }

        public int NewsId { get; set; }
    }
}