using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace XTECHLavrinova2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход из приложения", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                if (string.IsNullOrWhiteSpace(viewModel.SearchText))
                {
                    MessageBox.Show("Пожалуйста, введите название документа в текстовое поле.", "Печать документов", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                viewModel.Print();
            }
        }
    }
}
