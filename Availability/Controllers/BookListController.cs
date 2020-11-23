using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Availability.Models;
using Availability.Repository;
using Microsoft.Extensions.Logging.Log4Net;

namespace Availability.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookListController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(BookListController));
        private readonly IBookRepo BookRepo;
        public BookListController(IBookRepo Book)
        {
            this.BookRepo = Book;
        }
        [HttpGet]
        [Route("GetList")]
        public async Task<IActionResult> GetList()
        {
            _log4net.Info("API initiated to Get Book list");
            var listOfBooks = await BookRepo.GetBookList();
            if (listOfBooks == null)
            {
                //ModelState.AddModelError("", "No Books found");
                return NotFound();
                //return new StatusCodeResult(400);
            }
            _log4net.Info("No of Books retrieved : "+listOfBooks.Count());
            return Ok(listOfBooks);
            //return new StatusCodeResult(200);
            //return Ok();

        }
    }
}

