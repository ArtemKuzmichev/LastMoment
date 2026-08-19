using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LastMoment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TaskList taskList = new TaskList();
        string name_file = "tasks.json";
        public MainWindow()
        {
            InitializeComponent();
            ReadTasks();
        }
        private void ReadTasks()
        {
            if (File.Exists(name_file))
            {
                try
                {
                    TaskNode curr = taskList.FromJSON(File.ReadAllText(name_file));
                    int indexInsert = 0;
                    while (curr != null)
                    {
                        ListBoxItem item = new ListBoxItem();
                        item.Tag = curr;
                        item.Content = curr.GetDescription() + "    Дата начала: " + curr.GetStartDateWork().ToString("dd-MM-yyyy");
                        tasks.Items.Insert(indexInsert, item);
                        curr = curr.GetNext();
                        indexInsert++;
                    }
                }
                catch (JsonException) {
                    MessageBox.Show("Файл испорчен или имеет неверный формат. Список задач будет очищен.", "Ошибка чтения",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void AddTask(object sender, RoutedEventArgs e)
        {
            var AddTaskWIndow = new AddTaskWindow(this);
            AddTaskWIndow.Show();
        }

        private void OpenTask(object sender, MouseButtonEventArgs e)
        {
            if (tasks.SelectedItem == null)
            {
                return;
            }
            if (tasks.SelectedItem is ListBoxItem taskItem)
            {
                if (taskItem.Tag is TaskNode node)
                {
                    var changeTaskWindow = new ChangeTaskWindow(this, node, taskItem);
                    changeTaskWindow.Show();
                }
            }
        }
        private void SaveAndCloseWindow(object sender, System.ComponentModel.CancelEventArgs e) 
        {
            try
            {
                File.WriteAllText(name_file, taskList.ToJSON());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}