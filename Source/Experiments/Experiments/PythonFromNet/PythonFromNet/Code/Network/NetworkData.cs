using System;


namespace PythonFromNet
{
    public class NetworkData
    {
        public int[] LayerSizes { get; protected set; }
        public int NumberOfLayers { get; protected set; }
        public int NumberOfDataPlanes { get; protected set; }

        public int[] Indices { get; protected set; }
        public int[][] MiniBatches { get; protected set; }

        public Vector[] Biases { get; protected set; }
        public Matrix[] Weights { get; protected set; }

        // Used in feed-forwarding.
        public Vector[] PreActivations { get; protected set; }
        public Vector[] Activations { get; protected set; }

        // Used in training.
        public Vector[] NablaBiases { get; protected set; }
        public Matrix[] NablaWeights { get; protected set; }

        // Used in back propagation.
        public Vector[] DeltaNablaBiases { get; protected set; }
        public Matrix[] DeltaNablaWeights { get; protected set; }


        public NetworkData(int[] layerSizes)
        {
            this.LayerSizes = layerSizes;
            this.NumberOfLayers = this.LayerSizes.Length;
            this.NumberOfDataPlanes = this.NumberOfLayers - 1;
        }
    }
}
