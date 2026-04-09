using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SportFederationsAccounting.WPF
{
    public partial class MainWindow : Window
    {
        private bool isPanelOpen = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadHomePage();                    // при запуске показываем дашборд
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            LoadHomePage();
        }

        private void BtnSpравочная_Click(object sender, RoutedEventArgs e)
        {
            isPanelOpen = !isPanelOpen;

            if (isPanelOpen)
            {
                SpравочнаяПанель.Height = 0;
                SpравочнаяПанель.Visibility = Visibility.Visible;
                var storyboard = (Storyboard)Resources["SlideDownAnimation"];
                storyboard.Begin();
            }
            else
            {
                var storyboard = (Storyboard)Resources["SlideUpAnimation"];
                storyboard.Begin();

                System.Threading.Tasks.Task.Delay(300).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => SpравочнаяПанель.Visibility = Visibility.Collapsed);
                });
            }
        }

        private void BtnFederations_Click(object sender, RoutedEventArgs e)
        {
            SpравочнаяПанель.Visibility = Visibility.Collapsed;
            isPanelOpen = false;

            // Загружаем список федераций
            MainContent.Content = new FederationsPage();
        }

        private void LoadHomePage()
        {
            MainContent.Content = new TextBlock
            {
                Text = "Главный дашборд\n\nЗдесь будет статистика,\nколичество федераций и важная информация",
                FontSize = 28,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray)
            };
        }
    }
}