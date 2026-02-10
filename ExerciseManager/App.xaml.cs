using ExerciseManager.Mediators;
using ExerciseManager.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ExerciseManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ViewMediator viewMediator = new ViewMediator();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(viewMediator)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}
