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
        private Bar currDay;
        private Bar prevDay;
        private Bar prevBar;
        private Bar prev2Hour;
        private Bar first2Hour;
        private Bar curr2Hour;
        bool wasTrade, wasFix, notFirst2Hour;
        EntryBar entrybar;
        double stopPrice;

        public MainWindow()
        {
            InitializeComponent();
            currDay = new Bar();
            prevDay = new Bar();
            prevBar = new Bar();
            first2Hour = new Bar();
            curr2Hour = new Bar();
            prev2Hour = new Bar();
            Output.Create("test.xls");
            wasFix = false;
            wasTrade = false;
            notFirst2Hour = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var sr = new StreamReader("C://Users/Артур/Downloads/SPFB.RTS_121217_130201.txt"))
            {
                prev2Hour.Hour = 10;
                prevDay.Day = 17;
                curr2Hour.High = Double.MinValue;
                curr2Hour.Low = Double.MaxValue;

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var edesc = line.Split(',');
                    var bar = new Bar(edesc[0], edesc[1], edesc[2], edesc[3], edesc[4], edesc[5], edesc[6]);

                    if (bar.Date.Day != prevDay.Day)
                    {
                        prev2Hour.Hour = 10;
                        curr2Hour.High = bar.High;
                        curr2Hour.Low = bar.Low;
                        prevDay.Day = bar.Date.Day;
                        
                        if (wasTrade && !wasFix) 
                        {
                            entrybar.FixPrice = prevBar.Close;
                            Output.Add(entrybar);
                        }

                        wasTrade = false;
                        wasFix = false;
                        notFirst2Hour = false;
                    }


                    if (bar.Date.Hour - prev2Hour.Hour == 2 )
                    {
                        if (bar.Date.Hour < 13 && bar.Date.Minute == 0) 
                        {
                            first2Hour.High = curr2Hour.High;
                            first2Hour.Low = curr2Hour.Low;
                            notFirst2Hour = true;
                        }
                        prev2Hour.High = curr2Hour.High;
                        prev2Hour.Low = curr2Hour.Low;
                        curr2Hour.High = bar.High;
                        curr2Hour.Low = bar.Low;
                        prev2Hour.Hour = bar.Date.Hour;
                    }
                    else
                    {
                        prevBar = bar;
                        if (bar.High > curr2Hour.High) curr2Hour.High = bar.High; // отследиваем хай текущих 2 часов
                        if (bar.Low < curr2Hour.Low) curr2Hour.Low = bar.Low; // отслеживаем лоу текущих 2 часов
                    }

                    if (bar.Date.Hour - prev2Hour.Hour == 0 && notFirst2Hour)
                    {
                        if (prev2Hour.High > first2Hour.High && prev2Hour.Low < first2Hour.Low)
                        {
                            wasTrade = true;
                            wasFix = true;
                        }
                        // чтоб тока в одну сторону хай или лоу превышался
                        if ((prev2Hour.High > first2Hour.High || prev2Hour.Low < first2Hour.Low) && !(prev2Hour.High > first2Hour.High && prev2Hour.Low < first2Hour.Low) && !wasTrade)
                        {
                            wasTrade = true;
                            double entryPrice;
                            int direction;
                            // определям дирекшн
                            if (curr2Hour.High > first2Hour.High) direction = 1;
                            else direction = -1;
                            entryPrice = bar.Open;
                            if (direction == 1) stopPrice = prev2Hour.Low - (prev2Hour.High - prev2Hour.Low) / 2;
                            else                stopPrice = prev2Hour.High + (prev2Hour.High - prev2Hour.Low) / 2;

                            entrybar = new EntryBar(bar, direction, entryPrice, 0);
                        }
                    }
                    if (wasTrade && !wasFix)
                    {
                        if (entrybar.Direction == 1)
                        {
                            if (bar.Low < stopPrice)
                            {
                                entrybar.FixPrice = stopPrice;
                                wasFix = true;
                                Output.Add(entrybar);
                            }
                        }
                        else
                        {
                            if (bar.High > stopPrice)
                            {
                                entrybar.FixPrice = stopPrice;
                                wasFix = true;
                                Output.Add(entrybar);
                            }
                        }
                    }

                }

            }
            Console.WriteLine("Вышли из цикла");
            Output.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var sr = new StreamReader("C://Users/Артур/Desktop/архив котировок/Акции для WL/SRМинИнтрадей/SR_min_Intraday.txt"))
            {
                StreamWriter sw = new StreamWriter("C://Users/Артур/Desktop/output.txt");
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var edesc = line.Split(',');
                    var bar = new Bar(edesc[0], edesc[1], edesc[2], edesc[3], edesc[4], edesc[5], edesc[6]);

                    if (bar.Volume != 0)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
        }

    }
}