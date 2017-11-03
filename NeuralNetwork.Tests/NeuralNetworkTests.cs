using NUnit.Framework;
using System.Linq;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Tests
{
    class NeuralNetworkTests
    {

        [Test]
        public void NeuralNetwork_HiddenLayerNeuronCountConstructor_CorrectLayers()
        {
            var neuralNetwork = new NeuralNetwork(5, 3, 2, 4);

            Assert.AreEqual(5, neuralNetwork.InputLayer.Neurons.Count());
            Assert.AreEqual(3, neuralNetwork.OutputLayer.Neurons.Count());
            Assert.AreEqual(2, neuralNetwork.HiddenLayers.Count());
            Assert.IsTrue(neuralNetwork.HiddenLayers.All(l => l.Neurons.Count() == 5)); // 4 + 1 constant neuron
        }
    }
}
