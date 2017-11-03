using System;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Layers
{
    internal class HiddenLayer : NeuronLayer
    {
        public HiddenLayer(int numberOfNeurons, Func<int, ActivationFunction> activationFunctionSelector, Func<int, double> biasSelector) : base(numberOfNeurons, activationFunctionSelector, biasSelector)
        {
            var constantNeuron = AddNeuron(null, 0);
            constantNeuron.SetValue(1);
        }
    }
}
