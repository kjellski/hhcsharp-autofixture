using System;
using System.Reflection;
using BusinessService;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace BusinessServiceTests
{
    public class IgnoreVirtualMembersSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var pi = request as PropertyInfo;
            if (pi == null)
                return new NoSpecimen(request);

            // setting virtual member to null
            return !pi.GetGetMethod().IsVirtual ? 
                new NoSpecimen(request) : 
                null; 
        }
    }

    public class SetVirtualMembersToNullCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            // add the ignoring builder at last, this will be the fallback then...
            fixture.Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder());
        }
    }

    [TestFixture]
    public class ExtendingAutoFixtureTests
    {
        [Test]
        public void CreateItemWithFixture()
        {
            // arrange
            var f = new Fixture();
            var sut = new SomeService();

            // act 
            sut.DoStuff(f.Create<Item>());

            // assert
        }
    }
}
