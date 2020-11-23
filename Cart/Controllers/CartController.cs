using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.Models;
using Cart.Models.ViewModel;
using Cart.Repository;

namespace Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CartController));
        private readonly ICartRepo CartRepo;
        public CartController(ICartRepo Cart)
        {
            this.CartRepo = Cart;
        }
        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> AddBook([FromBody] CartViewModel model)
        {
            _log4net.Info("API initiated");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var roomAdded = await CartRepo.AddRoom(model);
            if (roomAdded == -1)
            {
                return new StatusCodeResult(400);
            }
            else if (roomAdded == 1)
            {
                _log4net.Info("Book Added");
                return new StatusCodeResult(201);
            }
            else
            {
                return new StatusCodeResult(500);
            }
        }
    }
}



