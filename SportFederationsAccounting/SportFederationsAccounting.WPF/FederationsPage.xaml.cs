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
            // Полная перезагрузка с отключением отслеживания
            _context.ChangeTracker.Clear(); // сбрасываем все отслеживаемые сущности

            var federations = _context.Federations
                .AsNoTracking()
                .Include(f => f.SportType)
                .Include(f => f.AccreditationStatus)
                .Include(f => f.FederationState)
                .OrderBy(f => f.Code)
                .ToList();

            dgFederations.ItemsSource = federations;

            if (newFederationCode.HasValue)
            {
                var item = federations.FirstOrDefault(f => f.Code == newFederationCode.Value);
                if (item != null)
                {
                    dgFederations.SelectedItem = item;
                    dgFederations.ScrollIntoView(item);
                }
            }
        }

        private void BtnAddFederation_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new FederationWindow();

            // Исправленная подписка
            addWindow.FederationSaved += code => RefreshList(code);

            addWindow.ShowDialog();
        }

        private void dgFederations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgFederations.SelectedItem is Federation selectedFederation)
            {
                var editWindow = new FederationWindow(selectedFederation);

                // Исправленная подписка
                editWindow.FederationSaved += code => RefreshList(code);

                editWindow.ShowDialog();
            }
        }

    }
}