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
using System.IO;
namespace WpfApplication1hjy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EntryBar entrybar;
        public MainWindow()
        {
            InitializeComponent();
            Output.Create("test.xls");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var sr = new StreamReader("C://Users/Артур/Desktop/архив котировок/Акции для WL/склейкМинРи/Ри.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var edesc = line.Split(',');
                    var bar = new Bar(edesc[0], edesc[1], edesc[2], edesc[3], edesc[4], edesc[5], edesc[6]);
                    if (((bar.Date.Hour == 10) && (bar.Date.Minute == 0)) || ((bar.Date.Hour == 11) && (bar.Date.Minute == 0)))
                    {
                        entrybar = new EntryBar(bar);
                        Output.Add(entrybar);
                    }
                }
                Output.Save();
            }
        }
    }
}
    