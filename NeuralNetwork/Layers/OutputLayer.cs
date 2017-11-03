using System;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Layers
{
    internal class OutputLayer : NeuronLayer
    {
        public OutputLayer(int numberOfNeurons, Func<int, ActivationFunction> activationFunctionSelector, Func<int, double> biasSelector) : base(numberOfNeurons, activationFunctionSelector, biasSelector)
        {

        }
    }
}
