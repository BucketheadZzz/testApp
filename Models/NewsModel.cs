using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace TestApp.Models
{

    public class NewsContext : DbContext
    {
        public NewsContext() : base("DefaultConnection")
        {
            
        }

        public DbSet<News> News { get; set; }
    }

    [Table("News")]
    public class News
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Max length - 200")]
        public string Title { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Max length - 500")]
        public string ShortDescrpition { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }
    }
}