using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;

namespace WpfApplication1hjy
{
    class Output
    {
        private static Workbook workbook;
        private static Worksheet worksheet;
        private static string Filename;
        private static int Count;

        public static void Create(string filename)
        {
            Filename = filename;
            workbook = new Workbook();
            worksheet = new Worksheet("Output");

            for (int i = 0; i < 100; i++)
                worksheet.Cells[i, 0] = new Cell("");
        }

        public static void Save()
        {
            workbook.Worksheets.Add(worksheet);

            workbook.Save(Filename);
        }

        public static void Add(EntryBar entry)
        {
            worksheet.Cells[Count, 0] = new Cell(entry.entryBar.Date, "dd/mm/yyyy");
            worksheet.Cells[Count, 1] = new Cell(entry.entryBar.Date, "HH:mm:ss");
            worksheet.Cells[Count, 2] = new Cell(entry.InitRisk);
            worksheet.Cells[Count, 3] = new Cell(entry.PrevDayHighLow);
            worksheet.Cells[Count, 4] = new Cell(entry.EntryPrice);
            worksheet.Cells[Count, 5] = new Cell(entry.Direction);
            worksheet.Cells[Count, 6] = new Cell(entry.Max1);
            worksheet.Cells[Count, 7] = new Cell(entry.Max2);
            worksheet.Cells[Count, 8] = new Cell(entry.Max3);
            worksheet.Cells[Count, 9] = new Cell(entry.Max4);
            worksheet.Cells[Count, 10] = new Cell(entry.FixPrice);
            worksheet.Cells[Count, 11] = new Cell(entry.Slipage);
            Count++;
        }
    }
}
