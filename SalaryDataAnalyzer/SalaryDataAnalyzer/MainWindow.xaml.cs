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
            // temporary
            // I ran out of unused buttons to check if network works
            _network.NetworkInput = new double[] { 0, 1, 0 };
            _network.Calculate();
            System.Console.WriteLine(_network.NetworkResult[0]);

            _network.NetworkInput = new double[] { 0, 1, 1 };
            _network.Calculate();
            System.Console.WriteLine(_network.NetworkResult[0]);

            _network.NetworkInput = new double[] { 0, 0, 0 };
            _network.Calculate();
            System.Console.WriteLine(_network.NetworkResult[0]);

            var headers = new List<string>();
            foreach (var item in questionGrid.SelectedItems)
            {
                var question = (Question)item;
                headers.Add(question.Header);
            }

            AnswerLabel.Content = _service.GetCountOfFullAnswers(
                _survey, headers);
        }

        private class DataObject
        {
            public string Response { get; set; }
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            var list = new ObservableCollection<DataObject>();
            var question = (Question)questionGrid.SelectedItem;
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
                var newLine = response + ",";
                csv.Append(newLine);

                list.Add(new DataObject
                {
                    Response = response
                });
            }

            File.WriteAllText("./options.csv", csv.ToString());

            answerGrid.ItemsSource = list;
            FitToContent(answerGrid);
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            CreateButton.IsEnabled = false;
            CancelTrainButton.IsEnabled = true;

            _network.StartTraining();

            CancelTrainButton.IsEnabled = false;
            CreateButton.IsEnabled = true;
        }

        private void CancelTrainButton_Click(object sender, RoutedEventArgs e)
        {
            _network.CancelTraining();

            CancelTrainButton.IsEnabled = false;
            CreateButton.IsEnabled = true;
            TrainButton.IsEnabled = true;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var factory = new NormalizersFactory();
            var normalizers = factory.CreateNormalizers();

            var questions = _survey.Questions.ToArray();     
            
            var responses = _survey.Responses.Select(x => x.Answers.Select((a, i) => normalizers.FirstOrDefault(n => n.HeaderValue.Equals(questions[i].Header))?.NormalizeData(a)));
            System.Console.WriteLine(responses.ToArray().ToArray());

            // temp
            _network.TrainingDataInput = new double[4][] { new double[]{1,0,1}, new double[] { 1, 1, 0 }, new double[] { 1, 0, 1 }, new double[] { 1, 1, 0 } };
            _network.TrainingDataOutput = new double[4][] { new double[] { 0 }, new double[] { 1 }, new double[] { 0 }, new double[] { 1 } };

            TrainButton.IsEnabled = true;
        }
    }
}
