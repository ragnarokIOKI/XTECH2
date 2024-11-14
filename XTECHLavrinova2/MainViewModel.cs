using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace XTECHLavrinova2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _searchText;
        private ObservableCollection<Person_Model> _people;

        public MainViewModel()
        {
            People = new ObservableCollection<Person_Model>
            {
                new Person_Model { Number_Person = 1, SecondName_Person = "Ivanov", Name_Person = "Ivan", MiddleName_Person = "Ivanovich" },
                new Person_Model { Number_Person = 2, SecondName_Person = "Petrov", Name_Person = "Petr", MiddleName_Person = "Petrovich" }
            };
        }

        public ObservableCollection<Person_Model> People
        {
            get => _people;
            set { _people = value; OnPropertyChanged(nameof(People)); }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(LabelContent));
            }
        }

        public string LabelContent => SearchText;

        public void Print()
        {
            string documentName = $"{SearchText}.pdf";
            CreatePdf(documentName);
        }

        private void CreatePdf(string documentName)
        {
            try
            {
                using (var fs = new FileStream(documentName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var document = new Document(PageSize.A4, 50, 50, 25, 25);
                    var writer = PdfWriter.GetInstance(document, fs);
                    document.Open();

                    var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.EMBEDDED);
                    var font = new Font(baseFont, 12, Font.NORMAL);

                    var titleFont = new Font(baseFont, 16, Font.BOLD);
                    document.Add(new Paragraph(SearchText, titleFont));
                    document.Add(new Paragraph("\n"));

                    PdfPTable table = new PdfPTable(4)
                    {
                        WidthPercentage = 100
                    };

                    table.AddCell(new PdfPCell(new Phrase("№", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase("Фамилия", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase("Имя", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase("Отчество", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    foreach (var person in People)
                    {
                        Debug.WriteLine($"Добавляем в PDF: {person.Number_Person}, {person.SecondName_Person}, {person.Name_Person}, {person.MiddleName_Person}");

                        table.AddCell(new PdfPCell(new Phrase($"{person.Number_Person}", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase($"{person.SecondName_Person}", font)));
                        table.AddCell(new PdfPCell(new Phrase($"{person.Name_Person}", font)));
                        table.AddCell(new PdfPCell(new Phrase($"{person.MiddleName_Person}", font)));
                    }

                    document.Add(table);
                    document.Close();
                }

                string directoryPath = Path.GetDirectoryName(Path.GetFullPath(documentName));
                Process.Start(new ProcessStartInfo
                {
                    FileName = directoryPath,
                    UseShellExecute = true,
                    Verb = "open"
                });

                System.Windows.MessageBox.Show($"PDF-файл '{documentName}' успешно создан.", "Успех", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при создании PDF: {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
