using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SportFederationsAccounting.Data;           // ← для AppDbContext
using SportFederationsAccounting.Core.Models;    // ← для класса Federation
using Microsoft.EntityFrameworkCore.Design;

namespace SportFederationsAccounting.WPF
{
    /// <summary>
    /// Логика взаимодействия для FederationAddWindow.xaml
    /// </summary>
    public partial class FederationAddWindow : Window
    {
        private readonly AppDbContext _context;

        public FederationAddWindow()
        {
            InitializeComponent();
            _context = new AppDbContextFactory().CreateDbContext(null!);   // Подключаем базу данных
            LoadComboBoxes();                // Заполняем выпадающие списки
            GenerateNextCode();              // Генерируем следующий код федерации
        }

        private void LoadComboBoxes()
        {
            // === Вид спорта - умная логика ===
            var sportTypes = _context.SportTypes.OrderBy(s => s.Name).ToList();

            if (sportTypes.Count <= 1)
            {
                // Мало элементов — показываем ComboBox
                cbSportType.Visibility = Visibility.Visible;
                BtnSelectSportType.Visibility = Visibility.Collapsed;

                cbSportType.ItemsSource = sportTypes;
                cbSportType.DisplayMemberPath = "Name";
                cbSportType.SelectedValuePath = "Id";
            }
            else
            {
                // Много элементов — показываем кнопку выбора из справочника
                cbSportType.Visibility = Visibility.Collapsed;
                BtnSelectSportType.Visibility = Visibility.Visible;
            }

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
        private void BtnSelectSportType_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Окно выбора из справочника 'Виды спорта' пока в разработке.\n\nСкоро будет полноценный поиск и выбор.",
                           "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateNextCode()
        {
            try
            {
                // Получаем максимальный код из базы
                int maxCode = _context.Federations
                                      .Max(f => (int?)f.Code) ?? 0;

                tbCode.Text = (maxCode + 1).ToString();   
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации кода:\n{ex.Message}\n\nКод будет установлен как 1.",
                               "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbCode.Text = "1";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFullName.Text))
            {
                MessageBox.Show("Поле 'Полное наименование' обязательно!",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbFullName.Focus();
                return;
            }

            try
            {
                if (!int.TryParse(tbCode.Text, out int code))
                {
                    MessageBox.Show("Код федерации должен быть числом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var federation = new Federation
                {
                    Code = code,
                    ShortName = tbShortName.Text?.Trim(),
                    FullName = tbFullName.Text.Trim(),
                    SportTypeId = cbSportType.SelectedIndex >= 0 ? cbSportType.SelectedIndex + 1 : null,
                    AccreditationStatusId = cbAccreditationStatus.SelectedIndex >= 0 ? cbAccreditationStatus.SelectedIndex + 1 : null,
                    FederationStateId = cbFederationState.SelectedIndex >= 0 ? cbFederationState.SelectedIndex + 1 : null,
                    LegalAddress = tbLegalAddress.Text?.Trim(),
                    PostalAddress = tbPostalAddress.Text?.Trim(),
                    Phone = tbPhone.Text?.Trim(),
                    Email = tbEmail.Text?.Trim(),
                    AccreditationEndDate = dpAccreditationEnd.SelectedDate.HasValue ? DateOnly.FromDateTime(dpAccreditationEnd.SelectedDate.Value) : null
                };

                _context.Federations.Add(federation);
                _context.SaveChanges();

                // Закрываем окно без сообщения об успехе
                DialogResult = true;
                Close();

                // Передаём управление главному окну для обновления и выделения
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.RefreshFederationsList(code);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _context?.Dispose();   // Освобождаем ресурсы базы
            base.OnClosed(e);
        }

        private void cbAccreditationStatus_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}