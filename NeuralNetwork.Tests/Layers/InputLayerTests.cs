using Apocalibs.ArtificialIntelligence.NeuralNetwork.Layers;
using NUnit.Framework;
using System.Linq;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Tests.Layers
{
    class InputLayerTests
    {
        [Test]
        public void InputLayer_Constructor_ExactNumberOfNeurons()
        {
            var layer = new InputLayer(10);

            Assert.AreEqual(10, layer.Neurons.Count());
        }

        [Test]
        public void InputLayer_Constructor_NoActivationFunction()
        {
            var layer = new InputLayer(10);

            Assert.IsTrue(layer.Neurons.All(n => n.ActivationFunction == null));
        }

    }
}
