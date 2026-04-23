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
            // Вид спорта
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
                MessageBox.Show("Поле 'Полное наименование' обязательно для заполнения!",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbFullName.Focus();
                return;
            }

            try
            {
                if (!int.TryParse(tbCode.Text, out int code))
                {
                    MessageBox.Show("Код федерации должен быть числом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    tbCode.Focus();
                    return;
                }

                var federation = new Federation
                {
                    Code = code,
                    ShortName = tbShortName.Text?.Trim(),
                    FullName = tbFullName.Text.Trim(),
                    // Остальные поля (SportTypeId, AccreditationStatusId и т.д.) пока оставляем null
                    // Это нормально, потому что мы сделали их необязательными в миграции
                };

                _context.Federations.Add(federation);
                _context.SaveChanges();

                MessageBox.Show($"Федерация успешно сохранена!\n\nКод: {tbCode.Text}\nПолное наименование: {tbFullName.Text}",
                               "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                string innerMessage = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Ошибка при сохранении:\n{innerMessage}",
                               "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}