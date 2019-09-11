using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DotnetCommon.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetCommon.Tests
{
    [TestClass]
    public class ResultExstensionsTests
    {

        [TestMethod]
        public void Ensure_and_OnSuccess_alternately_ok()
        {
            var testObject = new TestObject(1)
            {
                Id = 0,
                Max10 = 9,
                MaxLength10 = "123456789",
            };

            var result = Result.Ok(testObject);

            var updatedResult = result
                .Ensure(x => x.CanUpdateStatus(), "Status is already max!")
                .OnSuccess(x => x.UpdateStatus())
                .Ensure(x => x.Id == 0, "Invalid Id")
                .OnSuccess(x => { x.Id = 1; })
                .Ensure(x => x.Max10 < 10 && x.MaxLength10.Length < 10, "Max fields are already max")
                .OnSuccess(x =>
                {
                    x.MaxLength10 += ("0");
                    x.Max10++;
                });

            Assert.IsTrue(updatedResult.IsSuccess);
            Assert.AreEqual(2, updatedResult.Value.StatusMax3);
            Assert.AreEqual(1, updatedResult.Value.Id);
            Assert.AreEqual(10, updatedResult.Value.Max10);
            Assert.AreEqual(10, updatedResult.Value.MaxLength10.Length);
        }

        [TestMethod]
        public void Ensure_and_OnSuccess_alternately_failed_on_first_ensure()
        {
            var testObject = new TestObject(3)
            {
                Id = 0,
                Max10 = 9,
                MaxLength10 = "123456789",
            };

            var result = Result.Ok(testObject);

            var updatedResult = result
                .Ensure(x => x.CanUpdateStatus(), "Status is already max!")
                .OnSuccess(x => x.UpdateStatus())
                .Ensure(x => x.Id == 0, "Invalid Id")
                .OnSuccess(x => { x.Id = 1; })
                .Ensure(x => x.Max10 < 10 && x.MaxLength10.Length < 10, "Max fields are already max")
                .OnSuccess(x =>
                {
                    x.MaxLength10 += ("0");
                    x.Max10++;
                });

            Assert.IsFalse(updatedResult.IsSuccess);
            //UpdatedResult don't contains Value after Failed Result
            Assert.ThrowsException<InvalidOperationException>(() => updatedResult.Value);
            //In this case there is no any changes to testObject
            Assert.AreEqual(3, testObject.StatusMax3); //remain the same
            Assert.AreEqual(0, testObject.Id); //remain the same
            Assert.AreEqual(9, testObject.Max10); //remain the same
            Assert.AreEqual(9, testObject.MaxLength10.Length); //remain the same
        }

        [TestMethod]
        public void Ensure_and_OnSuccess_alternately_failed_on_non_first_ensure()
        {
            var testObject = new TestObject(1)
            {
                Id = 0,
                Max10 = 10,
                MaxLength10 = "123456789",
            };

            var result = Result.Ok(testObject);

            var updatedResult = result
                .Ensure(x => x.CanUpdateStatus(), "Status is already max!")
                .OnSuccess(x => x.UpdateStatus())
                .Ensure(x => x.Id == 0, "Invalid Id")
                .OnSuccess(x => { x.Id = 1; })
                .Ensure(x => x.Max10 < 10 && x.MaxLength10.Length < 10, "Max fields are already max") //failed here
                .OnSuccess(x =>
                {
                    x.MaxLength10 += ("0");
                    x.Max10++;
                });

            Assert.IsFalse(updatedResult.IsSuccess);
            //UpdatedResult don't contains Value after Failed Result
            Assert.ThrowsException<InvalidOperationException>(() => updatedResult.Value);
            //Be careful, in this case changes are applied to testObject
            Assert.AreEqual(2, testObject.StatusMax3); //remain the same
            Assert.AreEqual(1, testObject.Id); //remain the same
            Assert.AreEqual(10, testObject.Max10); //remain the same
            Assert.AreEqual(9, testObject.MaxLength10.Length); //remain the same
        }

        [TestMethod]
        public void EnsureAll_then_OnSuccess_ok()
        {
            var testObject = new TestObject(1)
            {
                Id = 0,
                Max10 = 9,
                MaxLength10 = "123456789",
            };

            var result = Result.Ok(testObject);

            var updatedResult = result
                .Ensure(x => x.CanUpdateStatus(), "Status is already max!")
                .Ensure(x => x.Max10 < 10, "Field Max10 is at max value")
                .Ensure(x => x.MaxLength10.Length < 10, "Field MaxLength10 is at max length")
                .Ensure(x => x.Id == 0, "Id is invalid")
                .OnSuccess(x =>
                {
                    x.UpdateStatus();
                    x.Id = 1;
                    x.Max10++;
                    x.MaxLength10 += "0";
                });

            Assert.IsTrue(updatedResult.IsSuccess);
            Assert.AreEqual(2, updatedResult.Value.StatusMax3);
            Assert.AreEqual(1, updatedResult.Value.Id);
            Assert.AreEqual(10, updatedResult.Value.Max10);
            Assert.AreEqual(10, updatedResult.Value.MaxLength10.Length);
        }

        [TestMethod]
        public void EnsureAll_then_OnSuccess_failed()
        {
            var testObject = new TestObject(3)
            {
                Id = 0,
                Max10 = 9,
                MaxLength10 = "123456789",
            };

            var result = Result.Ok(testObject);

            var updatedResult = result
                .Ensure(x => x.CanUpdateStatus(), "Status is already max!")
                .Ensure(x => x.Max10 < 10, "Field Max10 is at max value")
                .Ensure(x => x.MaxLength10.Length < 10, "Field MaxLength10 is at max length")
                .Ensure(x => x.Id == 0, "Id is invalid")
                .OnSuccess(x =>
                {
                    x.UpdateStatus();
                    x.Id = 1;
                    x.Max10++;
                    x.MaxLength10 += "0";
                });

            Assert.IsFalse(updatedResult.IsSuccess);
            Assert.AreEqual("Status is already max!", updatedResult.Message);
            //UpdatedResult don't contains Value after Failed Result
            Assert.ThrowsException<InvalidOperationException>(() => updatedResult.Value);
            //There is no any changes to testObject
            Assert.AreEqual(3, testObject.StatusMax3); //remain the same
            Assert.AreEqual(0, testObject.Id); //remain the same
            Assert.AreEqual(9, testObject.Max10); //remain the same
            Assert.AreEqual(9, testObject.MaxLength10.Length); //remain the same
        }
    }
}

