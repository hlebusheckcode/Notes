using Microsoft.Win32;
using Notes.Model;
using Notes.Repository;
using Notes.Wpf.Controls;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Notes
{
    public partial class ConfigurationWindow : Window
    {
        IMemoRepository _repository;

        public ConfigurationWindow(IMemoRepository repository)
        {
            InitializeComponent();
            _repository = repository;
        }

        private void ImportClick(object sender, RoutedEventArgs e)
            => Import();
        private void ExportClick(object sender, RoutedEventArgs e)
            => Export();

        private async Task Import()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var content = await File.ReadAllTextAsync(openFileDialog.FileName);
                if (string.IsNullOrEmpty(content))
                    return;
                var newItems = JsonSerializer.Deserialize<Memo[]>(content);
                if (newItems?.Any() != true)
                    return;
                await _repository.Import(newItems);
                if(Owner is MainWindow mainWindow)
                    mainWindow.Refresh();
                CustomMessageBox.Show(
                    this,
                    $"Import completed.",
                    "Informing",
                    "\u0027",
                    (SolidColorBrush)Application.Current.FindResource("InformationSolidBrush"),
                    MessageBoxButton.OK);
            }
        }

        private async Task Export()
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                RestoreDirectory = true,
                FileName = "Exported"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var content = JsonSerializer.Serialize((await _repository.Get(RemoveOption.All)).ToArray());
                await File.WriteAllTextAsync(saveFileDialog.FileName, content);
                CustomMessageBox.Show(
                    this,
                    $"Export completed.",
                    "Informing",
                    "\u0030",
                    (SolidColorBrush)Application.Current.FindResource("InformationSolidBrush"),
                    MessageBoxButton.OK);
            }
        }
    }
}
