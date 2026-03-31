using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SportFederationsAccounting.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPanelOpen = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Начальная страница (дашборд)");
        }

        private void BtnSpравочная_Click(object sender, RoutedEventArgs e)
        {
            isPanelOpen = !isPanelOpen;
            SpравочнаяПанель.Visibility = isPanelOpen ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnFederations_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открывается раздел Федераций", "Федерации");
            SpравочнаяПанель.Visibility = Visibility.Collapsed;
            isPanelOpen = false;
        }
    }
}