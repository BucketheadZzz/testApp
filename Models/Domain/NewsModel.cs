﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TestApp.Models.Domain
{

    [Table("News")]
    public class News : EntityTypeConfiguration<News> 
    {

        public News()
        {
            NewsTagMapping = new HashSet<NewsTagMapping>();
            NewsFileMappings = new HashSet<NewsFileMapping>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescrpition { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public virtual ICollection<NewsTagMapping> NewsTagMapping { get; set; }

        public virtual ICollection<NewsFileMapping> NewsFileMappings { get; set; } 
       
    }
}