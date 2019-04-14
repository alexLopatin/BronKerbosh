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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace kursach
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        



        private void MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point punto = e.GetPosition(null);
            int mouseX = (int)punto.X;
            int mouseY = (int)punto.Y;
        }
        public MainWindow()
        {
            InitializeComponent();
            
            

            DataContext = new ApplicationViewModel(canvas);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window help = new Help();
            help.ShowDialog();
        }
    }

}
