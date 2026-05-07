using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using SportFederationsAccounting.Core.Models;
using SportFederationsAccounting.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Application = Microsoft.Office.Interop.Word.Application;

// Алиасы для Word
using Word = Microsoft.Office.Interop.Word;

namespace SportFederationsAccounting.WPF.Windows
{
    public partial class TemplateCardWindow : Window
    {
        private readonly AppDbContext _context;
        private Template? _template;
        private byte[]? _fileContent;
        private string? _originalFileName;

        private List<TemplateMapping> _currentMappings = new();

        public TemplateCardWindow(Template? template = null)
        {
            InitializeComponent();
            _context = new AppDbContextFactory().CreateDbContext(null!);
            _template = template;

            if (_template != null)
            {
                Title = "Редактирование шаблона";
                LoadExistingTemplate();
                btnSaveToDisk.Visibility = Visibility.Visible;
            }
            else
            {
                Title = "Новый шаблон";
                btnSaveToDisk.Visibility = Visibility.Collapsed;
            }

            LoadMappedToOptions();
        }

        private void LoadExistingTemplate()
        {
            tbName.Text = _template!.Name;
            txtFileName.Text = _template.OriginalFileName;
            _fileContent = _template.FileContent;
            _originalFileName = _template.OriginalFileName;

            // Важно: загружаем маппинги
            _currentMappings = _template.Mappings.ToList();

            // Если вдруг пусто — попробуем перезагрузить из контекста
            if (_currentMappings.Count == 0 && _template.Id > 0)
            {
                var freshTemplate = _context.Templates
                    .Include(t => t.Mappings)
                    .FirstOrDefault(t => t.Id == _template.Id);

                if (freshTemplate != null)
                {
                    _currentMappings = freshTemplate.Mappings.ToList();
                }
            }

            dgMappings.ItemsSource = _currentMappings;
        }

        private void LoadMappedToOptions()
        {
            var options = new List<string>
            {
                "", // не заполнять
                "FullName",
                "ShortName",
                "Code",
                "SportType.Name",
                "AccreditationStatus",
                "FederationState",
                "LegalAddress",
                "PostalAddress",
                "Phone",
                "Email",
                "Ogrn",
                "Inn",
                "AccreditationEndDate"
            };

            colMappedTo.ItemsSource = options;
        }

        private void BtnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Word Documents|*.docx" };
            if (dlg.ShowDialog() != true) return;

            _fileContent = File.ReadAllBytes(dlg.FileName);
            _originalFileName = Path.GetFileName(dlg.FileName);
            txtFileName.Text = _originalFileName;

            ParsePlaceholdersWithCOM();
        }

