using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryDataAnalyzer
{
    class NeuralNetwork
    {
        public double learningRate { get; set; } = 0.1;
        public double momentum { get; set; } = 0.0;
        public double sigmoidAlphaValue { get; set; } = 2.0;
        public int neuronsInFirstLayer { get; set; } = 20;

        public void start()
        {
                learningRate = Math.Max(0.00001, Math.Min(1, learningRate));
                momentum = Math.Max(0, Math.Min(0.5, momentum));
                sigmoidAlphaValue = Math.Max(0.001, Math.Min(50, sigmoidAlphaValue));
                neuronsInFirstLayer = Math.Max(5, Math.Min(50, neuronsInFirstLayer));

            // iterations
            try
            {
                iterations = Math.Max(0, int.Parse(iterationsBox.Text));
            }
            catch
            {
                iterations = 1000;
            }

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(SearchSolution));
            workerThread.Start();
        }

        // Worker thread
        void SearchSolution()
        {
            // number of learning samples
            int samples = data.GetLength(0);

            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[1];
                output[i] = new double[1];

                // set input
                input[i][0] = (data[i, 0] - xMin) * xFactor - 1.0;
                // set output
                output[i][0] = (data[i, 1] - yMin) * yFactor - 0.85;
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue),
                1, neuronsInFirstLayer, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            // iterations
            int iteration = 1;

            // solution array
            double[,] solution = new double[50, 2];
            double[] networkInput = new double[1];

            // calculate X values to be used with solution function
            for (int j = 0; j < 50; j++)
            {
                solution[j, 0] = chart.RangeX.Min + (double)j * chart.RangeX.Length / 49;
            }

            // loop
            while (!needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;

                // calculate solution
                for (int j = 0; j < 50; j++)
                {
                    networkInput[0] = (solution[j, 0] - xMin) * xFactor - 1.0;
                    solution[j, 1] = (network.Compute(networkInput)[0] + 0.85) / yFactor + yMin;
                }
                chart.UpdateDataSeries("solution", solution);
                // calculate error
                double learningError = 0.0;
                for (int j = 0, k = data.GetLength(0); j < k; j++)
                {
                    networkInput[0] = input[j][0];
                    learningError += Math.Abs(data[j, 1] - ((network.Compute(networkInput)[0] + 0.85) / yFactor + yMin));
                }

                // set current iteration's info
                SetText(currentIterationBox, iteration.ToString());
                SetText(currentErrorBox, learningError.ToString("F3"));

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((iterations != 0) && (iteration > iterations))
                    break;
            }

        }
    }
}
