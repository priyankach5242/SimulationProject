using Login.Models;
using Login.Models.ViewModel;
using Login.Controllers;
using Login.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;

namespace LoginTest
{
    public class Tests
    {
        List<LoginModel> user = new List<LoginModel>();
        IQueryable<LoginModel> userdata;
        Mock<DbSet<LoginModel>> mockSet;
        Mock<AppDbContext> usercontextmock;
        [SetUp]
        public void Setup()
        {
            user = new List<LoginModel>()
            {
                new LoginModel{Username="Priyanka",Password="priyanka"}

            };
            userdata = user.AsQueryable();
            mockSet = new Mock<DbSet<LoginModel>>();
            mockSet.As<IQueryable<LoginModel>>().Setup(m => m.Provider).Returns(userdata.Provider);
            mockSet.As<IQueryable<LoginModel>>().Setup(m => m.Expression).Returns(userdata.Expression);
            mockSet.As<IQueryable<LoginModel>>().Setup(m => m.ElementType).Returns(userdata.ElementType);
            mockSet.As<IQueryable<LoginModel>>().Setup(m => m.GetEnumerator()).Returns(userdata.GetEnumerator());
            var p = new DbContextOptions<AppDbContext>();
            usercontextmock = new Mock<AppDbContext>(p);
            usercontextmock.Setup(x => x.AccLogin).Returns(mockSet.Object);



        }


        [Test]
        public void LoginTest()
        {

            Mock<IConfiguration> config = new Mock<IConfiguration>();
            config.Setup(p => p["Jwt:Key"]).Returns("ThisismySecretKey");
            var loginRepo = new LoginRepo(usercontextmock.Object);
            var controller = new LoginController(config.Object, loginRepo);
            var auth = controller.Login(new LoginViewModel() { Username = "Priyanka", Password = "priyanka" });
            var isDone = auth.IsCompleted;
            
            Assert.AreEqual(true, isDone);
        }

        
        [Test]
        public void LoginTestFail()
        {

            Mock<IConfiguration> config = new Mock<IConfiguration>();
            config.Setup(p => p["Jwt:Key"]).Returns("ThisismySecretKey");
            var loginRepo = new LoginRepo(usercontextmock.Object);
            var controller = new LoginController(config.Object, loginRepo);
            var auth = controller.Login(new LoginViewModel { Username = "abc", Password = "c123" });
            var isDone = auth.IsCompletedSuccessfully;
            Assert.AreEqual(false, isDone);

        }

    }
}