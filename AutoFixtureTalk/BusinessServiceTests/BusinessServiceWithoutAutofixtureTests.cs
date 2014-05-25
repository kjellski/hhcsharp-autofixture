﻿using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using ServiceUnderTest;
using ServiceUnderTest.Model;
using ServiceUnderTest.Services;

namespace BusinessServiceTests
{
    [TestFixture]
    public class BusinessServiceWithoutAutofixtureTests
    {
        [SetUp]
        public void SetUp()
        {
            _additionServiceMock = new Mock<IAdditionService>();
            _multiplicationServiceMock = new Mock<IMultiplicationService>();
        }

        private Mock<IAdditionService> _additionServiceMock;
        private Mock<IMultiplicationService> _multiplicationServiceMock;

        [Test]
        public void BusinessServiceTest()
        {
            // arrange
            // act
            // assert
            BusinessService sut = null;
            Assert.DoesNotThrow(
                () => sut = new BusinessService(_additionServiceMock.Object, _multiplicationServiceMock.Object));
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void ProcessInputs_AdditionService_GetsCalled_Test()
        {
            // arrange
            var input = new Input
            {
                Operands = new List<int> {1, 2, 3, 4, 5},
                AssertPresence = "Just not empty, but totally unrelated to test!",
                DontBeNull = new DontBeNull() // same here
            };

            // could go into setup, but not when you need to setup the mocked dependencies in any special way
            var sut = new BusinessService(_additionServiceMock.Object, _multiplicationServiceMock.Object);

            // act
            var result = sut.ProcessInput(input); // not even interested in result!

            // assert
            _additionServiceMock.Verify(m => m.Add(It.IsAny<IEnumerable<int>>()), Times.Once());
        }

        [Test]
        public void ProcessInputs_MultiplicationService_GetsCalled_Test()
        {
            // arrange
            var input = new Input
            {
                Operands = new List<int> {1, 2, 3, 4, 5},
                AssertPresence = "Just not empty, but totally unrelated to test!",
                DontBeNull = new DontBeNull() // same here
            };

            // could go into setup, but not when you need to setup the mocked dependencies in any special way
            var sut = new BusinessService(_additionServiceMock.Object, _multiplicationServiceMock.Object);

            // act
            var result = sut.ProcessInput(input); // not even interested in result!

            // assert
            _multiplicationServiceMock.Verify(m => m.Multiply(It.IsAny<IEnumerable<int>>()), Times.Once());
        }
    }
}