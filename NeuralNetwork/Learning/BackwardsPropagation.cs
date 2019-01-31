using Apocalibs.Core.Types;
using Apocalibs.Extensions.Enumerables;
using System.Collections.Generic;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Learning
{
    internal interface INeuralNetworkData
    {
        Scalar GetInputValue(int inputNeuron);

        Scalar GetExpectedOutputValue(int outputNeuron);
    }

    internal struct NeuralNetworkResult
    {
        public int OutputNeuron { get; set; }
        public Scalar ActualValue { get; set; }
        public Scalar ExpectedValue { get; set; }
    }

    internal class BackwardsPropagation
    {
        private readonly NeuralNetwork _neuralNetwork;

        public BackwardsPropagation(NeuralNetwork neuralNetwork)
        {
            _neuralNetwork = neuralNetwork;
        }

        public void Process(IEnumerable<INeuralNetworkData> dataList)
        {
            var results = new Dictionary<INeuralNetworkData, List<NeuralNetworkResult>>();

            foreach (var data in dataList)
            {
                results[data] = new List<NeuralNetworkResult>();

                foreach (var neuron in _neuralNetwork.InputLayer.Neurons.EnumerateWithIndex())
                {
                    neuron.Item.SetValue(data.GetInputValue(neuron.Index));
                }

                foreach (var neuron in _neuralNetwork.OutputLayer.Neurons.EnumerateWithIndex())
                {
                    results[data].Add(new NeuralNetworkResult()
                    {
                        OutputNeuron = neuron.Index,
                        ActualValue = neuron.Item.GetValue(),
                        ExpectedValue = data.GetExpectedOutputValue(neuron.Index)
                    });
                }
            }

            ///////

        }
    }
}
