using System;
using System.Collections.Generic;
using System.Text;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork.Layers
{
    internal class InputLayer : NeuronLayer
    {
        public InputLayer(int numberOfNeurons) : base(numberOfNeurons, (i) => null, (i) => 0)
        {

        }
    }
}
