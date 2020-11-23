using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;
using Login.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Login.Repository
{
    public class LoginRepo : ILoginRepo
    {
        private readonly AppDbContext dbContext;

        public LoginRepo(AppDbContext context)
        {
            this.dbContext = context;
        }
        public async Task<LoginModel> GetAccount(LoginViewModel model)
        {
            var output = await dbContext.AccLogin.Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefaultAsync();
            return output;
        }
    }
}