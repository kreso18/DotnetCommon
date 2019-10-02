using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DotnetCommon.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotnetCommon.Tests
{
    [TestClass]
    public class MaybeExtensionsTests
    {

        [TestMethod]
        public void ToResult()
        {
            Maybe<string> stringOrNothingHaveValue = "test"; //new Maybe<string>("test");
            Maybe<string> stringOrNothingIsNull = null;

            var resultValue = stringOrNothingHaveValue.ToResult("String is nothing");
            var resultNull = stringOrNothingIsNull.ToResult("String is nothing");

            Assert.IsTrue(resultValue.IsSuccess);
            Assert.AreEqual("test", resultValue.Value);

            Assert.IsTrue(resultNull.IsFailure);
            Assert.AreEqual("String is nothing", resultNull.Message);
        }
    }
}

