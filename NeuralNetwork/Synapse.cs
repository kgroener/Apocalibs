namespace Apocalibs.ArtificialIntelligence.NeuralNetwork
{
    internal class Synapse
    {
        public Synapse(Neuron neuronIn, Neuron neuronOut, double weight)
        {
            NeuronIn = neuronIn;
            NeuronOut = neuronOut;
            Weight = weight;
        }

        public Neuron NeuronIn { get; }
        public Neuron NeuronOut { get; }

        public double Weight { get; set; }
    }
}
