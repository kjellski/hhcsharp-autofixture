using System.Collections.Generic;
using BusinessService;
using BusinessService.Model;
using BusinessService.Services;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace BusinessServiceTests
{
    [TestFixture]
    public class BusinessServiceWithAutofixtureAndAutoMoqTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        private IFixture _fixture;

        [Test]
        public void BusinessServiceTest()
        {
            // arrange
            // act
            // assert
            CalculationService sut = null;
            Assert.DoesNotThrow(() => sut = _fixture.Create<CalculationService>());
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void ProcessInputs_AdditionService_GetsCalled_Test()
        {
            // arrange
            var additionServiceMock = _fixture.Freeze<Mock<IAdditionService>>();
            var sut = _fixture.Create<CalculationService>();

            // act
            var result = sut.ProcessInput(_fixture.Create<Input>()); // not even interested in result!

            // assert
            additionServiceMock.Verify(m => m.Add(It.IsAny<IEnumerable<int>>()), Times.Once());
        }

        [Test]
        public void ProcessInputs_MultiplicationService_GetsCalled_Test()
        {
            // arrange
            var multiplicationServiceMock = _fixture.Freeze<Mock<IMultiplicationService>>();
            var sut = _fixture.Create<CalculationService>();

            // act
            var result = sut.ProcessInput(_fixture.Create<Input>()); // not even interested in result!

            // assert
            multiplicationServiceMock.Verify(m => m.Multiply(It.IsAny<IEnumerable<int>>()), Times.Once());
        }
    }
}