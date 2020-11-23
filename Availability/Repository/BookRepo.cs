using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Availability.Models;
using Microsoft.EntityFrameworkCore;

namespace Availability.Repository
{
    public class BookRepo : IBookRepo
    {
        private readonly AppDbContext dbContext;

        public BookRepo(AppDbContext context)
        {
            this.dbContext = context;
        }
        public async Task<List<BookListModel>> GetBookList()
        {
            return await dbContext.BookList.ToListAsync();
        }
    }
}
