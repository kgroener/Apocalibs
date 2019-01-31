using Apocalibs.Core.Types;
using Apocalibs.Extensions.Mathematics;
using System;

namespace Apocalibs.ArtificialIntelligence.NeuralNetwork
{
    public delegate Scalar ActivationFunction(double value);

    public static class ActivationFunctions
    {
        /// <summary>
        /// x = ((y &gt;= 1) ? 1 : (y &lt;= 0) ? 0 : y)
        /// </summary>
        public static ActivationFunction LineairTruncated => (v) => v.Clip(0, 1);

        /// <summary>
        /// x = (y &gt;= 0.5 ? 1 : 0)
        /// </summary>
        public static ActivationFunction Binairy => (v) => v >= 0.5 ? 1 : 0;

        /// <summary>
        /// x = 1/(1+e^-y)
        /// </summary>
        public static ActivationFunction Sigmoid => (v) => 1d / (1d + Math.Exp(-v));
    }
}
