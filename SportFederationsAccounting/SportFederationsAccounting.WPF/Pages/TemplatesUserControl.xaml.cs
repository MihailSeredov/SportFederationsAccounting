using Microsoft.EntityFrameworkCore;
using SportFederationsAccounting.Core.Models;
using SportFederationsAccounting.Data;
using SportFederationsAccounting.WPF.Windows;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SportFederationsAccounting.WPF.Pages
{
    public partial class TemplatesUserControl : UserControl
    {
        private readonly AppDbContext _context;

        public TemplatesUserControl()
        {
            InitializeComponent();
            _context = new AppDbContextFactory().CreateDbContext(null!);
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            dgTemplates.ItemsSource = _context.Templates
                .Include(t => t.Mappings)           
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        private void BtnAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            var window = new TemplateCardWindow();
            if (window.ShowDialog() == true)
            {
                LoadTemplates();
            }
        }

        private void dgTemplates_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgTemplates.SelectedItem is Template selectedTemplate)
            {
                var window = new TemplateCardWindow(selectedTemplate);
                if (window.ShowDialog() == true)
                {
                    LoadTemplates();
                }
            }
        }
    }
}