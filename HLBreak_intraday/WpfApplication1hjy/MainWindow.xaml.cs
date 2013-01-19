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

                        if (wasTrade && entrybar.Direction == 1 && stopPrice > bar.Low )
                        {
                            entrybar.FixPrice = stopPrice;
                            wasTrade = false;
                            switch (entrybar.entryBar.Date.Hour - bar.Date.Hour) 
                                {
                                    case 0:
                                        Max1 = currHour.High - entrybar.EntryPrice;
                                        break;
                                    case 1:
                                        Max2 = currHour.High - entrybar.EntryPrice;
                                        break;
                                    case 2:
                                        Max3 = currHour.High - entrybar.EntryPrice;
                                        break;
                                    case 3:
                                        Max4 = currHour.High - entrybar.EntryPrice;
                                        break;
                                }
                        }
                        if (wasTrade && entrybar.Direction == -1 && stopPrice > bar.Low )
                        {
                            entrybar.FixPrice = stopPrice;
                            wasTrade = false;
                            switch (entrybar.entryBar.Date.Hour - bar.Date.Hour) 
                                {
                                    case 0:
                                        Max1 = - currHour.High + entrybar.EntryPrice;
                                        break;
                                    case 1:
                                        Max2 = - currHour.High + entrybar.EntryPrice;
                                        break;
                                    case 2:
                                        Max3 = - currHour.High + entrybar.EntryPrice;
                                        break;
                                    case 3:
                                        Max4 = - currHour.High + entrybar.EntryPrice;
                                        break;
                                }
                        }

                        if (wasTrade && (entrybar.entryBar.Date.Hour < bar.Date.Hour) ) 
                        {
                            
                            if (entrybar.Direction == 1) 
                            {
                                switch (entrybar.entryBar.Date.Hour - bar.Date.Hour) 
                                {
                                    case 1:
                                        Max1 = prevHour.High - entrybar.EntryPrice;
                                        break;
                                    case 2:
                                        Max2 = prevHour.High - entrybar.EntryPrice;
                                        break;
                                    case 3:                      
                                        Max3 = prevHour.High - entrybar.EntryPrice;
                                        break;
                                    case 4:
                                        Max4 = prevHour.High - entrybar.EntryPrice;
                                        break;
                                }
                            }
                            else 
                            {
                                switch (entrybar.entryBar.Date.Hour - bar.Date.Hour) 
                                {
                                    case 1:
                                        Max1 = - prevHour.Low + entrybar.EntryPrice;
                                        break;
                                    case 2:
                                        Max2 = - prevHour.Low + entrybar.EntryPrice;
                                        break;
                                    case 3:
                                        Max3 = - prevHour.Low + entrybar.EntryPrice;
                                        break;
                                    case 4:
                                        Max4 = - prevHour.Low + entrybar.EntryPrice;
                                        break;
                                }
                            }
                        }

                        if (notFirstDay && !wasTrade)
                        {
                            double currDayHighLow = currDay.High - currDay.Low; // считаем диапазон текущего месяца
                            double prevDayHighLow = prevDay.High - prevDay.Low; // считаем диапазон предыдущего месяца
                            if ( (currDayHighLow > prevDayHighLow) && (bar.Date.TimeOfDay < new TimeSpan(18, 45, 00)) )
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
                                    //    Output.Add(entrybar);
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
