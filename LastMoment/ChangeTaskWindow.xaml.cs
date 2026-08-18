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

namespace LastMoment
{
    /// <summary>
    /// Логика взаимодействия для ChangeTask.xaml
    /// </summary>
    public partial class ChangeTaskWindow : Window
    {
        MainWindow mainWindow;
        TaskNode node;
        ListBoxItem taskItem;
        public ChangeTaskWindow(MainWindow mainWindow, TaskNode node, ListBoxItem taskItem)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.node = node;
            this.taskItem = taskItem;
            Description.Text = this. node.GetDescription();
            Description.IsReadOnly = true;
            Date.SelectedDate = this.node.GetDeadline();
            Date.IsEnabled = false;
            Importance.Text = this.node.GetImportance().ToString();
            Importance.IsEnabled = false;
            Days.Text = this.node.GetDays().ToString();
            Days.IsReadOnly = true;
        }
        private void IsDigital(object sender, TextCompositionEventArgs e)
        {
            string days_str = Days.Text;
            foreach (char c in e.Text)
            {
                if (!(char.IsDigit(c) && (int.Parse(days_str + c) != 0) && (int.Parse(days_str + c) < 366)))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            if (!mainWindow.taskList.DeleteTask(node))
            {
                MessageBox.Show("Ошибка удаления задачи", "Ошибка удаления задачи", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mainWindow.tasks.Items.Remove(taskItem);
            this.Close();
        }
        private void ChangeTask(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
