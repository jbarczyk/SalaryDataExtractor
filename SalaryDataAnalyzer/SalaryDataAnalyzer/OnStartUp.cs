using System.Configuration;
using System.Windows;
using Microsoft.Configuration.ConfigurationBuilders;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SalaryDataAnalyzer.Contracts;
using SalaryDataAnalyzer.Extractors;
using SalaryDataAnalyzer.Services;

namespace SalaryDataAnalyzer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            services.AddTransient<ICsvExtractor, CsvExtractor>();
            services.AddTransient<IDataAnalyzerService, DataAnalyzerService>();
            services.AddSingleton<MainWindow>();

            var provider = services.BuildServiceProvider();
            var mainWindow = provider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
