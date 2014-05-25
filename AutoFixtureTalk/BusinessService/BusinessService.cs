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
        public DontBeNull DontBeNull { get; set; }

        public String NotRelevantForThisTest01 { get; set; }
        public String NotRelevantForThisTest02 { get; set; }
    }

    public class Output
    {
        public int Sum { get; set; }
        public int Product { get; set; }
    }

    public class DontBeNull { }
}
#endregion

#region Services
namespace ServiceUnderTest.Services
{
    public interface IAdditionService
    {
        int Add(IEnumerable<int> operands);
    }

    public class AdditionService : IAdditionService
    {
        public int Add(IEnumerable<int> operands)
        {
            return operands.Aggregate(0, (ints, i) => ints + i);
        }
    }


    public interface IMultiplicationService
    {
        int Multiply(IEnumerable<int> operands);
    }

    public class MultiplicationService : IMultiplicationService
    {
        public int Multiply(IEnumerable<int> operands)
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

            if (input.DontBeNull == null)
                throw new ArgumentException("input.DontBeNull has to be set.");

            var result = new Output
            {
                Sum = _additionService.Add(input.Operands),
                Product = _multiplicationService.Multiply(input.Operands)
            };

            return result;
        }
    }
}
#endregion
