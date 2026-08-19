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
    /// Логика взаимодействия для AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        MainWindow mainWindow;
        public AddTaskWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.Owner = mainWindow;
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

        private void SaveAndClose(object sender, RoutedEventArgs e)
        {
            string description = Description.Text;
            if (Date.SelectedDate == null)
            {
                MessageBox.Show("Выберите срок сдачи задачи");
                return;
            }
            DateTime date = Date.SelectedDate.Value;
            int importance = int.Parse(Importance.Text);
            int days = int.Parse(Days.Text);
            Task task = new Task(description, date, importance, days);
            try
            {
                TaskNode taskNode = mainWindow.taskList.AddTask(task, out int indexInsert);
                ListBoxItem taskItem = new ListBoxItem();
                taskItem.Tag = taskNode;
                taskItem.Content = taskNode.GetDescription() + "    Дата начала: " + taskNode.GetStartDateWork().ToString("dd-MM-yyyy");
                mainWindow.tasks.Items.Insert(indexInsert, taskItem);
                foreach (ListBoxItem item in mainWindow.tasks.Items)
                {
                    if (item.Equals(taskItem))
                    {
                        break;
                    }
                    item.Content = ((TaskNode) item.Tag).GetDescription() + "    Дата начала: " + ((TaskNode)item.Tag).GetStartDateWork().ToString("dd-MM-yyyy");
                }
                this.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
