using AuthorizationService.Repositories;
using AuthorizationService.Repositories.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthorizationService.Unit.Tests
{
    [TestClass]
    public class UserRepositoryTest
    {
        private static readonly Mock<IAuthDBContext> AuthDBContext = new Mock<IAuthDBContext>();
        private static readonly Mock<IAuthDBContextFactory> AuthDBContextFactory = new Mock<IAuthDBContextFactory>();

        [TestMethod]
        public void GetUsers_UsersExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email2 = "email2@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username2 = "name2";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt2 = "22ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password2 = "111gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate2 = DateTime.UtcNow.AddMinutes(-6);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id2, UserName = username2, Email = email2, Salt = salt2, Password = password2, CreatedDate = createDate2, UpdatedDate = updateDate2, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUsers();

            AuthDBContext.Verify(x => x.User, Times.Once);
            Assert.AreEqual(result.Count(), 3);
            Assert.IsTrue(result.Any(t => t.Id == id1 && t.UserName == username1 && t.Email == email1 && t.Salt == salt1 && t.CreatedDate == createDate1 && t.Password == password1 && t.UpdatedDate == updateDate1 && t.IsActive == true));
            Assert.IsTrue(result.Any(t => t.Id == id2 && t.UserName == username2 && t.Email == email2 && t.Salt == salt2 && t.CreatedDate == createDate2 && t.Password == password2 && t.UpdatedDate == updateDate2 && t.IsActive == true));
            Assert.IsTrue(result.Any(t => t.Id == id3 && t.UserName == username3 && t.Email == email3 && t.Salt == salt3 && t.CreatedDate == createDate3 && t.Password == password3 && t.UpdatedDate == updateDate3 && t.IsActive == true));
        }

        [TestMethod]
        public void GetUsers_UsersNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            var data = new List<Repositories.Entities.User>().AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUsers();

            AuthDBContext.Verify(x => x.User, Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void GetUserByName_UserExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email2 = "email2@mail.ru";
            var username1 = "name1";
            var username2 = "name2";
            var salt1 = "54ythgdh9";
            var salt2 = "22ythgdh9";
            var password1 = "542gythnfsli8";
            var password2 = "111gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate2 = DateTime.UtcNow.AddMinutes(-6);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id2, UserName = username2, Email = email2, Salt = salt2, Password = password2, CreatedDate = createDate2, UpdatedDate = updateDate2, IsActive = true }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUserByName(username2);

            AuthDBContext.Verify(x => x.User, Times.Exactly(1));
            Assert.AreEqual(result.UserName, username2);
            Assert.AreEqual(result.Email, email2);
            Assert.AreEqual(result.Salt, salt2);
            Assert.AreEqual(result.Password, password2);
            Assert.AreEqual(result.CreatedDate, createDate2);
            Assert.AreEqual(result.UpdatedDate, updateDate2);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.Id, id2);
        }

        [TestMethod]
        public void GetUsersByName_UserNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            var id1 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username2 = "name2";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUserByName(username2);

            AuthDBContext.Verify(x => x.User, Times.Exactly(1));
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetUserByEmail_UserExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email2 = "email2@mail.ru";
            var username1 = "name1";
            var username2 = "name2";
            var salt1 = "54ythgdh9";
            var salt2 = "22ythgdh9";
            var password1 = "542gythnfsli8";
            var password2 = "111gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate2 = DateTime.UtcNow.AddMinutes(-6);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id2, UserName = username2, Email = email2, Salt = salt2, Password = password2, CreatedDate = createDate2, UpdatedDate = updateDate2, IsActive = true }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUserByEmail(email2);

            AuthDBContext.Verify(x => x.User, Times.Exactly(1));
            Assert.AreEqual(result.UserName, username2);
            Assert.AreEqual(result.Email, email2);
            Assert.AreEqual(result.Salt, salt2);
            Assert.AreEqual(result.Password, password2);
            Assert.AreEqual(result.CreatedDate, createDate2);
            Assert.AreEqual(result.UpdatedDate, updateDate2);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.Id, id2);
        }

        [TestMethod]
        public void GetUsersByEmail_UserNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            var id1 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email2 = "email2@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUserByEmail(email2);

            AuthDBContext.Verify(x => x.User, Times.Exactly(1));
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetUser_UserExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email2 = "email2@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username2 = "name2";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt2 = "22ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password2 = "111gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate2 = DateTime.UtcNow.AddMinutes(-6);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id2, UserName = username2, Email = email2, Salt = salt2, Password = password2, CreatedDate = createDate2, UpdatedDate = updateDate2, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUser(id2);

            AuthDBContext.Verify(x => x.User, Times.Once);
            Assert.AreEqual(result.UserName, username2);
            Assert.AreEqual(result.Email, email2);
            Assert.AreEqual(result.Salt, salt2);
            Assert.AreEqual(result.Password, password2);
            Assert.AreEqual(result.CreatedDate, createDate2);
            Assert.AreEqual(result.UpdatedDate, updateDate2);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.Id, id2);
        }

        [TestMethod]
        public void GetUser_UserNotExisted_UseDbContextReturnNull()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUser(id2);

            AuthDBContext.Verify(x => x.User, Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetUserRoles_UserRolesExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.UserRole>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var id4 = Guid.NewGuid();
            var id5 = Guid.NewGuid();
            var id6 = Guid.NewGuid();
            var id7 = Guid.NewGuid();
            var id8 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var roleId1 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            var roleId3 = Guid.NewGuid();
            var roleId4 = Guid.NewGuid();
            var roleId5 = Guid.NewGuid();

            var data = new List<Repositories.Entities.UserRole>
            {
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId3 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId5 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId5 },
                new Repositories.Entities.UserRole { UserId = userId3, RoleId = roleId1 }

            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserRole).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result1 = repository.GetUserRoles(userId1);
            var result2 = repository.GetUserRoles(userId2);
            var result3 = repository.GetUserRoles(userId3);

            AuthDBContext.Verify(x => x.UserRole, Times.Exactly(3));
            Assert.AreEqual(result1.Count(), 4);
            Assert.IsTrue(result1.Contains(roleId2));
            Assert.IsTrue(result1.Contains(roleId3));
            Assert.IsTrue(result1.Contains(roleId4));
            Assert.IsTrue(result1.Contains(roleId5));
            Assert.AreEqual(result2.Count(), 3);
            Assert.IsTrue(result2.Contains(roleId2));
            Assert.IsTrue(result2.Contains(roleId4));
            Assert.IsTrue(result2.Contains(roleId5));
            Assert.AreEqual(result3.Count(), 1);
            Assert.IsTrue(result3.Contains(roleId1));
        }

        [TestMethod]
        public void GetUserRoles_UserRolesNorExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.UserRole>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var id4 = Guid.NewGuid();
            var id5 = Guid.NewGuid();
            var id6 = Guid.NewGuid();
            var id7 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            var roleId3 = Guid.NewGuid();
            var roleId4 = Guid.NewGuid();
            var roleId5 = Guid.NewGuid();

            var data = new List<Repositories.Entities.UserRole>
            {
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId3 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId5 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId5 },

            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserRole).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.GetUserRoles(userId3);

            AuthDBContext.Verify(x => x.UserRole, Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void CreateUser_Correct_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email1@mail.ru";
            var userName = "name1";
            var salt = "54ythgdh9";
            var password = "542gythnfsli8";
            var createDate = DateTime.UtcNow;
            var updateDate = DateTime.UtcNow;

            var user = new Repositories.Entities.User();

            var mockSet0 = new Mock<DbSet<Repositories.Entities.User>>();

            var data0 = new List<Repositories.Entities.User>().AsQueryable();

            mockSet0.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data0.Provider);
            mockSet0.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data0.Expression);
            mockSet0.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data0.ElementType);
            mockSet0.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data0.GetEnumerator());
            mockSet0.Setup(m => m.Add(It.IsAny<Repositories.Entities.User>()))
                .Callback((Repositories.Entities.User userParam) => { user = userParam; });

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id, UserName = userName, Email = email, Salt = salt, Password = password, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.User>()));

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.User>()).Returns(mockSet0.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.CreateUser(email, userName, password, salt);

            AuthDBContext.Verify(x => x.User, Times.Once);
            AuthDBContext.Verify(x => x.Set<Repositories.Entities.User>(), Times.Once);
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
            mockSet0.Verify(x => x.Add(It.IsAny<Repositories.Entities.User>()), Times.Once);
            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(user.UserName, userName);
            Assert.AreEqual(user.Salt, salt);
            Assert.AreEqual(user.Password, password);
        }

        [TestMethod]
        public void UpdateUser_UserExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email2 = "email2@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username2 = "name2";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt2 = "22ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password2 = "111gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate2 = DateTime.UtcNow.AddMinutes(-6);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var newEmail = "email5@mail.ru";
            var newUsername = "name5";

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id2, UserName = username2, Email = email2, Salt = salt2, Password = password2, CreatedDate = createDate2, UpdatedDate = updateDate2, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.User>()));

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.User>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.UpdateUser(id1, newEmail, newUsername);

            Assert.AreEqual(result.Email, newEmail);
            Assert.AreEqual(result.UserName, newUsername);
            Assert.AreEqual(result.Id, id1);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.Salt, salt1);
            Assert.AreEqual(result.Password, password1);
            Assert.IsTrue(result.UpdatedDate > updateDate1);
            AuthDBContext.Verify(x => x.User, Times.Exactly(2));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_UserNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var newEmail = "email5@mail.ru";
            var newUsername = "name5";

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.User>()));

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.User>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.UpdateUser(id2, newEmail, newUsername);

            Assert.AreEqual(result, null);
            AuthDBContext.Verify(x => x.User, Times.Exactly(1));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void UpdateActive_UserExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email2 = "email2@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username2 = "name2";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt2 = "22ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password2 = "111gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate2 = DateTime.UtcNow.AddMinutes(-6);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id2, UserName = username2, Email = email2, Salt = salt2, Password = password2, CreatedDate = createDate2, UpdatedDate = updateDate2, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.User>()));

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.User>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.UpdateActive(id1, false);

            Assert.IsFalse(result.IsActive);
            Assert.AreEqual(result.Email, email1);
            Assert.AreEqual(result.UserName, username1);
            Assert.AreEqual(result.Id, id1);
            Assert.AreEqual(result.Salt, salt1);
            Assert.AreEqual(result.Password, password1);
            Assert.IsTrue(result.UpdatedDate > updateDate1);
            AuthDBContext.Verify(x => x.User, Times.Exactly(2));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void UpdateActive_UserNotExisted_UseDbContextReturnNull()
        {
            AuthDBContext.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.User>()));

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.User>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.UpdateActive(id2, false);

            AuthDBContext.Verify(x => x.User, Times.Exactly(1));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Never);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void ChangePassword_UserExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt3 = "33ythgdh9";
            var password1 = "542gythnfsli8";
            var password3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var newSalt = "77ythgdh9";
            var newPassword = "659gythnfsli8";

            var data = new List<Repositories.Entities.User>
            {
                new Repositories.Entities.User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = password1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new Repositories.Entities.User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = password3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.User>>();

            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.User>()));

            AuthDBContext.Setup(x => x.User).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.User>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            var result = repository.ChangePassword(id2, newPassword, newSalt);

            Assert.IsFalse(result);
            AuthDBContext.Verify(x => x.User, Times.Exactly(1));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void UpdateUserRoles_UserRolesExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.UserRole>>();

            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var roleId1 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            var roleId3 = Guid.NewGuid();
            var roleId4 = Guid.NewGuid();
            var roleId5 = Guid.NewGuid();

            var addedRoles = new List<Repositories.Entities.UserRole>();
            var removedRoles = new List<Repositories.Entities.UserRole>();

            var data = new List<Repositories.Entities.UserRole>
            {
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId3 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId5 },
                new Repositories.Entities.UserRole { UserId = userId3, RoleId = roleId1 }

            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.UserRole>())).Callback((Repositories.Entities.UserRole role) => addedRoles.Add(role));
            mockSet.Setup(m => m.Remove(It.IsAny<Repositories.Entities.UserRole>())).Callback((Repositories.Entities.UserRole role) => removedRoles.Add(role));

            AuthDBContext.Setup(x => x.UserRole).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.UserRole>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            repository.UpdateRoles(userId1, new List<Guid> { roleId2, roleId5 });

            mockSet.Verify(x => x.Add(It.IsAny<Repositories.Entities.UserRole>()), Times.Exactly(1));
            mockSet.Verify(x => x.Remove(It.IsAny<Repositories.Entities.UserRole>()), Times.Exactly(2));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(addedRoles.Count(), 1);
            Assert.IsTrue(addedRoles.Select(t => t.RoleId).Contains(roleId5));
            Assert.AreEqual(removedRoles.Count(), 2);
            Assert.IsTrue(removedRoles.Select(t => t.RoleId).Contains(roleId3));
            Assert.IsTrue(removedRoles.Select(t => t.RoleId).Contains(roleId4));
        }

        [TestMethod]
        public void UpdateUserRoles_OnlyNewUserRoles_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.UserRole>>();

            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            var roleId3 = Guid.NewGuid();
            var roleId4 = Guid.NewGuid();
            var roleId5 = Guid.NewGuid();

            var addedRoles = new List<Repositories.Entities.UserRole>();
            var removedRoles = new List<Repositories.Entities.UserRole>();

            var data = new List<Repositories.Entities.UserRole>
            {
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId3 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId5 },
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.UserRole>())).Callback((Repositories.Entities.UserRole role) => addedRoles.Add(role));
            mockSet.Setup(m => m.Remove(It.IsAny<Repositories.Entities.UserRole>())).Callback((Repositories.Entities.UserRole role) => removedRoles.Add(role));

            AuthDBContext.Setup(x => x.UserRole).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.UserRole>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            repository.UpdateRoles(userId3, new List<Guid> { roleId2, roleId5 });

            mockSet.Verify(x => x.Add(It.IsAny<Repositories.Entities.UserRole>()), Times.Exactly(2));
            mockSet.Verify(x => x.Remove(It.IsAny<Repositories.Entities.UserRole>()), Times.Never);
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(addedRoles.Count(), 2);
            Assert.IsTrue(addedRoles.Select(t => t.RoleId).Contains(roleId2));
            Assert.IsTrue(addedRoles.Select(t => t.RoleId).Contains(roleId5));
            Assert.AreEqual(removedRoles.Count(), 0);
        }

        [TestMethod]
        public void UpdateUserRoles_OnlyDeletedUserRoles_UseDbContextReturnCorrect()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.UserRole>>();

            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            var roleId3 = Guid.NewGuid();
            var roleId4 = Guid.NewGuid();
            var roleId5 = Guid.NewGuid();

            var addedRoles = new List<Repositories.Entities.UserRole>();
            var removedRoles = new List<Repositories.Entities.UserRole>();

            var data = new List<Repositories.Entities.UserRole>
            {
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId3 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId5 },
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.UserRole>())).Callback((Repositories.Entities.UserRole role) => addedRoles.Add(role));
            mockSet.Setup(m => m.Remove(It.IsAny<Repositories.Entities.UserRole>())).Callback((Repositories.Entities.UserRole role) => removedRoles.Add(role));

            AuthDBContext.Setup(x => x.UserRole).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.UserRole>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            repository.UpdateRoles(userId2, new List<Guid> { roleId2, roleId5 });

            mockSet.Verify(x => x.Add(It.IsAny<Repositories.Entities.UserRole>()), Times.Never);
            mockSet.Verify(x => x.Remove(It.IsAny<Repositories.Entities.UserRole>()), Times.Once);
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(addedRoles.Count(), 0);
            Assert.AreEqual(removedRoles.Count(), 1);
            Assert.IsTrue(removedRoles.Select(t => t.RoleId).Contains(roleId4));
        }

        [TestMethod]
        public void UpdateUserRoles_SameUserRoles_NoChangesInBD()
        {
            AuthDBContext.ResetCalls();

            var mockSet = new Mock<DbSet<Repositories.Entities.UserRole>>();

            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            var roleId3 = Guid.NewGuid();
            var roleId4 = Guid.NewGuid();
            var roleId5 = Guid.NewGuid();

            var addedRoles = new List<Repositories.Entities.UserRole>();
            var removedRoles = new List<Repositories.Entities.UserRole>();

            var data = new List<Repositories.Entities.UserRole>
            {
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId3 },
                new Repositories.Entities.UserRole { UserId = userId1, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId2 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId4 },
                new Repositories.Entities.UserRole { UserId = userId2, RoleId = roleId5 }

            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.UserRole>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.UserRole>())).Callback((Repositories.Entities.UserRole role) => addedRoles.Add(role));
            mockSet.Setup(m => m.Remove(It.IsAny<Repositories.Entities.UserRole>())).Callback((Repositories.Entities.UserRole role) => removedRoles.Add(role));

            AuthDBContext.Setup(x => x.UserRole).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.UserRole>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new UserRepository(AuthDBContextFactory.Object);

            repository.UpdateRoles(userId1, new List<Guid> { roleId2, roleId3, roleId4 });

            mockSet.Verify(x => x.Add(It.IsAny<Repositories.Entities.UserRole>()), Times.Never);
            mockSet.Verify(x => x.Remove(It.IsAny<Repositories.Entities.UserRole>()), Times.Never);
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Never);
            Assert.AreEqual(addedRoles.Count(), 0);
            Assert.AreEqual(removedRoles.Count(), 0);
        }
    }
}
