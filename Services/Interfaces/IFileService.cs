using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface IFileService
    {
        File GetById(int id);

        int Add(File picture);

        void Delete(int id);
    }
}
