using SportFederationsAccounting.WPF.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SportFederationsAccounting.WPF
{
    public partial class MainWindow : Window
    {
        
        private bool isSpравочнаяPanelOpen = false;
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
            MainContent.Content = null;   // или твоя главная страница

            if (isSpравочнаяPanelOpen)
            {
                var slideUp = (Storyboard)FindResource("SlideUpAnimation");
                slideUp.Begin();
                isSpравочнаяPanelOpen = false;

                ResetSpравочнаяButton();
            }


        }
        private void ResetSpравочнаяButton()
        {
            // Это полностью снимает локальное значение и возвращает контроль стилю
            BtnSpравочная.ClearValue(Button.BackgroundProperty);
            BtnSpравочная.ClearValue(Button.ForegroundProperty);
        }
        
        private void BtnSpравочная_Click(object sender, RoutedEventArgs e)
        {
            if (isSpравочнаяPanelOpen)
            {
                // Закрываем панель
                var slideUp = (Storyboard)FindResource("SlideUpAnimation");
                slideUp.Begin();
                isSpравочнаяPanelOpen = false;

                ResetSpравочнаяButton();        // ← возвращаем в обычное состояние
            }
            else
            {
                // Открываем панель
                var slideDown = (Storyboard)FindResource("SlideDownAnimation");
                slideDown.Begin();
                isSpравочнаяPanelOpen = true;

                BtnSpравочная.Background = new SolidColorBrush(Color.FromRgb(190, 230, 253)); 
                BtnSpравочная.Foreground = new SolidColorBrush(Color.FromRgb(38, 50, 72));   // ← выделяем кнопку
            }
        }



        private void BtnFederations_Click(object sender, RoutedEventArgs e)
        {
            if (isSpравочнаяPanelOpen)
            {
                var slideUp = (Storyboard)FindResource("SlideUpAnimation");
                slideUp.Begin();
                isSpравочнаяPanelOpen = false;

                ResetSpравочнаяButton();   
            }

            // Переключаемся на страницу Федераций
            MainContent.Content = new FederationsPage();
        }
        private void BtnSportTypes_Click(object sender, RoutedEventArgs e)
        {
            var window = new SportTypesPage(isSelectionMode: false);  // режим управления
            window.ShowDialog();
        }
        private void BtnTemplates_Click(object sender, RoutedEventArgs e)
        {
            if (isSpравочнаяPanelOpen)
            {
                var slideUp = (Storyboard)FindResource("SlideUpAnimation");
                slideUp.Begin();
                isSpравочнаяPanelOpen = false;
                ResetSpравочнаяButton();
            }

            MainContent.Content = new SportFederationsAccounting.WPF.Pages.TemplatesUserControl();
        }


        private void LoadHomePage()
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}