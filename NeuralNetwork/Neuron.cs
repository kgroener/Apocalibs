using Apocalibs.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork
{
    internal class Neuron
    {
        private object _synapsesLock = new object();
        private object _valueLock = new object();
        private readonly ActivationFunction _activationFunction;
        private List<Synapse> _outputSynapses;
        private readonly double _bias;
        private List<Synapse> _inputSynapses;
        private Scalar? _value;

        public Neuron(ActivationFunction activationFunction, double bias)
        {
            _activationFunction = activationFunction;
            _inputSynapses = new List<Synapse>();
            _outputSynapses = new List<Synapse>();
            _bias = bias;
        }

        public Scalar GetValue()
        {
            lock (_valueLock)
            {
                return (_value = _value ?? CalculateValue()).Value;
            }
        }

        private Scalar CalculateValue()
        {
            if (_activationFunction == null)
            {
                throw new ArgumentNullException(nameof(ActivationFunction), "No activation function specified. Specify an activation function or call SetValue first.");
            }

            return _activationFunction(_inputSynapses.Sum(s => s.NeuronIn.GetValue() * s.Weight) + _bias);
        }

        public void SetValue(Scalar value)
        {
            SetValueInternal(value);
        }

        private void Reset()
        {
            SetValueInternal(null);
        }

        private void SetValueInternal(Scalar? value)
        {
            lock (_valueLock)
            {
                _value = value;

                lock (_synapsesLock)
                {
                    foreach (var synapse in _outputSynapses)
                    {
                        synapse.NeuronOut.Reset();
                    }
                }
            }
        }

        internal IEnumerable<Synapse> InputSynapses => _inputSynapses;
        internal IEnumerable<Synapse> OutputSynapses => _outputSynapses;

        public double Bias => _bias;

        internal ActivationFunction ActivationFunction => _activationFunction;

        public void AddSynapse(Neuron toNeuron, double weight)
        {
            lock (_synapsesLock)
            {
                Synapse newSynapse = new Synapse(this, toNeuron, weight);
                _outputSynapses.Add(newSynapse);
                toNeuron.ConnectInputSynapse(newSynapse);
            }
        }

        private void ConnectInputSynapse(Synapse synapse)
        {
            lock (_synapsesLock)
            {
                _inputSynapses.Add(synapse);
            }
        }

        public void RemoveSynapse(Neuron toNeuron)
        {
            lock (_synapsesLock)
            {
                var synapseToRemove = _outputSynapses.Single(s => s.NeuronOut == toNeuron);

                synapseToRemove.NeuronOut.Reset();
                synapseToRemove.NeuronOut.DisconnectInputSynapse(synapseToRemove);
                _outputSynapses.Remove(synapseToRemove);
            }
        }

        private void DisconnectInputSynapse(Synapse synapseToRemove)
        {
            lock (_synapsesLock)
            {
                _inputSynapses.Remove(synapseToRemove);
            }
        }
    }
}
