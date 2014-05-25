using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ServiceUnderTest.Model;
using ServiceUnderTest.Services;

#region Model
namespace ServiceUnderTest.Model
{
    public class Input
    {
        public List<int> Operands { get; set; }

        public String AssertPresence { get; set; }
        public bool HasToBeTrue { get; set; }

        public String NotRelevantForThisTest01 { get; set; }
        public String NotRelevantForThisTest02 { get; set; }
    }

    public class Output
    {
        public int Sum { get; set; }
        public int Product { get; set; }
    }
}
#endregion

#region Services
namespace ServiceUnderTest.Services
{
    public interface IAdditionService
    {
        int Sum(IEnumerable<int> operands);
    }

    public class AdditionService : IAdditionService
    {
        public int Sum(IEnumerable<int> operands)
        {
            return operands.Aggregate(0, (ints, i) => ints + i);
        }
    }


    public interface IMultiplicationService
    {
        int Product(List<int> operands);
    }

    public class MultiplicationService : IMultiplicationService
    {
        public int Product(List<int> operands)
        {
            return operands.Aggregate(1, (ints, i) => ints*i);
        }
    }
}
#endregion

#region DependencyInjection
namespace ServiceUnderTest.DependencyInjection
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<AdditionService>().As<IAdditionService>();
            builder.RegisterType<MultiplicationService>().As<IMultiplicationService>();
            builder.RegisterType<BusinessService>().As<IBusinessService>();
        }
    }
}
#endregion

#region ServiceUnderTest
namespace ServiceUnderTest
{
    public interface IBusinessService
    {
        Output ProcessInput(Input input);
    }

    public class BusinessService : IBusinessService
    {
        private readonly IAdditionService _additionService;
        private readonly IMultiplicationService _multiplicationService;

        public BusinessService(IAdditionService additionService, IMultiplicationService multiplicationService)
        {
            _additionService = additionService;
            _multiplicationService = multiplicationService;
        }

        public Output ProcessInput(Input input)
        {
            if (String.IsNullOrEmpty(input.AssertPresence))
                throw new ArgumentException("input.AssertPresence might not be null or Empty.");

            if (!input.HasToBeTrue)
                throw new ArgumentException("input.HasToBeTrue has to be true.");

            var result = new Output
            {
                Sum = _additionService.Sum(input.Operands),
                Product = _multiplicationService.Product(input.Operands)
            };

            return result;
        }
    }
}
#endregion
