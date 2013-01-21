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
        bool notFirstDay, wasTrade, wasFix, notFirstHour;
        EntryBar entrybar;
        double stopPrice;
        double Max1, Max2, Max3, Max4;


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
            using (var sr = new StreamReader("C://Users/Артур/Desktop/архив котировок/Акции для WL/RiМинИнтрадей/СклИнтрадейRI(01.2009-12.2012).txt"))
            {
                currDay.High = Double.MinValue;
                currDay.Low = Double.MaxValue;
                currHour.High = Double.MinValue;
                currHour.Low = Double.MaxValue;
                prevDay.Day = 11;
                wasTrade = false;
                wasFix = false;
                notFirstDay = false;
                notFirstHour = false;
                prevHour.Date = new DateTime(2020, 12, 12, 10, 59, 59);
                prevDay.Month = 1; // для месяцев добавляю
                prevDay.Year = 2009; // для лет добавляю
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var edesc = line.Split(',');
                    var bar = new Bar(edesc[0], edesc[1], edesc[2], edesc[3], edesc[4], edesc[5], edesc[6]);



                    if (bar.Date.Day > prevDay.Day || bar.Date.Month > prevDay.Month || bar.Date.Year > prevDay.Year)                            // проверяем изменился ли день
                    {
                        prevDay.High = currDay.High;
                        prevDay.Low = currDay.Low;
                        prevDay.Day = bar.Date.Day;
                        notFirstDay = true;
                        wasTrade = false;
                        wasFix = false;
                        currDay.High = bar.High;
                        currDay.Low = bar.Low;
                        prevHour.High = currHour.High;
                        prevHour.Low = currHour.Low;
                        currHour.High = bar.High;
                        currHour.Low = bar.Low;
                        prevHour.Date = bar.Date;
                        prevDay.Month = bar.Date.Month; // для месяцев добавляю
                        prevDay.Year = bar.Date.Year; // для лет добавляю
                    }
                    else
                    {
                        if (bar.High > currDay.High) currDay.High = bar.High; // отследиваем хай текущего дня
                        if (bar.Low < currDay.Low) currDay.Low = bar.Low; // отслеживаем лоу текущего дня

                        if (bar.Date.Hour == 23 && bar.Date.Minute == 49 && wasTrade && entrybar.FixPrice == 0)
                        {
                            entrybar.FixPrice = bar.Close;
                            Output.Add(entrybar);
                        }

                        if (bar.Date.Hour != prevHour.Date.Hour)
                        {
                            prevHour.High = currHour.High;
                            prevHour.Low = currHour.Low;
                            currHour.High = bar.High;
                            currHour.Low = bar.Low;
                            prevHour.Date = bar.Date;
                        }
                        else
                        {
                            if (bar.High > currHour.High) currHour.High = bar.High; // отследиваем хай текущего часа
                            if (bar.Low < currHour.Low) currHour.Low = bar.Low; // отслеживаем лоу текущего часа
                        }


                        if (wasTrade && entrybar.Direction == 1 && stopPrice > bar.Low)
                        {
                            entrybar.FixPrice = stopPrice;
                            wasTrade = false;
                            wasFix = true;
                            switch (entrybar.entryBar.Date.Hour - bar.Date.Hour)
                            {
                                case 0:
                                    entrybar.Max1 = currHour.High - entrybar.EntryPrice;
                                    break;
                                case 1:
                                    entrybar.Max2  = currHour.High - entrybar.EntryPrice;
                                    break;
                                case 2:
                                    entrybar.Max3 = currHour.High - entrybar.EntryPrice;
                                    break;
                                case 3:
                                    entrybar.Max4 = currHour.High - entrybar.EntryPrice;
                                    break;
                            }
                            Output.Add(entrybar);

                        }
                        if (wasTrade && entrybar.Direction == -1 && stopPrice < bar.Low)
                        {
                            entrybar.FixPrice = stopPrice;
                            wasTrade = false;
                            wasFix = true;
                            switch (entrybar.entryBar.Date.Hour - bar.Date.Hour)
                            {
                                case 0:
                                    entrybar.Max1 = -currHour.High + entrybar.EntryPrice;
                                    break;
                                case 1:
                                    entrybar.Max2 = -currHour.High + entrybar.EntryPrice;
                                    break;
                                case 2:
                                    entrybar.Max3 = -currHour.High + entrybar.EntryPrice;
                                    break;
                                case 3:
                                    entrybar.Max4 = -currHour.High + entrybar.EntryPrice;
                                    break;
                            }
                            Output.Add(entrybar);
                        }

                        if (wasTrade && (entrybar.entryBar.Date.Hour < bar.Date.Hour))
                        {

                            if (entrybar.Direction == 1)
                            {
                                switch ( -entrybar.entryBar.Date.Hour + bar.Date.Hour)
                                {
                                    case 1:
                                        entrybar.Max1 = prevHour.High - entrybar.EntryPrice;
                                        break;
                                    case 2:
                                        entrybar.Max2 = prevHour.High - entrybar.EntryPrice;
                                        break;
                                    case 3:
                                        entrybar.Max3 = prevHour.High - entrybar.EntryPrice;
                                        break;
                                    case 4:
                                        entrybar.Max4 = prevHour.High - entrybar.EntryPrice;
                                        break;
                                }
                            }
                            else
                            {
                                switch ( -entrybar.entryBar.Date.Hour + bar.Date.Hour)
                                {
                                    case 1:
                                        entrybar.Max1 = -prevHour.Low + entrybar.EntryPrice;
                                        break;
                                    case 2:
                                        entrybar.Max2 = -prevHour.Low + entrybar.EntryPrice;
                                        break;
                                    case 3:
                                        entrybar.Max3 = -prevHour.Low + entrybar.EntryPrice;
                                        break;
                                    case 4:
                                        entrybar.Max4 = -prevHour.Low + entrybar.EntryPrice;
                                        break;
                                }
                            }
                        }

                        if (notFirstDay && !wasTrade && !wasFix)
                        {
                            double currDayHighLow = currDay.High - currDay.Low; // считаем диапазон текущего месяца
                            double prevDayHighLow = prevDay.High - prevDay.Low; // считаем диапазон предыдущего месяца
                            if ((currDayHighLow > prevDayHighLow) && (bar.Date.TimeOfDay < new TimeSpan(18, 45, 00)))
                            {
                                wasTrade = true;
                                double entryPrice;
                                double initRisk;
                                var direction = Math.Sign(Math.Abs(currDay.Low - bar.Close) - Math.Abs(currDay.High - bar.Close));
                                if (direction == 1)
                                {
                                    entryPrice = currDay.Low + prevDayHighLow;
                                    initRisk = entryPrice - currHour.Low;
                                    stopPrice = currHour.Low - 10;
                                }
                                else
                                {
                                    entryPrice = currDay.High - prevDayHighLow;
                                    initRisk = currHour.High - entryPrice;
                                    stopPrice = currHour.High + 10;
                                }

                                entrybar = new EntryBar(
                                    bar,
                                    direction,
                                    initRisk,
                                    prevDayHighLow,
                                    entryPrice,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0
                                );
                                //  Console.WriteLine("close:" + entrybar.Close + " dir:" + entrybar.Direction + " date:" + bar.Date); 

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