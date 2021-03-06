﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SalaryDataAnalyzer.Contracts;
using SalaryDataAnalyzer.Extractors;
using SalaryDataAnalyzer.Factories;
using SalaryDataAnalyzer.Services;

namespace SalaryDataAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICsvExtractor _extractor;
        private readonly IDataAnalyzerService _service;
        private Survey _survey;
        private NeuralNetwork _network;

        public MainWindow(ICsvExtractor extractor, IDataAnalyzerService service)
        {
            _extractor = extractor;
            _service = service;
            _network = new NeuralNetwork();
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadButton.IsEnabled = false;

            var list = new ObservableCollection<Question>();
            _survey = new Survey
            {
                Questions = await _extractor.GetHeaders(),
            };

            foreach (var question in _survey.Questions)
            {
                list.Add(question);
            }

            questionGrid.ItemsSource = list;
            FitToContent(questionGrid);
            var task = Task.Run(() =>
            {
                _survey = _extractor.GetSurvey();
            });

            await task;
            CreateButton.IsEnabled = true;
            CalculateButton.IsEnabled = true;
            LoadNetworkButton.IsEnabled = true;
            LoadInputsButton.IsEnabled = true;
        }

        private void FitToContent(DataGrid grid)
        {
            foreach (var column in grid.Columns)
            {
                column.Width = new DataGridLength(2.0, DataGridLengthUnitType.SizeToCells);
                column.Width = new DataGridLength(2.0, DataGridLengthUnitType.SizeToHeader);
                column.Width = new DataGridLength(2.0, DataGridLengthUnitType.Auto);
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {

            var headers = new List<string>();
            foreach (var item in questionGrid.SelectedItems)
            {
                var question = (Question)item;
                headers.Add(question.Header);
            }

            AnswerLabel.Content = _service.GetCountOfFullAnswers(
                _survey, headers);
            ShowButton.IsEnabled = true;
        }

        private class DataObject
        {
            public string Response { get; set; }
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            var list = new ObservableCollection<DataObject>();
            var question = (Question)questionGrid.SelectedItem;
            if(question == null)
            {
                System.Windows.MessageBox.Show("Select a question first");
                return;
            }
            var questionId = _survey.Questions.Select((q, i) => new { q, i })
                .Where(x => x.q.Header.Equals(question.Header))
                .FirstOrDefault()
                .i;

            // save to file
            var csv = new StringBuilder();

            foreach (var response in _survey.Responses
                .Select( x=> x.Answers[questionId])
                .Distinct()
                .OrderBy(x => x))
            {
                if (!response.Contains(";"))
                {
                    var newLine = "<ComboBoxItem Content=\"" + response + "\"></ComboBoxItem>\n";
                    csv.Append(newLine);
                }
                list.Add(new DataObject
                {
                    Response = response
                });
            }

            if (!Directory.Exists("./Saved")) Directory.CreateDirectory("./Saved");
            File.WriteAllText("./Saved/"+question.Header+".txt", csv.ToString());

            answerGrid.ItemsSource = list;
            FitToContent(answerGrid);
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            CreateButton.IsEnabled = false;

            _network.StartTraining();

            CreateButton.IsEnabled = true;
            SaveNetworkButton.IsEnabled = true;
            CalcButton.IsEnabled = true;
        }

        private void TrainTestButton_Click(object sender, RoutedEventArgs e)
        {
            CreateButton.IsEnabled = false;

            _network.StartTrainingAndTesting();

            CreateButton.IsEnabled = true;
            SaveNetworkButton.IsEnabled = true;
            CalcButton.IsEnabled = true;
        }

        private void CancelTrainButton_Click(object sender, RoutedEventArgs e)
        {
            _network.CancelTraining();

            CreateButton.IsEnabled = true;
            TrainButton.IsEnabled = true;
            TrainTestButton.IsEnabled = true;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNetworkButton.IsEnabled = false;
            var factory = new NormalizersFactory();
            var normalizers = factory.CreateNormalizers();

            //var shuffledResponses = _survey.Responses.OrderBy(elem => Guid.NewGuid()); // shuffle

            var questions = _survey.Questions.ToArray();
            var responses = _survey.Responses;

            Model.Extensions.Shuffle(responses.ToList());

            var outputVectors = new List<List<decimal>>();
            var inputVectors = responses.Select((x, idx) =>
            {
                // find vectors where all required answers exist and normalize them (with nulls at this point)
                System.Console.WriteLine("input:\t" + (idx + 1) + " / " + responses.Count());
                return x.Answers.SelectMany((a, i) =>
                {
                    var correctNormalizers = normalizers.Where(n => n.HeaderValue.Equals(questions[i].Header));
                    return correctNormalizers.Select(n => n.NormalizeData(a));
                });
            }).Where(vec => !vec.Contains(null)).Select(x => 
            {
                outputVectors.Add(new List<decimal> { x.Last().Value });
                foreach (var y in x)
                {
                    if (y != null)
                    {
                        System.Console.Write(System.String.Format("{0,15}", System.Math.Round((double)y, 5)));
                    }
                    else
                    {
                        System.Console.Write(System.String.Format("{0,15}", "null"));
                    }
                }
                System.Console.Write('\n');

                return x.Select(y => (double)y.Value).Reverse().Skip(1).Reverse().ToArray();
            }).ToArray();
            
            //here we take 80% for training and 20% for testing
            var vectorCount = inputVectors.Count();

            _network.TrainingDataInput = inputVectors.Take((int)(vectorCount * 0.8)).ToArray();
            _network.TrainingDataOutput = outputVectors.Take((int)(vectorCount * 0.8)).Select(x => x.Select(y => (double) y).ToArray()).ToArray();

            _network.TestingDataInput = inputVectors.Skip((int)(vectorCount * 0.8)).ToArray();
            _network.TestingDataOutput = outputVectors.Skip((int)(vectorCount * 0.8)).Select(x => x.Select(y => (double)y).ToArray()).ToArray();

            TrainButton.IsEnabled = true;
            TrainTestButton.IsEnabled = true;
            LoadNetworkButton.IsEnabled = true;
            SaveInputsButton.IsEnabled = true;
        }

        private void LoadNetwork_Click(object sender, RoutedEventArgs e)
        {
            if (_network.Load())
            {
                MessageBox.Show("Network loaded");
                SaveNetworkButton.IsEnabled = false;
                TrainButton.IsEnabled = true;
                TrainTestButton.IsEnabled = true;
                LoadNetworkButton.IsEnabled = true;
            }
        }

        private void SaveNetwork_Click(object sender, RoutedEventArgs e)
        {
            if(_network.Save())
            {
                MessageBox.Show("Network saved");
            }
        }

        private void LoadInputs_Click(object sender, RoutedEventArgs e)
        {
            if(_network.LoadInputs())
            {
                MessageBox.Show("Network inputs loaded");
                TrainButton.IsEnabled = true;
                TrainTestButton.IsEnabled = true;
                SaveInputsButton.IsEnabled = false;
            }
        }

        private void SaveInputs_Click(object sender, RoutedEventArgs e)
        {
            if (_network.SaveInputs())
            {
                MessageBox.Show("Network inputs saved");
            }
        }

        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            var factory = new NormalizersFactory();
            var normalizers = factory.CreateNormalizers().Skip(1);

            var questions = new string[] { "Country","CompanySize","DevType","Employment","FormalEducation","Student","YearsCoding"};
            var responses = new string[] {
                //CountryCombo.Text ,
                CompanySizeCombo.Text ,
                //DevTypeCombo.Text,
                EmploymentCombo.Text,
                FormalEducationCombo.Text,
                StudentCombo.Text,
                YearsCodingCombo.Text};

            if(responses.Any(x => x == ""))
            {
                System.Windows.MessageBox.Show("Select all answers");
                return;
            }

            var input = new List<double> { };

            for(int o = 0; o < normalizers.Count(); o++)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (questions[i] == normalizers.ElementAt(o).HeaderValue)
                    {
                        decimal? temp = normalizers.ElementAt(o).NormalizeData(responses[i]);
                        if(temp == null)
                        {
                            temp = 0;
                        }
                        input.Add((double)temp.Value);
                    }
                }
            }

            //hardcoded input vector to check
            Console.WriteLine("Result: " + _network.Calculate(input.ToArray())[0] * 1_000_000);
            ResultLabel.Content = Math.Round(_network.Calculate(input.ToArray())[0]*1_000_000) + " USD/Year";
        }
    }
}
