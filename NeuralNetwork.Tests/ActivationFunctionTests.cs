using NUnit.Framework;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Tests
{
    public class ActivationFunctionTests
    {
        [TestCase(-0.1, ExpectedResult = 0)]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(0.1, ExpectedResult = 0.1)]
        [TestCase(0.5, ExpectedResult = 0.5)]
        [TestCase(0.9, ExpectedResult = 0.9)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(1.1, ExpectedResult = 1)]
        public double ActivationFunction_LineairTruncated_ExpectedResults(double value)
        {
            return ActivationFunctions.LineairTruncated(value);
        }

        [TestCase(-0.1, ExpectedResult = 0)]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(0.1, ExpectedResult = 0)]
        [TestCase(0.5, ExpectedResult = 1)]
        [TestCase(0.9, ExpectedResult = 1)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(1.1, ExpectedResult = 1)]
        public double ActivationFunction_Binairy_ExpectedResults(double value)
        {
            return ActivationFunctions.Binairy(value);
        }

        [TestCase(-10, ExpectedResult = 4.5397868702434395E-05d)]
        [TestCase(-1, ExpectedResult = 0.2689414213699951d)]
        [TestCase(-0.5, ExpectedResult = 0.37754066879814541d)]
        [TestCase(0, ExpectedResult = 0.5)]
        [TestCase(0.5, ExpectedResult = 0.62245933120185459d)]
        [TestCase(1, ExpectedResult = 0.7310585786300049d)]
        [TestCase(10, ExpectedResult = 0.99995460213129761d)]
        public double ActivationFunction_Sigmoid_ExpectedResults(double value)
        {
            return ActivationFunctions.Sigmoid(value);
        }

    }
}
