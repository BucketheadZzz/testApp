using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApp.Models.Domain
{
    public interface IBaseTagMappingEntity
    {
         int Id { get; }
         int ObjectId { get; }
         int TagId { get; set; }
    }
}