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
   public partial class MainWindow : Window
    {

        private Bar prevDay;
        private Bar currDay;
        private Bar prevHour;
        private Bar currHour;
        int prevDirection;
        bool notFirstDay, wasTrade;
        EntryBar entrybar;
        
       public MainWindow()
        {
            InitializeComponent();
            currDay = new Bar();
            prevDay = new Bar();
            currHour = new Bar();
            prevHour = new Bar();
            Output.Create("test.xls");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var sr = new StreamReader("C://Users/Артур/Desktop/архив котировок/Акции для WL/ХолМРСК/ХолМРСК.txt"))
            {
                currDay.High = Double.MinValue;
                currDay.Low = Double.MaxValue;
                prevDay.Day = 1;
                wasTrade = false;
                notFirstDay = false;
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var edesc = line.Split(',');
                    var bar = new Bar(edesc[0], edesc[1], edesc[2], edesc[3], edesc[4], edesc[5], edesc[6]);
                    if (bar.Date.Day > prevDay.Day)                            // проверяем изменился ли день
                    {
                        prevDay.High = currDay.High;
                        prevDay.Low = currDay.Low;
                        prevDay.Day = bar.Date.Day;
                        notFirstDay = true;
                        wasTrade = false;
                        currDay.High = bar.High;
                        currDay.Low = bar.Low;
                    }
                    else
                    {
                        if (bar.High > currDay.High) currDay.High = bar.High; // отследиваем хай текущего дня
                        if (bar.Low < currDay.Low) currDay.Low = bar.Low; // отслеживаем лоу текущего дня

                        if (notFirstDay && !wasTrade)
                        {
                            double currDayHighLow = currDay.High - currDay.Low; // считаем диапазон текущего месяца
                            double prevDayHighLow = prevDay.High - prevDay.Low; // считаем диапазон предыдущего месяца
                            if ( (currDayHighLow > prevDayHighLow) && (bar.Date.TimeOfDay < new TimeSpan(18, 45, 00)) )
                                {
                                    wasTrade = true;
                                    entrybar = new EntryBar(bar, Math.Sign(Math.Abs(currDay.Low - bar.Close) - Math.Abs(currDay.High - bar.Close)));
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
