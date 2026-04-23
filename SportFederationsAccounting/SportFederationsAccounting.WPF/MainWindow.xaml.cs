using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SportFederationsAccounting.WPF
{
    public partial class MainWindow : Window
    {
        private bool isPanelOpen = false;
        public int? AddedFederationCode { get; set; }
        // Метод для обновления списка
        public void RefreshFederationsList(int? newFederationCode = null)
        {
            // Переключаемся на страницу Федераций, если ещё не там
            if (MainContent.Content is not FederationsPage)
            {
                MainContent.Content = new FederationsPage();
            }

            if (MainContent.Content is FederationsPage page)
            {
                page.RefreshList(newFederationCode);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadHomePage();                    // при запуске показываем дашборд

        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            // 1. Закрываем выдвижную панель, если она открыта
            if (isPanelOpen)
            {
                var storyboard = (Storyboard)Resources["SlideUpAnimation"];
                storyboard.Begin();

                // Ждём окончания анимации и скрываем панель
                System.Threading.Tasks.Task.Delay(300).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        SpравочнаяПанель.Visibility = Visibility.Collapsed;
                        SpравочнаяПанель.Height = 0;
                        isPanelOpen = false;
                    });
                });
            }
            // 2. Сбрасываем выделение кнопки "Справочная информация"
            if (BtnSpравочная != null)
            {
                BtnSpравочная.Background = new SolidColorBrush(Color.FromRgb(38, 50, 72)); // #37474F
                BtnSpравочная.Foreground = new SolidColorBrush(Colors.White);
            }
            MainContent.Content = new TextBlock
            {
                
                
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray)
            };
        }

        private void BtnSpравочная_Click(object sender, RoutedEventArgs e)
        {
            isPanelOpen = !isPanelOpen;

            if (isPanelOpen)
            {
                // Панель открывается
                SpравочнаяПанель.Height = 0;
                SpравочнаяПанель.Visibility = Visibility.Visible;
                var storyboard = (Storyboard)Resources["SlideDownAnimation"];
                storyboard.Begin();

                // Выделяем кнопку серым фоном
                BtnSpравочная.Background = new SolidColorBrush(Color.FromRgb(55, 71, 79));
                BtnSpравочная.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                // Панель закрывается
                var storyboard = (Storyboard)Resources["SlideUpAnimation"];
                storyboard.Begin();

                System.Threading.Tasks.Task.Delay(300).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        SpравочнаяПанель.Visibility = Visibility.Collapsed;
                        // Возвращаем кнопку в обычное состояние
                        BtnSpравочная.Background = new SolidColorBrush(Color.FromRgb(55, 71, 79)); // #37474F
                        BtnSpравочная.Foreground = new SolidColorBrush(Colors.White);
                    });
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
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}