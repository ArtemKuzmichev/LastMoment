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
        public AddTaskWindow()
        {
            InitializeComponent();
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
            string date = Date.Text;
            string importance = Importance.Text;
            string days = Days.Text;
        }
    }
}
