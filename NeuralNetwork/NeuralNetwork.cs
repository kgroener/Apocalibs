using Apocalibs.ArtificialIntelligence.NeuralNetwork.Layers;
using Apocalibs.Extensions.Enumerables;
using System.Collections.Generic;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork
{
    public class NeuralNetwork
    {

        public delegate double SynapseWeightSelector(int layerFrom, int neuronFrom, int layerTo, int neuronTo);
        public delegate ActivationFunction ActivationFunctionSelector(int layer, int neuron);
        public delegate double BiasSelector(int layer, int neuron);
        public delegate int LayerNeuronCountSelector(int layer);

        List<HiddenLayer> _hiddenLayers;
        private readonly InputLayer _inputLayer;
        private readonly OutputLayer _outputLayer;

        internal IEnumerable<HiddenLayer> HiddenLayers => _hiddenLayers;
        internal InputLayer InputLayer => _inputLayer;
        internal OutputLayer OutputLayer => _outputLayer;

        /// <summary>
        /// Creates a neural network with <paramref name="inputCount"/> number of inputs,
        /// <paramref name="outputCount"/> number of outputs,
        /// <paramref name="hiddenLayersCount"/> number of hidden layers, 
        /// where every hidden layer consists of <paramref name="hiddenLayerNeuronCount"/> number of neurons.
        /// Each neuron in the hidden layer uses the default activation function (Sigmoid), default bias (0) and synapse weights are initialized to 0.5.
        /// </summary>
        /// <param name="inputCount">The ammount of inputs</param>
        /// <param name="outputCount">The ammount of outputs</param>
        /// <param name="hiddenLayersCount">The ammount of hidden layers</param>
        /// <param name="hiddenLayerNeuronCount">The number of neurons in each hidden layer</param>
        public NeuralNetwork(int inputCount, int outputCount, int hiddenLayersCount, int hiddenLayerNeuronCount) : this(inputCount, outputCount, hiddenLayersCount, (l) => hiddenLayerNeuronCount) { }

        /// <summary>
        /// Creates a neural network with <paramref name="inputCount"/> number of inputs,
        /// <paramref name="outputCount"/> number of outputs,
        /// <paramref name="hiddenLayersCount"/> number of hidden layers, 
        /// where the number of neurons in each hidden layer can be selected using the <paramref name="hiddenLayerNeuronCountSelector"/>.
        /// Each neuron in the hidden layer uses the default activation function (Sigmoid), default bias (0) and synapse weights are initialized to 0.5.
        /// </summary>
        /// <param name="inputCount">The ammount of inputs</param>
        /// <param name="outputCount">The ammount of outputs</param>
        /// <param name="hiddenLayersCount">The ammount of hidden layers</param>
        /// <param name="hiddenLayerNeuronCountSelector">Selects the ammount of neurons in the given hidden layer index</param>
        public NeuralNetwork(int inputCount, int outputCount, int hiddenLayersCount, LayerNeuronCountSelector hiddenLayerNeuronCountSelector) : this(inputCount, outputCount, hiddenLayersCount, hiddenLayerNeuronCountSelector, (l, n) => ActivationFunctions.Sigmoid) { }

        /// <summary>
        /// Creates a neural network with <paramref name="inputCount"/> number of inputs,
        /// <paramref name="outputCount"/> number of outputs,
        /// <paramref name="hiddenLayersCount"/> number of hidden layers, 
        /// where the number of neurons in each hidden layer can be selected using the <paramref name="hiddenLayerNeuronCountSelector"/>
        /// The <paramref name="activationFunctionSelector"/> allows to select a different activation function used for each neuron in each hidden layer.
        /// Each neuron in the hidden layer uses the default bias (0) and synapse weights are initialized to 0.5.
        /// </summary>
        /// <param name="inputCount">The ammount of inputs</param>
        /// <param name="outputCount">The ammount of outputs</param>
        /// <param name="hiddenLayersCount">The ammount of hidden layers</param>
        /// <param name="hiddenLayerNeuronCountSelector">Selects the ammount of neurons in the given hidden layer index</param>
        /// <param name="activationFunctionSelector">Selects an activation function for each neuron in each hidden layer</param>
        public NeuralNetwork(int inputCount, int outputCount, int hiddenLayersCount, LayerNeuronCountSelector hiddenLayerNeuronCountSelector, ActivationFunctionSelector activationFunctionSelector) : this(inputCount, outputCount, hiddenLayersCount, hiddenLayerNeuronCountSelector, activationFunctionSelector, (l, n) => 0) { }

        /// <summary>
        /// Creates a neural network with <paramref name="inputCount"/> number of inputs,
        /// <paramref name="outputCount"/> number of outputs,
        /// <paramref name="hiddenLayersCount"/> number of hidden layers, 
        /// where the number of neurons in each hidden layer can be selected using the <paramref name="hiddenLayerNeuronCountSelector"/>
        /// The <paramref name="activationFunctionSelector"/> allows to select a different activation function used for each neuron in each hidden layer.
        /// The <paramref name="biasSelector"/> allows to select a different bias for each neuron in each hidden layer.
        /// Each synapse weight is initialized to 0.5.
        /// </summary>
        /// <param name="inputCount">The ammount of inputs</param>
        /// <param name="outputCount">The ammount of outputs</param>
        /// <param name="hiddenLayersCount">The ammount of hidden layers</param>
        /// <param name="hiddenLayerNeuronCountSelector">Selects the ammount of neurons in the given hidden layer index</param>
        /// <param name="activationFunctionSelector">Selects an activation function for each neuron in each hidden layer</param>
        /// <param name="biasSelector">Selects a bias for each neuron in each hidden layer</param>
        public NeuralNetwork(int inputCount, int outputCount, int hiddenLayersCount, LayerNeuronCountSelector hiddenLayerNeuronCountSelector, ActivationFunctionSelector activationFunctionSelector, BiasSelector biasSelector) : this(inputCount, outputCount, hiddenLayersCount, hiddenLayerNeuronCountSelector, activationFunctionSelector, biasSelector, (l1, n1, l2, n2) => 0.5) { }

        /// <summary>
        /// Creates a neural network with <paramref name="inputCount"/> number of inputs,
        /// <paramref name="outputCount"/> number of outputs,
        /// <paramref name="hiddenLayersCount"/> number of hidden layers, 
        /// where the number of neurons in each hidden layer can be selected using the <paramref name="hiddenLayerNeuronCountSelector"/>
        /// The <paramref name="activationFunctionSelector"/> allows to select a different activation function used for each neuron in each hidden layer.
        /// The <paramref name="biasSelector"/> allows to select a different bias for each neuron in each hidden layer.
        /// The <paramref name="synapseWeightSelector"/> allows to select a different weight for each synapse between each neuron. Starting from the input layer (Layer -1) to the output layer (Number of hidden layers + 1).
        /// </summary>
        /// <param name="inputCount">The ammount of inputs</param>
        /// <param name="outputCount">The ammount of outputs</param>
        /// <param name="hiddenLayersCount">The ammount of hidden layers</param>
        /// <param name="hiddenLayerNeuronCountSelector">Selects the ammount of neurons in the given hidden layer index</param>
        /// <param name="activationFunctionSelector">Selects an activation function for each neuron in each hidden layer</param>
        /// <param name="biasSelector">Selects a bias for each neuron in each hidden layer</param>
        /// <param name="synapseWeightSelector">Selects a synapse weight for each synapse between each neuron. Starting from the input layer (Layer -1) to the output layer (Number of hidden layers + 1).</param>
        public NeuralNetwork(int inputCount, int outputCount, int hiddenLayersCount, LayerNeuronCountSelector hiddenLayerNeuronCountSelector, ActivationFunctionSelector activationFunctionSelector, BiasSelector biasSelector, SynapseWeightSelector synapseWeightSelector)
        {
            _hiddenLayers = new List<HiddenLayer>();
            _inputLayer = new InputLayer(inputCount);
            _outputLayer = new OutputLayer(outputCount, (i) => ActivationFunctions.Sigmoid, (i) => 0);

            for (int layer = 0; layer < hiddenLayersCount; layer++)
            {
                var hiddenLayer = new HiddenLayer(hiddenLayerNeuronCountSelector(layer), (i) => activationFunctionSelector(layer, i), (i) => biasSelector(layer, i));
                _hiddenLayers.Add(hiddenLayer);

                if (layer == 0)
                {
                    foreach (var inputNeuron in _inputLayer.Neurons.EnumerateWithIndex())
                    {
                        foreach (var hiddenNeuron in hiddenLayer.Neurons.EnumerateWithIndex())
                        {
                            inputNeuron.Item.AddSynapse(hiddenNeuron, synapseWeightSelector(-1, inputNeuron.Index, 0, hiddenNeuron.Index));
                        }
                    }
                }
                else
                {
                    foreach (var previousLayerNeuron in _hiddenLayers[layer - 1].Neurons.EnumerateWithIndex())
                    {
                        foreach (var hiddenNeuron in hiddenLayer.Neurons.EnumerateWithIndex())
                        {
                            previousLayerNeuron.Item.AddSynapse(hiddenNeuron, synapseWeightSelector(layer - 1, previousLayerNeuron.Index, layer, hiddenNeuron.Index));
                        }
                    }
                }

                if (layer == hiddenLayersCount - 1)
                {
                    foreach (var hiddenNeuron in hiddenLayer.Neurons.EnumerateWithIndex())
                    {
                        foreach (var outputNeuron in _outputLayer.Neurons.EnumerateWithIndex())
                        {
                            hiddenNeuron.Item.AddSynapse(outputNeuron, synapseWeightSelector(layer - 1, hiddenNeuron.Index, layer, outputNeuron.Index));
                        }
                    }
                }
            }
        }
    }
}
