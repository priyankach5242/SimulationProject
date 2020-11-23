using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Availability.Models;

namespace Availability.Repository
{
    public interface IBookRepo
    {
        public Task<List<BookListModel>> GetBookList();

    }
}
