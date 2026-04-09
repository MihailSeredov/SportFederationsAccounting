using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SportFederationsAccounting.WPF
{
    public partial class FederationsPage : UserControl
    {
        public FederationsPage()
        {
            InitializeComponent();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Пока просто заглушка
            MessageBox.Show("Поиск: " + tbSearch.Text);
        }

        private void BtnAddFederation_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new FederationAddWindow();

            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                MessageBox.Show("Федерация была успешно добавлена!\n\n(Пока только заглушка. В будущем здесь будет обновление списка)",
                                "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                // В будущем здесь будет обновление таблицы
            }
        }

        private void dgFederations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgFederations.SelectedItem != null)
            {
                MessageBox.Show("Открывается карточка выбранной федерации", "Редактирование федерации");
            }
        }
    }
}