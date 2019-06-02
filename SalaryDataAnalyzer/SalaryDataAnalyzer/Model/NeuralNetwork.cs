using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.IO;
using System.Threading;

namespace SalaryDataAnalyzer
{
    class NeuralNetwork
    {
        private ActivationNetwork network;
        private BackPropagationLearning teacher;

        public double LearningRate { get; set; } = 0.1;
        public double Momentum { get; set; } = 0.0;
        public double SigmoidAlphaValue { get; set; } = 2.0;
        public int Epochs { get; set; } = 1000;
        public int NeuronsInFirstLayer { get; set; } = 10;

        // data needed for training
        public double[][] TrainingDataInput { get; set; }
        public double[][] TrainingDataOutput { get; set; }

        // data needed for calculation
        public double[] NetworkInput { get; set; }
        public double[] NetworkResult { get; set; }

        private bool stopTraining = false;

        public void StartTraining()
        {
            LearningRate = Math.Max(0.00001, Math.Min(1, LearningRate));
            Momentum = Math.Max(0, Math.Min(0.5, Momentum));
            SigmoidAlphaValue = Math.Max(0.001, Math.Min(50, SigmoidAlphaValue));
            NeuronsInFirstLayer = Math.Max(5, Math.Min(50, NeuronsInFirstLayer));
            Epochs = Math.Max(1, Epochs);

            stopTraining = false;
            Thread workerThread = new Thread(new ThreadStart(Train));
            workerThread.Start();
        }

        public void CancelTraining()
        {
            stopTraining = true;
        }


        void Train()
        {
            if(TrainingDataInput == null ||
               TrainingDataOutput == null ||
               TrainingDataInput.Length != TrainingDataOutput.Length)
            {
                throw new Exception("Incorrect training data.");
            }

            network = new ActivationNetwork(new BipolarSigmoidFunction(SigmoidAlphaValue), TrainingDataInput[0].Length, NeuronsInFirstLayer, 1);
            teacher = new BackPropagationLearning(network)
            {
                LearningRate = LearningRate,
                Momentum = Momentum
            };

            int currentEpoch = 1;

            
            var csv = new System.Text.StringBuilder();

            while (!stopTraining)
            {
                double error = teacher.RunEpoch(TrainingDataInput, TrainingDataOutput) / TrainingDataInput.GetLength(0);
                System.Console.WriteLine("Training error: " + error + " | Epoch: " + currentEpoch );

                // save to file
                if (currentEpoch % 2 == 0)
                {
                    var newLine = (100.0 - error) + ";";
                    csv.Append(newLine);
                }
                
                currentEpoch++;
                if ((Epochs != 0) && (currentEpoch > Epochs))
                {
                    File.WriteAllText("./error_data_every50steps.csv", csv.ToString());
                    break;
                }
            }

        }

        public void Calculate()
        {
            if (network == null)
            {
                throw new Exception("Network untrained.");
            }

            NetworkResult = network.Compute(NetworkInput);
        }

    }
}
