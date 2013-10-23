using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace NerdDinner.Tests.Controllers
{
    [TestFixture]
    public class Temp
    {
        public interface IMyInterface
        {
            string GetValue();
        }

        public class MyClass
        {
            private readonly IMyInterface _myInterface;

            public MyClass(IMyInterface myInterface)
            {
                _myInterface = myInterface;
            }

            public string MyMehod()
            {
                var initialValue = _myInterface.GetValue();
                switch (initialValue)
                {
                    case "Fred":
                        return "Bob";

                    case "Tom":
                        return "Jerry";

                    case                         "Fizz":
                        return "Binn";

                    case "Abbot":
                        return "Costello";

                    default:
                        return null;
                }
            }
        }


        [Test]
        [TestCase("Fred", "Bob")]
        [TestCase("Fizz", "Binn")]
        [TestCase("Tom", "Jerry")]
        [TestCase("Abbot", "Costello")]
        public void Test(string value, string expected)
        {
            var myInterface = Substitute.For<IMyInterface>();
            myInterface.GetValue().Returns(value);

            var cls = new MyClass(myInterface);

            // Act
            var result = cls.MyMehod();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

    }
}
