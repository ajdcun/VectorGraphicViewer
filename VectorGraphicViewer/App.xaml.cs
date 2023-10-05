using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using VectorGraphicViewer.MVVM.ViewModel;
using VectorGraphicViewer.Services;

namespace VectorGraphicViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider ServiceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddScoped(serviceProvider => new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainViewModel>()
            });
            services.AddScoped<MainViewModel>();
            services.AddScoped<ShapeViewModel>();
            services.AddScoped<LineViewModel>();
            services.AddScoped<CircleViewModel>();
            services.AddScoped<TriangleViewModel>();
            services.AddScoped<IDataService>(provider =>
            {
                var inputPath = Path.Combine(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Input")));
                return new JsonDataService(inputPath);
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
