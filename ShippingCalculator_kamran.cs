// Express Shipping Rate Calculator
// Developer: Daniel Chen
// Date: March 2024
using System;

namespace ShippingExpress.Strategy
{
    // Strategy interface for validation
    public interface IValidationStrategy
    {
        bool Validate(double value);
        string GetErrorMessage();
    }

    // Weight validation strategy
    public class WeightValidationStrategy : IValidationStrategy
    {
        public bool Validate(double weight) => weight <= 50;
        public string GetErrorMessage() => "Package too heavy to be shipped via Package Express. Have a good day.";
    }

    // Size validation strategy
    public class SizeValidationStrategy : IValidationStrategy
    {
        public bool Validate(double size) => size <= 50;
        public string GetErrorMessage() => "Package too big to be shipped via Package Express.";
    }

    // Strategy interface for shipping cost calculation
    public interface IShippingCostStrategy
    {
        double Calculate(double width, double height, double length, double weight);
    }

    // Standard shipping cost strategy
    public class StandardShippingCostStrategy : IShippingCostStrategy
    {
        public double Calculate(double width, double height, double length, double weight)
        {
            return (width * height * length * weight) / 100;
        }
    }

    // Package processor
    public class PackageProcessor
    {
        private readonly IValidationStrategy _weightValidator;
        private readonly IValidationStrategy _sizeValidator;
        private readonly IShippingCostStrategy _costCalculator;

        public PackageProcessor(
            IValidationStrategy weightValidator,
            IValidationStrategy sizeValidator,
            IShippingCostStrategy costCalculator)
        {
            _weightValidator = weightValidator;
            _sizeValidator = sizeValidator;
            _costCalculator = costCalculator;
        }

        public bool ValidateWeight(double weight)
        {
            if (!_weightValidator.Validate(weight))
            {
                Console.WriteLine(_weightValidator.GetErrorMessage());
                return false;
            }
            return true;
        }

        public bool ValidateSize(double totalSize)
        {
            if (!_sizeValidator.Validate(totalSize))
            {
                Console.WriteLine(_sizeValidator.GetErrorMessage());
                return false;
            }
            return true;
        }

        public double CalculateShippingCost(double width, double height, double length, double weight)
        {
            return _costCalculator.Calculate(width, height, length, weight);
        }
    }

    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            // Show program introduction
            Console.WriteLine("Welcome to Package Express. Please follow the instructions below.");

            try
            {
                var processor = new PackageProcessor(
                    new WeightValidationStrategy(),
                    new SizeValidationStrategy(),
                    new StandardShippingCostStrategy()
                );

                // Get package weight
                Console.WriteLine("Please enter the package weight:");
                if (!double.TryParse(Console.ReadLine(), out double weightValue))
                {
                    Console.WriteLine("Invalid weight input.");
                    return;
                }

                // Validate weight
                if (!processor.ValidateWeight(weightValue))
                    return;

                // Get package dimensions
                Console.WriteLine("Please enter the package width:");
                if (!double.TryParse(Console.ReadLine(), out double dimW))
                {
                    Console.WriteLine("Invalid width input.");
                    return;
                }

                Console.WriteLine("Please enter the package height:");
                if (!double.TryParse(Console.ReadLine(), out double dimH))
                {
                    Console.WriteLine("Invalid height input.");
                    return;
                }

                Console.WriteLine("Please enter the package length:");
                if (!double.TryParse(Console.ReadLine(), out double dimL))
                {
                    Console.WriteLine("Invalid length input.");
                    return;
                }

                // Validate total size
                double totalDims = dimW + dimH + dimL;
                if (!processor.ValidateSize(totalDims))
                    return;

                // Calculate and show shipping cost
                double finalCost = processor.CalculateShippingCost(dimW, dimH, dimL, weightValue);
                Console.WriteLine($"Your estimated total for shipping this package is: ${finalCost:F2}");
                Console.WriteLine("Thank you!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
} 