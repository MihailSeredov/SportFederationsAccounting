using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SportFederationsAccounting.Data;
using SportFederationsAccounting.Core.Models;

namespace SportFederationsAccounting.WPF
{
    public partial class FederationWindow : Window
    {
        public event Action<int>? FederationSaved;

        private readonly AppDbContext _context;
        private readonly Federation? _editingFederation;

        public FederationWindow(Federation? federationToEdit = null)
        {
            InitializeComponent();
            _context = new AppDbContextFactory().CreateDbContext(null!);
            _editingFederation = federationToEdit;

            LoadComboBoxes();
          

            if (_editingFederation != null)
            {
                Title = "Редактирование федерации";
                LoadFederationData();
            }
            else
            {
                Title = "Добавление новой федерации";
                GenerateNextCode();
            }
        }

        private void LoadComboBoxes()
        {
            // === Рабочий способ из старого EditWindow ===
            cbSportType.ItemsSource = _context.SportTypes
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToList();

            cbAccreditationStatus.ItemsSource = _context.AccreditationStatuses
                .AsNoTracking()
                .Where(s => s.Name != "Аккредитация завершена")
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
            if (_editingFederation == null) return;

            tbCode.Text = _editingFederation.Code.ToString();
            tbShortName.Text = _editingFederation.ShortName ?? "";
            tbFullName.Text = _editingFederation.FullName ?? "";
            tbLegalAddress.Text = _editingFederation.LegalAddress ?? "";
            tbPostalAddress.Text = _editingFederation.PostalAddress ?? "";
            tbPhone.Text = _editingFederation.Phone ?? "";
            tbEmail.Text = _editingFederation.Email ?? "";

            if (_editingFederation.AccreditationEndDate.HasValue)
                dpAccreditationEnd.SelectedDate = _editingFederation.AccreditationEndDate.Value.ToDateTime(TimeOnly.MinValue);

            cbSportType.SelectedValue = _editingFederation.SportTypeId;
            cbAccreditationStatus.SelectedValue = _editingFederation.AccreditationStatusId;
            cbFederationState.SelectedValue = _editingFederation.FederationStateId;
        }

        private void GenerateNextCode()
        {
            var maxCode = _context.Federations.Max(f => (int?)f.Code) ?? 0;
            tbCode.Text = (maxCode + 1).ToString();
        }

        private void BtnSelectSportType_Click(object sender, RoutedEventArgs e)
        {
            var window = new SportTypesPage(isSelectionMode: true);
            window.SportTypeSelected += selected =>
            {
                if (selected != null)
                {
                    LoadComboBoxes();
                    cbSportType.SelectedValue = selected.Id;
                }
            };
            window.ShowDialog();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFullName.Text))
            {
                MessageBox.Show("Полное наименование обязательно!", "Ошибка");
                return;
            }
            

            try
            {
                if (_editingFederation != null)
                {
                    // Редактирование
                    _editingFederation.ShortName = tbShortName.Text?.Trim();
                    _editingFederation.FullName = tbFullName.Text.Trim();
                    _editingFederation.SportTypeId = (int?)cbSportType.SelectedValue;

                    // === Самое важное исправление ===
                    _editingFederation.AccreditationStatusId = cbAccreditationStatus.SelectedValue as int?;
                    _editingFederation.FederationStateId = cbFederationState.SelectedValue as int?;

                    _editingFederation.LegalAddress = tbLegalAddress.Text?.Trim();
                    _editingFederation.PostalAddress = tbPostalAddress.Text?.Trim();
                    _editingFederation.Phone = tbPhone.Text?.Trim();
                    _editingFederation.Email = tbEmail.Text?.Trim();
                    _editingFederation.AccreditationEndDate = dpAccreditationEnd.SelectedDate.HasValue ? DateOnly.FromDateTime(dpAccreditationEnd.SelectedDate.Value) : null;
                    _context.Entry(_editingFederation).State = EntityState.Modified;
                    _context.Federations.Update(_editingFederation);

                    _context.SaveChanges();

                    
                }
                else
                {
                    // Добавление
                    var federation = new Federation
                    {
                        Code = int.Parse(tbCode.Text),
                        ShortName = tbShortName.Text?.Trim(),
                        FullName = tbFullName.Text.Trim(),
                        SportTypeId = (int?)cbSportType.SelectedValue,
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
                }

                


                FederationSaved?.Invoke(_editingFederation?.Code ?? int.Parse(tbCode.Text));
                if (Application.Current.MainWindow is MainWindow mainWindow)
                    mainWindow.RefreshFederationsList(_editingFederation?.Code ?? int.Parse(tbCode.Text));
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
