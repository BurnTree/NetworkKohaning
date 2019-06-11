using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohaningNeuralNetwork.component
{
    class Network
    {
        public readonly Input[] _inputs;
        public readonly Neuron[] _neurons;

        public Network(int inputCount, int outputCount)
        {
            _neurons = new Neuron[outputCount];
            for (var i = 0; i < outputCount; i++)
            {
                _neurons[i] = new Neuron { IncomingLinks = new Link[inputCount] };
            }

            _inputs = new Input[inputCount];
            for (var i = 0; i < inputCount; i++)
            {
                var inputNeuron = new Input();

                inputNeuron.OutgoingLinks = new Link[outputCount];
                for (var j = 0; j < outputCount; j++)
                {
                    var link = new Link
                    {
                        Neuron = _neurons[j]
                    };
                    inputNeuron.OutgoingLinks[j] = link;
                    _neurons[j].IncomingLinks[i] = link;
                }

                _inputs[i] = inputNeuron;
            }
        }

        public int Handle(double[] input)
        {
            for (var i = 0; i < _inputs.Length; i++)
            {
                var inputNeuron = _inputs[i];
                foreach (var outgoingLink in inputNeuron.OutgoingLinks)
                {
                    outgoingLink.Neuron.Power += outgoingLink.Weight * input[i];
                }
            }

            var maxIndex = 0;
            for (var i = 1; i < _neurons.Length; i++)
            {
                if (_neurons[i].Power > _neurons[maxIndex].Power)
                    maxIndex = i;
            }

            foreach (var outputNeuron in _neurons)
            {
                outputNeuron.Power = 0;
            }

            return maxIndex;
        }

        public void Study(double[] input, int correctAnswer)
        {
            var neuron = _neurons[correctAnswer];
            for (var i = 0; i < neuron.IncomingLinks.Length; i++)
            {
                var incomingLink = neuron.IncomingLinks[i];
                incomingLink.Weight = incomingLink.Weight + 0.5 * (input[i] - incomingLink.Weight);
            }
        }

        public void Create()
        {
            Random random = new Random();
            for (int i = 0; i < _neurons.Length; i++)
            {
                var neuron = _neurons[i];
                for (int j = 0; j < neuron.IncomingLinks.Length; j++)
                {
                    var incomingLink = neuron.IncomingLinks[j];
                    incomingLink.Weight = random.NextDouble() * (0.8 - 0.2) + 0.2;
                }
            }
        }
    }
}
