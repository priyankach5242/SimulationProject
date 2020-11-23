using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.Models;
using Cart.Models.ViewModel;
using Microsoft.EntityFrameworkCore;


namespace Cart.Repository
{
    public class CartRepo : ICartRepo 
    {
        private readonly AppDbContext dbContext;

        public CartRepo(AppDbContext context)
        {
            this.dbContext = context;
        }
        public async Task<int> AddRoom(CartViewModel model)
        {

            var alreadyExist = await dbContext.BookList.Where(x => x.Name == model.Name).FirstOrDefaultAsync();
            if (alreadyExist != null)
            {
                return -1;
            }
            else
            {
                CartModel temp = new CartModel()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description
                };
                dbContext.BookList.Add(temp);
                var row = dbContext.SaveChanges();
                return row;
            }
        }
    }
}
