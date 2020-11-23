using NUnit.Framework;
using Cart.Models;
using Cart.Models.ViewModel;
using Cart.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace CartTest
{
    public class Tests
    {
        List<CartModel> books = new List<CartModel>();
        IQueryable<CartModel> bookdata;
        Mock<DbSet<CartModel>> mockSet;
        Mock<AppDbContext> bookcontextmock;
        [SetUp]
        public void Setup()
        {

            books = new List<CartModel>()
            {
                new CartModel(){Id=1,Name="Harry Potter and the Philosopher's Stone",Price=300,Description="1st book of Harry Potter series"},
                new CartModel(){Id=2,Name="Harry Potter and the Chamber of Secrets",Price=349,Description="2nd book of Harry Potter series"},
                new CartModel(){Id=3,Name="Harry Potter and the Prisoner of Azkaban",Price=301,Description="3rd book of Harry Potter series"},
                new CartModel(){Id=4,Name="Harry Potter and the Goblet of Fire",Price=359,Description="4th book of Harry Potter series"}

            };
            bookdata = books.AsQueryable();
            mockSet = new Mock<DbSet<CartModel>>();

            mockSet.As<IQueryable<CartModel>>().Setup(m => m.Provider).Returns(bookdata.Provider);
            mockSet.As<IQueryable<CartModel>>().Setup(m => m.Expression).Returns(bookdata.Expression);
            mockSet.As<IQueryable<CartModel>>().Setup(m => m.ElementType).Returns(bookdata.ElementType);
            mockSet.As<IQueryable<CartModel>>().Setup(m => m.GetEnumerator()).Returns(bookdata.GetEnumerator());
            var p = new DbContextOptions<AppDbContext>();
            bookcontextmock = new Mock<AppDbContext>(p);
            bookcontextmock.Setup(x => x.BookList).Returns(mockSet.Object);
        }

        [Test]
        public void GetRoomTest()
        {
            var cartRepo = new CartRepo(bookcontextmock.Object);
            CartViewModel temp = new CartViewModel()
            {
                Name = "Harry Potter and the Order of Phoenix",
                Price = 531,
                Description = "5th book of Harry Potter series"
            };
            CartModel temp1 = new CartModel()
            {
                Name = temp.Name,
                Price = temp.Price,
                Description = temp.Description
            };
            var roomAdded = cartRepo.AddRoom(temp);
            books.Add(temp1);
            Assert.AreNotEqual(4, books.Count());
            Assert.AreEqual(5, books.Count());
        }
    }
}