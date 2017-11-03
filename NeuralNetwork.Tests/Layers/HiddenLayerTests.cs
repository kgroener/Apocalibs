
using Apocalibs.ArtificialIntelligence.NeuralNetwork.Layers;
using Apocalibs.Core.Types;
using Apocalibs.Extensions.Enumerables;
using NUnit.Framework;
using System.Linq;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Tests.Layers

{
    public class HiddenLayerTests
    {
        [Test]
        public void HiddenLayer_Constructor_ConstantNeuronAdded()
        {
            var hiddenLayer = new HiddenLayer(10, (i) => ActivationFunctions.Binairy, (i) => 12.34);

            Assert.AreEqual(11, hiddenLayer.Neurons.Count());

            var constantNeuron = hiddenLayer.Neurons.Single(n => n.ActivationFunction == null);
            Assert.AreEqual(new OneRange(1), constantNeuron.GetValue());
            Assert.AreEqual(0, constantNeuron.Bias);
        }

        [Test]
        public void HiddenLayer_Constructor_ActivationFunctionSelector()
        {
            var hiddenLayer = new HiddenLayer(10, (i) => ActivationFunctions.Binairy, (i) => 12.34);

            var constantNeuron = hiddenLayer.Neurons.Single(n => n.ActivationFunction == null);

            Assert.IsTrue(hiddenLayer.Neurons.Except(constantNeuron).All(n => n.ActivationFunction == ActivationFunctions.Binairy));
        }

        [Test]
        public void HiddenLayer_Constructor_BiasSelector()
        {
            var hiddenLayer = new HiddenLayer(10, (i) => ActivationFunctions.Binairy, (i) => 12.34);

            var constantNeuron = hiddenLayer.Neurons.Single(n => n.ActivationFunction == null);

            Assert.IsTrue(hiddenLayer.Neurons.Except(constantNeuron).All(n => n.Bias == 12.34));
        }
    }
}
