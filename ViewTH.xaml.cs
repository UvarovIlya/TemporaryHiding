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
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TemporaryHiding
{
    /// <summary>
    /// Логика взаимодействия для ViewTH.xaml
    /// </summary>
    public partial class ViewTH : Window, IDisposable
    {
        [DllImport("user32.dll")]
        internal extern static int SetWindowLong(IntPtr hwnd, int index, int value);
        [DllImport("user32.dll")]
        internal extern static int GetWindowLong(IntPtr hwnd, int index);

        internal static void HideMinimizeAndMaximizeButtons(Window window)
        {
            const int GWL_STYLE = -16;

            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            long value = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (int)(value & -131073 & -65537));
        }

        public ViewTH()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            this.Close();
        }

        private void Window_SourceInitialized(object sender, EventArgs eventArgs)
        {
            HideMinimizeAndMaximizeButtons(this);
        }

        

        internal ViewModelTH viewModel { get; set; }

        private void ListBoxParameters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ListBox)sender).ScrollIntoView(e.AddedItems[0]);
        }

        //private void SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    var ViewModel.SelectedTreeNode;
        //}

        //internal ViewModelTH ViewModel { get; set; }
    }

    //public class Node
    //{
    //    public string Category { get; set; }
    //    public IEnumerable<Node> Nodes { get; set; }
    //}
}
