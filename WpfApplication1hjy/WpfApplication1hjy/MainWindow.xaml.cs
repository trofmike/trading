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
        private Bar prevMonth;
        private Bar currMonth;
        bool notFirstMonth, wasTrade;
        EntryBar entrybar;
        int prevDirection;
        public int entrymonth;

        public MainWindow()
        {
            InitializeComponent();
            currMonth = new Bar();
            prevMonth = new Bar();
            Output.Create("test.xls");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var sr = new StreamReader("C://Users/Артур/Desktop/архив котировок/Акции для WL/Тнкбипи/Тнкбп_1мин.txt"))
            {
                currMonth.High = Double.MinValue;
                currMonth.Low = Double.MaxValue;
                prevMonth.Month = 1;
                prevMonth.Year = 99;
                wasTrade = false;
                notFirstMonth = false;
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var edesc = line.Split(',');
                    var bar = new Bar(edesc[0], edesc[1], edesc[2], edesc[3], edesc[4], edesc[5], edesc[6]);
                    if (bar.Date.Month > prevMonth.Month || bar.Date.Year > prevMonth.Year)                            // проверяем изменился ли месяц
                    {
                        prevMonth.High = currMonth.High;
                        prevMonth.Low = currMonth.Low;
                        prevMonth.Month = bar.Date.Month;
                        prevMonth.Year = bar.Date.Year;
                        notFirstMonth = true;
                        wasTrade = false;
                        currMonth.High = bar.High;
                        currMonth.Low = bar.Low;
                    }
                    else
                    {
                        if (bar.High > currMonth.High) currMonth.High = bar.High; // отследиваем хай текущего месяца
                        if (bar.Low < currMonth.Low) currMonth.Low = bar.Low; // отслеживаем лоу текущего месяца

                        if (notFirstMonth && !wasTrade)
                        {
                            double currMonthHighLow = currMonth.High - currMonth.Low; // считаем диапазон текущего месяца
                            double prevMonthHighLow = prevMonth.High - prevMonth.Low; // считаем диапазон предыдущего месяца
                            if (currMonthHighLow > prevMonthHighLow)
                                {
                                    wasTrade = true;
                                    entrybar = new EntryBar(bar, Math.Sign(Math.Abs(currMonth.Low - bar.Close) - Math.Abs(currMonth.High - bar.Close)));
                                    if (entrybar.Direction != prevDirection)
                                    {   
                                        prevDirection = entrybar.Direction;
                                        Console.WriteLine("close:" + entrybar.Close + " dir:" + entrybar.Direction + " date:" + bar.Date); 
                                        Output.Add(entrybar);
                                    }
                                }
                        }
                    }
                }
                Console.WriteLine("Вышли из цикла");
                Output.Save();
            }

        }
    }
}
