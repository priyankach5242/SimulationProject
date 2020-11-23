using NUnit.Framework;
using Availability.Models;
using Availability.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace AvailabilityTest
{
    public class Tests
    {
        List<BookListModel> books = new List<BookListModel>();
        IQueryable<BookListModel> bookdata;
        Mock<DbSet<BookListModel>> mockSet;
        Mock<AppDbContext> bookcontextmock;
        [SetUp]
        public void Setup()
        {
            books = new List<BookListModel>()
            {
                new BookListModel(){Id=1,Name="Harry Potter and the Philosopher's Stone",Price=300,Description="1st book of Harry Potter series"},
                new BookListModel(){Id=2,Name="Harry Potter and the Chamber of Secrets",Price=349,Description="2nd book of Harry Potter series"},
                new BookListModel(){Id=3,Name="Harry Potter and the Prisoner of Azkaban",Price=301,Description="3rd book of Harry Potter series"},
                new BookListModel(){Id=4,Name="Harry Potter and the Goblet of Fire",Price=359,Description="4th book of Harry Potter series"}

            };
            bookdata = books.AsQueryable();
            mockSet = new Mock<DbSet<BookListModel>>();

            mockSet.As<IQueryable<BookListModel>>().Setup(m => m.Provider).Returns(bookdata.Provider);
            mockSet.As<IQueryable<BookListModel>>().Setup(m => m.Expression).Returns(bookdata.Expression);
            mockSet.As<IQueryable<BookListModel>>().Setup(m => m.ElementType).Returns(bookdata.ElementType);
            mockSet.As<IQueryable<BookListModel>>().Setup(m => m.GetEnumerator()).Returns(bookdata.GetEnumerator());
            var p = new DbContextOptions<AppDbContext>();
            bookcontextmock = new Mock<AppDbContext>(p);
            bookcontextmock.Setup(x => x.BookList).Returns(mockSet.Object);
        }

        [Test]
        public void GetAllTest()
        {
            var bookRepo = new BookRepo(bookcontextmock.Object);
            var bookList = bookRepo.GetBookList();
            Assert.AreEqual(4, books.Count());
            Assert.AreNotEqual(5, books.Count());
        }

    }
}