        private void ParsePlaceholdersWithCOM()
        {
            if (_fileContent == null) return;

            var found = new HashSet<string>();
            var regex = new Regex(@"\{([^}]+)\}");

            string tempPath = Path.Combine(Path.GetTempPath(), $"template_{Guid.NewGuid()}.docx");
            File.WriteAllBytes(tempPath, _fileContent);

            Application? wordApp = null;
            Word.Document? doc = null;

            try
            {
                wordApp = new Application { Visible = false, ScreenUpdating = false };
                doc = wordApp.Documents.Open(tempPath, ReadOnly: true);

                foreach (Word.Range range in doc.StoryRanges)
                {
                    string text = range.Text ?? "";
                    if (string.IsNullOrWhiteSpace(text)) continue;

                    var matches = regex.Matches(text);
                    foreach (Match m in matches)
                    {
                        string ph = "{" + m.Groups[1].Value.Trim() + "}";
                        found.Add(ph);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения шаблона: " + ex.Message);
            }
            finally
            {
                doc?.Close(false);
                wordApp?.Quit();
                try { File.Delete(tempPath); } catch { }
            }

            var updated = new List<TemplateMapping>();

            foreach (var ph in found)
            {
                var existing = _currentMappings.FirstOrDefault(m => m.Placeholder == ph);
                updated.Add(existing ?? new TemplateMapping { Placeholder = ph });
            }

            foreach (var old in _currentMappings)
            {
                if (!found.Contains(old.Placeholder))
                {
                    old.IsMissing = true;
                    updated.Add(old);
                }
            }

            _currentMappings = updated;
            dgMappings.ItemsSource = _currentMappings;
        }

        private void BtnSaveToDisk_Click(object sender, RoutedEventArgs e)
        {
            if (_fileContent == null || _fileContent.Length == 0)
            {
                MessageBox.Show("Нет файла для сохранения!", "Ошибка");
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Сохранить шаблон для редактирования",
                FileName = _originalFileName ?? "Шаблон.docx"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllBytes(saveDialog.FileName, _fileContent);
                    MessageBox.Show("Шаблон сохранён на диск.\nОтредактируйте его в Word и загрузите обратно.",
                        "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения файла:\n" + ex.Message);
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Укажите название шаблона!", "Ошибка");
                return;
            }

            if (_fileContent == null || _fileContent.Length == 0)
            {
                MessageBox.Show("Загрузите файл шаблона!", "Ошибка");
                return;
            }

            // Обработка удалённых плейсхолдеров
            var missing = _currentMappings.Where(m => m.IsMissing).ToList();
            if (missing.Any())
            {
                if (MessageBox.Show($"Найдено {missing.Count} удалённых плейсхолдеров.\nУдалить их из настроек?",
                    "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _currentMappings.RemoveAll(m => m.IsMissing);
                }
            }

            try
            {
                if (_template == null)
                {
                    // Новый шаблон
                    _template = new Template
                    {
                        Name = tbName.Text.Trim(),
                        FileContent = _fileContent!,
                        OriginalFileName = _originalFileName!,
                        CreatedAt = DateTime.UtcNow,
                        Mappings = _currentMappings
                    };

                    _context.Templates.Add(_template);
                }
                else
                {
                    // Обновление существующего
                    _template.Name = tbName.Text.Trim();
                    _template.FileContent = _fileContent!;
                    _template.OriginalFileName = _originalFileName!;
                    _template.UpdatedAt = DateTime.UtcNow;

                    // Важно: очищаем старые маппинги и добавляем новые
                    _template.Mappings.Clear();
                    foreach (var mapping in _currentMappings)
                    {
                        mapping.TemplateId = _template.Id;
                        _template.Mappings.Add(mapping);
                    }
                }

                _context.SaveChanges();

                

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения:\n{ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Генерация документа для одной федерации
        /// </summary>
        public void GenerateDocument(Federation federation, string outputFolder)
        {
            if (_fileContent == null || _template == null)
            {
                MessageBox.Show("Шаблон не загружен!");
                return;
            }

            string tempInput = Path.Combine(Path.GetTempPath(), $"temp_{Guid.NewGuid()}.docx");
            string outputPath = Path.Combine(outputFolder,
                $"{_template.Name}_{federation.ShortName}_{DateTime.Now:yyyy-MM-dd_HH-mm}.docx");

            Application? wordApp = null;
            Word.Document? doc = null;

            try
            {
                File.WriteAllBytes(tempInput, _fileContent);

                wordApp = new Application { Visible = false, ScreenUpdating = false };
                doc = wordApp.Documents.Open(tempInput);

                foreach (var mapping in _template.Mappings.Where(m => !string.IsNullOrEmpty(m.MappedTo)))
                {
                    string placeholder = mapping.Placeholder;
                    string value = GetValueFromFederation(federation, mapping.MappedTo!);

                    FindAndReplace(doc, placeholder, value);
                }

                doc.SaveAs2(outputPath);

                MessageBox.Show($"Документ успешно создан:\n{outputPath}", "Готово",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при генерации документа:\n" + ex.Message);
            }
            finally
            {
                doc?.Close(false);
                wordApp?.Quit();
                try { File.Delete(tempInput); } catch { }
            }
        }

        private string GetValueFromFederation(Federation fed, string mappedTo)
        {
            return mappedTo switch
            {
                "FullName" => fed.FullName ?? "",
                "ShortName" => fed.ShortName ?? "",
                "Code" => fed.Code.ToString(),
                "SportType.Name" => fed.SportType?.Name ?? "",
                "AccreditationStatus" => fed.AccreditationStatus.ToString(),
                "FederationState" => fed.FederationState.ToString(),
                "LegalAddress" => fed.LegalAddress ?? "",
                "PostalAddress" => fed.PostalAddress ?? "",
                "Phone" => fed.Phone ?? "",
                "Email" => fed.Email ?? "",
                "AccreditationEndDate" => fed.AccreditationEndDate?.ToString("dd.MM.yyyy") ?? "",
                _ => $"[{mappedTo}]"
            };
        }

        private void FindAndReplace(Word.Document doc, string findText, string replaceText)
        {
            Word.Find find = doc.Content.Find;
            find.ClearFormatting();
            find.Replacement.ClearFormatting();

            find.Text = findText;
            find.Replacement.Text = replaceText;
            find.Execute(Replace: Word.WdReplace.wdReplaceAll);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}