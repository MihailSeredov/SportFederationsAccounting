using Microsoft.EntityFrameworkCore;
using SportFederationsAccounting.Core.Models;
using SportFederationsAccounting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SportFederationsAccounting.WPF
{
    /// <summary>
    /// Логика взаимодействия для FederationEditWindow.xaml
    /// </summary>
    public partial class FederationEditWindow : Window
    {
        private readonly AppDbContext _context;
        private readonly Federation _federation;

        public FederationEditWindow(Federation federation)
        {
            InitializeComponent();
            _context = new AppDbContextFactory().CreateDbContext(null!);
            _federation = federation;

            LoadComboBoxes();
            LoadFederationData();
        }

        private void LoadComboBoxes()
        {
            // Загружаем без отслеживания, чтобы избежать конфликта
            cbSportType.ItemsSource = _context.SportTypes
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToList();

            cbAccreditationStatus.ItemsSource = _context.AccreditationStatuses
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToList();

            cbFederationState.ItemsSource = _context.FederationStates
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToList();

            cbSportType.DisplayMemberPath = "Name";
            cbSportType.SelectedValuePath = "Id";

            cbAccreditationStatus.DisplayMemberPath = "Name";
            cbAccreditationStatus.SelectedValuePath = "Id";

            cbFederationState.DisplayMemberPath = "Name";
            cbFederationState.SelectedValuePath = "Id";
        }

        private void LoadFederationData()
        {
            tbCode.Text = _federation.Code.ToString();
            tbShortName.Text = _federation.ShortName;
            tbFullName.Text = _federation.FullName;
            tbLegalAddress.Text = _federation.LegalAddress;
            tbPostalAddress.Text = _federation.PostalAddress;
            tbPhone.Text = _federation.Phone;
            tbEmail.Text = _federation.Email;

            cbSportType.SelectedValue = _federation.SportTypeId;
            cbAccreditationStatus.SelectedValue = _federation.AccreditationStatusId;
            cbFederationState.SelectedValue = _federation.FederationStateId;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFullName.Text))
            {
                MessageBox.Show("Поле 'Полное наименование' обязательно!", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _federation.ShortName = tbShortName.Text?.Trim();
                _federation.FullName = tbFullName.Text.Trim();
                _federation.LegalAddress = tbLegalAddress.Text?.Trim();
                _federation.PostalAddress = tbPostalAddress.Text?.Trim();
                _federation.Phone = tbPhone.Text?.Trim();
                _federation.Email = tbEmail.Text?.Trim();

                _federation.SportTypeId = cbSportType.SelectedValue as int?;
                _federation.AccreditationStatusId = cbAccreditationStatus.SelectedValue as int?;
                _federation.FederationStateId = cbFederationState.SelectedValue as int?;

                // Перезагружаем сущность, чтобы EF правильно отследил изменения
                _context.Entry(_federation).State = EntityState.Modified;

                _context.Federations.Update(_federation);
                _context.SaveChanges();

                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.RefreshFederationsList(_federation.Code);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения:\n{ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}
