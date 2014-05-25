using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BusinessService.Model;
using BusinessService.Services;

#region Model
namespace BusinessService.Model
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
namespace BusinessService.Services
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
namespace BusinessService.DependencyInjection
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<AdditionService>().As<IAdditionService>();
            builder.RegisterType<MultiplicationService>().As<IMultiplicationService>();
            builder.RegisterType<CalculationService>().As<ICalculationService>();
        }
    }
}
#endregion

#region ServiceUnderTest
namespace BusinessService
{
    public interface ICalculationService
    {
        Output ProcessInput(Input input);
    }

    public class CalculationService : ICalculationService
    {
        private readonly IAdditionService _additionService;
        private readonly IMultiplicationService _multiplicationService;

        public CalculationService(IAdditionService additionService, IMultiplicationService multiplicationService)
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
