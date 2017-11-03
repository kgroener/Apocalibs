using Apocalibs.ArtificialIntelligence.NeuralNetwork.Layers;
using NUnit.Framework;
using System.Linq;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Tests.Layers
{
    class OutputLayerTests
    {
        [Test]
        public void OutputLayer_Constructor_ExactNumberOfNeurons()
        {
            var layer = new OutputLayer(10, (i) => ActivationFunctions.Binairy, (i) => 12.34);

            Assert.AreEqual(10, layer.Neurons.Count());
        }

        [Test]
        public void OutputLayer_Constructor_ActivationFunctionSelector()
        {
            var layer = new OutputLayer(10, (i) => ActivationFunctions.Binairy, (i) => 12.34);

            Assert.IsTrue(layer.Neurons.All(n => n.ActivationFunction == ActivationFunctions.Binairy));
        }

        [Test]
        public void OutputLayer_Constructor_BiasSelector()
        {
            var layer = new OutputLayer(10, (i) => ActivationFunctions.Binairy, (i) => 12.34);

            Assert.IsTrue(layer.Neurons.All(n => n.Bias == 12.34));
        }
    }
}
