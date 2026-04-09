using System.Windows;

namespace SportFederationsAccounting.WPF

/// <summary>
/// Логика взаимодействия для FederationAddWindow.xaml
/// </summary>
{
    public partial class FederationAddWindow : Window
    {
        public FederationAddWindow()
        {
            InitializeComponent();
            LoadComboBoxes();   // ← добавили вызов заполнения списков
        }

        private void LoadComboBoxes()
        {
            // Вид спорта (тестовые значения)
            cbSportType.Items.Add("Лыжные гонки");
            cbSportType.Items.Add("Биатлон");
            cbSportType.Items.Add("Фигурное катание");
            cbSportType.Items.Add("Хоккей");
            cbSportType.Items.Add("Футбол");

            // Статус аккредитации
            cbAccreditationStatus.Items.Add("Аккредитована");
            cbAccreditationStatus.Items.Add("Аккредитация в процессе");
            cbAccreditationStatus.Items.Add("Отказ в аккредитации");
            cbAccreditationStatus.Items.Add("Аккредитация завершена");

            // Состояние федерации
            cbFederationState.Items.Add("Действующая");
            cbFederationState.Items.Add("В процессе реорганизации");
            cbFederationState.Items.Add("В статусе ликвидации");
            cbFederationState.Items.Add("Ликвидирована");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCode.Text))
            {
                MessageBox.Show("Поле 'Код федерации' обязательно для заполнения!",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbFullName.Text))
            {
                MessageBox.Show("Поле 'Полное наименование' обязательно для заполнения!",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbFullName.Focus();
                return;
            }

            MessageBox.Show($"Федерация успешно сохранена!\n\nКод: {tbCode.Text}\nНаименование: {tbFullName.Text}",
                            "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
    
