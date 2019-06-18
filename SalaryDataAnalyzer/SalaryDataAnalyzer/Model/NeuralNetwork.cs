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
                System.Windows.MessageBox.Show("Incorrect training data! :(");
                return;
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
                // error - squared error (difference between current network's output and desired output) divided by 2
                double error = teacher.RunEpoch(TrainingDataInput, TrainingDataOutput) / TrainingDataInput.GetLength(0);
                System.Console.WriteLine("Epoch: " + currentEpoch + " | Error: " + error );

                // save to file
                if (currentEpoch % 2 == 0)
                {
                    var newLine = (error) + ";";
                    csv.Append(newLine);
                }
                
                currentEpoch++;
                if ((Epochs != 0) && (currentEpoch > Epochs))
                {
                    if (!Directory.Exists("./Saved")) Directory.CreateDirectory("./Saved");
                    File.WriteAllText("./Saved/error_data.csv", csv.ToString());
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

        public bool Save()
        {
            if (network != null)
            {
                if (!Directory.Exists("./Saved")) Directory.CreateDirectory("./Saved");
                network.Save("./Saved/saved_network.bin");
                return true;
            } else
            {
                return false;
            }
        }

        public bool Load()
        {
            if(File.Exists("./Saved/saved_network.bin")) { 
            network = (ActivationNetwork) Network.Load("./Saved/saved_network.bin");
            if (network != null) { return true; } else { return false; }
            } else
            {
                System.Console.WriteLine("Can't find \"./Saved/saved_network.bin\" file.");
                return false;
            }
        }

        public bool SaveInputs()
        {
            if (!Directory.Exists("./Saved")) Directory.CreateDirectory("./Saved");

            using (Stream stream = File.Open("./Saved/training_data_input.bin", FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, TrainingDataInput);
            }

            using (Stream stream = File.Open("./Saved/training_data_output.bin", FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, TrainingDataOutput);
            }

            if (File.Exists("./Saved/training_data_input.bin") && File.Exists("./Saved/training_data_output.bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LoadInputs()
        {
            if (File.Exists("./Saved/training_data_input.bin") && File.Exists("./Saved/training_data_output.bin"))
            {
                using (Stream stream = File.Open("./Saved/training_data_input.bin", FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                     TrainingDataInput = (double[][])binaryFormatter.Deserialize(stream);
                }

                using (Stream stream = File.Open("./Saved/training_data_output.bin", FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    TrainingDataOutput = (double[][])binaryFormatter.Deserialize(stream);
                }

                if(TrainingDataInput != null && TrainingDataOutput != null)
                {
                    return true;
                }
            }
            System.Console.WriteLine("Can't find \"./Saved/training_data_input.bin\" and \"./Saved/training_data_output.bin\" files.");
            return false;
        }
    }
}
