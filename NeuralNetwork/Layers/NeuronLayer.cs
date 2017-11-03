using System;
using System.Collections.Generic;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Layers
{
    internal abstract class NeuronLayer
    {
        private List<Neuron> _neurons;

        public NeuronLayer(int numberOfNeurons, Func<int, ActivationFunction> activationFunctionSelector, Func<int, double> biasSelector)
        {
            _neurons = new List<Neuron>();

            for (int i = 0; i < numberOfNeurons; i++)
            {
                AddNeuron(activationFunctionSelector(i), biasSelector(i));
            }
        }

        protected Neuron AddNeuron(ActivationFunction activation, double bias)
        {
            Neuron newNeuron = new Neuron(activation, bias);
            _neurons.Add(newNeuron);
            return newNeuron;
        }

        public IEnumerable<Neuron> Neurons => _neurons;
    }
}
