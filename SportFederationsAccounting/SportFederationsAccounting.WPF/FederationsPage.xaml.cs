using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using SportFederationsAccounting.Data;
using SportFederationsAccounting.Core.Models;

namespace SportFederationsAccounting.WPF
{
    public partial class FederationsPage : UserControl
    {
        private readonly AppDbContext _context;

        public FederationsPage()
        {
            InitializeComponent();
            _context = new AppDbContextFactory().CreateDbContext(null!);
            LoadFederations();
        }

        private void LoadFederations()
        {
            try
            {
                var federations = _context.Federations
                    .Include(f => f.SportType)
                    .Include(f => f.AccreditationStatus)
                    .Include(f => f.FederationState)
                    .OrderBy(f => f.Code)
                    .ToList();

                dgFederations.ItemsSource = federations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списка федераций:\n{ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RefreshList(int? newFederationCode = null)
        {
            LoadFederations();

            if (newFederationCode.HasValue)
            {
                var item = dgFederations.Items.Cast<Federation>()
                    .FirstOrDefault(f => f.Code == newFederationCode.Value);

                if (item != null)
                {
                    dgFederations.SelectedItem = item;
                    dgFederations.ScrollIntoView(item);

                    // Дополнительно — принудительно обновляем UI
                    dgFederations.Focus();
                }
            }
        }

        private void BtnAddFederation_Click(object sender, RoutedEventArgs e)
        {
            var window = new FederationAddWindow();
            if (window.ShowDialog() == true)
            {
                RefreshList();   // обновляем список
            }
        }

        private void dgFederations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgFederations.SelectedItem is Federation f)
            {
                MessageBox.Show($"Открыта федерация: {f.FullName} (Код: {f.Code})", "Информация");
            }
        }
    }
}