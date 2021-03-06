﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prak.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using static Prak.Controllers.jQueriesController;
using System.Web.Mvc;
using Microsoft.CSharp;


namespace Prak.Tests
{
    [TestClass()]
    public class jQueriesControllerTests
    {
        jQueriesController jQ;

        //Простенький тест для проверки работы метода
        [TestMethod()]
        public void MyPowTest()
        {
            jQ = new jQueriesController();
            Assert.AreEqual(4, jQ.MyPow(2));
        }

        //Отрицательный результат
        [TestMethod()]
        public void MyPowNoTest()
        {
            jQ = new jQueriesController();
            Assert.AreNotEqual(7, jQ.MyPow(2));
        }

        //Проверим имя запускаемого представления
        [TestMethod()]
        public void SomeTextTestNameTest()
        {
            var jQ = new jQueriesController();
            ViewResult result = jQ.SomeText() as ViewResult;
            Assert.AreEqual("SomeText", result.ViewName);
        }

        //Отрицательный результат
        [TestMethod()]
        public void SomeTextTestNameNoTest()
        {
            var jQ = new jQueriesController();
            ViewResult result = jQ.SomeText() as ViewResult;
            Assert.AreNotEqual("NO", result.ViewName);
        }

        //Тест для проверки информации передающейся на представление
        [TestMethod()]
        public void SomeTextTestMassageTest()
        {
            var jQ = new jQueriesController();
            ViewResult result = jQ.SomeText() as ViewResult;
            Assert.AreEqual("My text in ViewBag.Message", result.ViewBag.Message);
        }

        //Проверим Отрицательный результат
        [TestMethod()]
        public void SomeTextTestNoMassageTest()
        {
            var jQ = new jQueriesController();
            ViewResult result = jQ.SomeText() as ViewResult;
            Assert.AreNotEqual("No", result.ViewBag.Message);
        }

        //Проверим как передается модель в представление 
        [TestMethod()]
        public void QueryIndexTest()
        {
            //Создадим 1 заявку
            jQuery query = new jQuery();
            List<jQuery> queryList = new List<jQuery> { query };
            //Сымитируем работу бд
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetQueryList()).Returns(queryList);
            jQ = new jQueriesController(mock.Object);
            ViewResult result = jQ.QueryIndex() as ViewResult;
            Assert.AreEqual("1", result.ViewBag.Message);
        }

        //Проверим Отрицательный результат
        [TestMethod()]
        public void QueryIndexNoTest()
        {
            //Создадим 1 заявку
            jQuery query = new jQuery();
            List<jQuery> queryList = new List<jQuery> { query };
            //Сымитируем работу бд
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetQueryList()).Returns(queryList);
            jQ = new jQueriesController(mock.Object);
            ViewResult result = jQ.QueryIndex() as ViewResult;
            Assert.AreNotEqual("2", result.ViewBag.Message);
        }

        //Проверим логику поиска Администраторов в системе
        [TestMethod()]
        public void GetAdminTest()
        {
            //Создадим пользователей
            AspNetUsers user1 = new AspNetUsers();
            user1.Id = "Id1";
            AspNetUsers user2 = new AspNetUsers();
            user1.Id = "Id2";
            List<AspNetUsers> users = new List<AspNetUsers> { user1, user2 };

            //Создадим роли
            AspNetRoles admin = new AspNetRoles();
            admin.Name = "Admin";
            admin.Id = "admin1";
            AspNetRoles user = new AspNetRoles();
            user.Name = "User";
            user.Id = "user1";
            List<AspNetRoles> rols = new List<AspNetRoles> { admin, user };

            //Распределим роли по пользователям
            AspNetUserRoles userRol1 = new AspNetUserRoles();
            userRol1.UserRoleId = 1;
            userRol1.UserId = "Id1";
            userRol1.RoleId = "admin1";
            AspNetUserRoles userRol2 = new AspNetUserRoles();
            userRol2.UserRoleId = 2;
            userRol2.UserId = "Id2";
            userRol2.RoleId = "user1";
            List<AspNetUserRoles> userRols = new List<AspNetUserRoles> { userRol1, userRol2 };

            //Чтобы наши тесты работали без использования бд воспользуемся Фреймворком Moq
            var mock = new Mock<IRepository>();
            //Заменим используемые для работы с бд методы, сразу готовыми значениями
            mock.Setup(a => a.GetUserList()).Returns(users);
            mock.Setup(b => b.GetRoleList()).Returns(rols);
            mock.Setup(c => c.GetUserRoleList()).Returns(userRols);
            mock.Setup(d => d.GetUserFromDb("Id1")).Returns(user1);
            mock.Setup(d => d.GetUserFromDb("Id2")).Returns(user2);
            jQ = new jQueriesController(mock.Object);
            //Администратора мы сделали только 1 по этому вернуться должно 1
            Assert.AreEqual(1, jQ.GetAdmin().Count);
        }

        //Проверим логику поиска Администраторов в системе когда их нет
        [TestMethod()]
        public void GetAdminZeroTest()
        {

            AspNetUsers user1 = new AspNetUsers();
            user1.Id = "Id1";
            AspNetUsers user2 = new AspNetUsers();
            user1.Id = "Id2";
            List<AspNetUsers> users = new List<AspNetUsers> { user1, user2 };


            AspNetRoles admin = new AspNetRoles();
            admin.Name = "Admin";
            admin.Id = "admin1";
            AspNetRoles user = new AspNetRoles();
            user.Name = "User";
            user.Id = "user1";
            List<AspNetRoles> rols = new List<AspNetRoles> { admin, user };


            AspNetUserRoles userRol1 = new AspNetUserRoles();
            userRol1.UserRoleId = 1;
            userRol1.UserId = "Id1";
            userRol1.RoleId = "user1"; //назначим всех простыми юзерами
            AspNetUserRoles userRol2 = new AspNetUserRoles();
            userRol2.UserRoleId = 2;
            userRol2.UserId = "Id2";
            userRol2.RoleId = "user1";
            List<AspNetUserRoles> userRols = new List<AspNetUserRoles> { userRol1, userRol2 };

            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetUserList()).Returns(users);
            mock.Setup(b => b.GetRoleList()).Returns(rols);
            mock.Setup(c => c.GetUserRoleList()).Returns(userRols);
            mock.Setup(d => d.GetUserFromDb("Id1")).Returns(user1);
            mock.Setup(d => d.GetUserFromDb("Id2")).Returns(user2);
            jQ = new jQueriesController(mock.Object);
            //В системе нет Администраторов ожидаем ответ 0
            Assert.AreEqual(0, jQ.GetAdmin().Count);
        }


    }
}