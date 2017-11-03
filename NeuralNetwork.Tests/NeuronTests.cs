using Apocalibs.Core.Types;
using NUnit.Framework;
using System;
using System.Linq;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Tests
{
    class NeuronTests
    {
        [Test]
        public void Neuron_Constructor_PropertiesSet()
        {
            var neuron = new Neuron(ActivationFunctions.Binairy, 12.34);

            Assert.AreEqual(ActivationFunctions.Binairy, neuron.ActivationFunction);
            Assert.AreEqual(12.34, neuron.Bias);
        }

        [TestCase(0)]
        [TestCase(0.5)]
        [TestCase(1)]
        public void Neuron_SetValue_ExpectedGetValue(double value)
        {
            var neuron = new Neuron(ActivationFunctions.Binairy, 12.34);

            neuron.SetValue(value);

            Assert.AreEqual(new OneRange(value), neuron.GetValue());
        }

        [Test]
        public void Neuron_GetValue_NoInputSynapses_NoException()
        {
            var neuron = new Neuron(ActivationFunctions.Binairy, 1);

            Assert.AreEqual(new OneRange(1), neuron.GetValue());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Neuron_GetValue_NoActivationFunctionAndNoValue_Exception()
        {
            var neuron = new Neuron(null, 1);

            neuron.GetValue();
        }

        [Test]
        public void Neuron_GetValue_NoActivationFunctionAndSetValue_NoException()
        {
            var neuron = new Neuron(null, 1);

            neuron.SetValue(0.5);
            Assert.AreEqual(new OneRange(0.5), neuron.GetValue());
        }

        [TestCase(0)]
        [TestCase(0.5)]
        [TestCase(1)]
        public void Neuron_SetValue_GetValueWithNextNeuron(double value)
        {
            var inputNeuron = new Neuron(null, 1);
            var outputNeuron = new Neuron(ActivationFunctions.LineairTruncated, 0);

            inputNeuron.AddSynapse(outputNeuron, 1);

            inputNeuron.SetValue(value);
            Assert.AreEqual(new OneRange(value), outputNeuron.GetValue());
        }

        [TestCase(0)]
        [TestCase(0.5)]
        [TestCase(1)]
        public void Neuron_SetValue_GetValueWithNextNeuron_WithWeightedSynapse(double value)
        {
            const double SYNAPSE_WEIGHT = 0.5;

            var inputNeuron = new Neuron(null, 1);
            var outputNeuron = new Neuron(ActivationFunctions.LineairTruncated, 0);

            inputNeuron.AddSynapse(outputNeuron, SYNAPSE_WEIGHT);

            inputNeuron.SetValue(value);
            Assert.AreEqual(new OneRange(value * SYNAPSE_WEIGHT), outputNeuron.GetValue());
        }

        [TestCase(0, ExpectedResult = 0.5)]
        [TestCase(0.5, ExpectedResult = 1)]
        [TestCase(1, ExpectedResult = 1)]
        public double Neuron_SetValue_GetValueWithNextNeuron_WithPositiveBias(double value)
        {
            const double BIAS = 0.5;

            var inputNeuron = new Neuron(null, 1);
            var outputNeuron = new Neuron(ActivationFunctions.LineairTruncated, BIAS);

            inputNeuron.AddSynapse(outputNeuron, 1);

            inputNeuron.SetValue(value);
            return outputNeuron.GetValue();
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(0.5, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 0.5)]
        public double Neuron_SetValue_GetValueWithNextNeuron_WithNegativeBias(double value)
        {
            const double BIAS = -0.5;

            var inputNeuron = new Neuron(null, 1);
            var outputNeuron = new Neuron(ActivationFunctions.LineairTruncated, BIAS);

            inputNeuron.AddSynapse(outputNeuron, 1);

            inputNeuron.SetValue(value);
            return outputNeuron.GetValue();
        }

        [Test]
        public void Neuron_RemoveSynapse_AllConnectionsRemoved_ValuesCorrect()
        {
            var inputNeuron = new Neuron(null, 1);
            var outputNeuron = new Neuron(ActivationFunctions.LineairTruncated, 0);

            inputNeuron.AddSynapse(outputNeuron, 1);

            Assert.IsEmpty(inputNeuron.InputSynapses);
            Assert.AreEqual(1, inputNeuron.OutputSynapses.Count());
            Assert.AreEqual(1, outputNeuron.InputSynapses.Count());
            Assert.IsEmpty(outputNeuron.OutputSynapses);

            inputNeuron.SetValue(1);
            Assert.AreEqual(new OneRange(1), outputNeuron.GetValue());

            inputNeuron.RemoveSynapse(outputNeuron);

            Assert.IsEmpty(inputNeuron.InputSynapses);
            Assert.IsEmpty(inputNeuron.OutputSynapses);
            Assert.IsEmpty(outputNeuron.InputSynapses);
            Assert.IsEmpty(outputNeuron.OutputSynapses);

            Assert.AreEqual(new OneRange(0), outputNeuron.GetValue());
            Assert.AreEqual(new OneRange(1), inputNeuron.GetValue());
        }
    }
}
