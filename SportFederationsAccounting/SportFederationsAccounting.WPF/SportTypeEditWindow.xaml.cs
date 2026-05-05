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
    /// Логика взаимодействия для SportTypeEditWindow.xaml
    /// </summary>
    public partial class SportTypeEditWindow : Window
    {
        private readonly AppDbContext _context;
        private readonly SportType? _sportType; // null = добавление нового

        public SportTypeEditWindow(SportType? sportType = null)
        {
            InitializeComponent();
            _context = new AppDbContextFactory().CreateDbContext(null!);
            _sportType = sportType;

            if (_sportType == null)
            {
                Title = "Добавление нового вида спорта";
                tbName.Text = "";
            }
            else
            {
                Title = "Редактирование вида спорта";
                tbName.Text = _sportType.Name;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Название вида спорта обязательно!", "Ошибка");
                return;
            }

            string newName = tbName.Text.Trim();

            try
            {
                // Проверка на дубликат
                if (_context.SportTypes.Any(s => s.Name.ToLower() == newName.ToLower() &&
                                               (_sportType == null || s.Id != _sportType.Id)))
                {
                    MessageBox.Show("Вид спорта с таким названием уже существует!", "Ошибка");
                    return;
                }

                if (_sportType == null)
                {
                    // Добавление нового
                    var newType = new SportType
                    {
                        Name = newName,
                        Code = newName.Length > 50
                            ? newName.Substring(0, 50).ToUpper()
                            : newName.ToUpper()
                    };

                    _context.SportTypes.Add(newType);
                    _context.SaveChanges();
                    DialogResult = true;
                    Close();
                }
                else
                {
                    // Редактирование
                    _sportType.Name = newName;
                    _context.Entry(_sportType).State = EntityState.Modified;
                    _context.SaveChanges();
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
