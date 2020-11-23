using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;
using Login.Models.ViewModel;

namespace Login.Repository
{
    public interface ILoginRepo
    {
        public Task<LoginModel> GetAccount(LoginViewModel model);
    }
}