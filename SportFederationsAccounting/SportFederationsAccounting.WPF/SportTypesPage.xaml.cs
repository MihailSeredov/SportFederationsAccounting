using Microsoft.EntityFrameworkCore;
using SportFederationsAccounting.Core.Models;
using SportFederationsAccounting.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SportFederationsAccounting.WPF
{
    public partial class SportTypesPage : Window
    {
        public event Action<SportType?>? SportTypeSelected;

        private readonly AppDbContext _context;

        private readonly bool _isSelectionMode;

        public SportTypesPage(bool isSelectionMode = false)
        {
            InitializeComponent();
            _context = new AppDbContextFactory().CreateDbContext(null!);
            _isSelectionMode = isSelectionMode;

            LoadSportTypes();
            UpdateButtonsVisibility();
        }

        private void UpdateButtonsVisibility()
        {
            // Кнопка "Выбрать" видна только в режиме выбора
            BtnSelect.Visibility = _isSelectionMode ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadSportTypes()
        {
            dgSportTypes.ItemsSource = _context.SportTypes
                                               .AsNoTracking()
                                               .OrderBy(s => s.Name)
                                               .ToList();
        }


        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            // Максимально агрессивно завершаем редактирование
            dgSportTypes.CommitEdit(DataGridEditingUnit.Cell, true);
            dgSportTypes.CommitEdit(DataGridEditingUnit.Row, true);

            // Дополнительно заставляем DataGrid обновить источник
            if (dgSportTypes.SelectedItem is SportType selected)
            {
                // Принудительно обновляем binding
                var selectedCell = dgSportTypes.CurrentCell;
                if (selectedCell != null)
                {
                    dgSportTypes.CancelEdit();
                }

                // Берём данные напрямую из базы
                var fresh = _context.SportTypes
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Id == selected.Id);

                SportTypeSelected?.Invoke(fresh ?? selected);
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            SportTypeSelected?.Invoke(null);
            this.Close();
        }

        // Двойной клик по существующему виду спорта → открываем окошко редактирования
        private void dgSportTypes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgSportTypes.SelectedItem is SportType selected)
            {
                var window = new SportTypeEditWindow(selected);
                if (window.ShowDialog() == true)
                {
                    LoadSportTypes();
                }
            }
        }


        private void BtnAddNew_Click(object sender, RoutedEventArgs e)
        {
            var window = new SportTypeEditWindow(); // без параметра = добавление
            if (window.ShowDialog() == true)
            {
                LoadSportTypes();
            }
        }
    }
}