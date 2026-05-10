using System.Windows;

namespace RENAOptimizer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitApp(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private async void RunOptimize_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Optimizing... please wait";
            
            // Запуск логики в фоновом режиме, чтобы интерфейс не завис
            await System.Threading.Tasks.Task.Run(() => {
                OptimizerLogic.ApplyTweaks();
            });

            StatusText.Text = "System Optimized Successfully!";
            MessageBox.Show("RENA Optimizer has finished!", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Позволяет перетаскивать окно мышкой
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}