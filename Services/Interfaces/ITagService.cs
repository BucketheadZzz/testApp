using System.Collections.Generic;
using System.Linq;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface ITagService
    {
        IQueryable<Tag> GetAll();
 
        Tag Add(string tagName);

        IEnumerable<Tag> Add(IEnumerable<string> tags); 

        void Delete(string tagName);


    }
}
