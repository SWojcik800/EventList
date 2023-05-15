using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_DataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TestTextField TextObj { get; set; } = new TestTextField()
        {
            TextVal = "Test"
        };
        public MainWindow()
        {
            DataContext = TextObj;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextObj.TextVal = "Test23";
        }
    }

    public class TestTextField : INotifyPropertyChanged
    {
        public string TextVal { set
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Text"));
            } }

        public event PropertyChangedEventHandler? PropertyChanged;
    }


}
