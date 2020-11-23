using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.Models;
using Cart.Models.ViewModel;

namespace Cart.Repository
{
    public interface ICartRepo
    {
        public Task<int> AddRoom(CartViewModel model);

    }
}
