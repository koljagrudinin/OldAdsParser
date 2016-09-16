using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelLibrary.SpreadSheet;

namespace _074_Parser.Enigne
{
    class Saver
    {
        public void Save(string fileName)
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("Первый Лист");
            worksheet.Cells[0, 0] = new Cell("Название");
            worksheet.Cells[0, 1] = new Cell("Справочник");
            worksheet.Cells[0, 2] = new Cell("Рубрика");
            worksheet.Cells[0, 3] = new Cell("Адрес");
            worksheet.Cells[0, 4] = new Cell("Адрес 2");
            worksheet.Cells[0, 5] = new Cell("Телефон");
            for (int i = 0; i < Parser.Data.DATA.Count(); i++)
            {
                worksheet.Cells[i + 1, 0] = new Cell(Parser.Data.DATA[i].Name);
                worksheet.Cells[i + 1, 1] = new Cell(Parser.Data.DATA[i].Sprav);
                worksheet.Cells[i + 1, 2] = new Cell(Parser.Data.DATA[i].Rubric);
                worksheet.Cells[i + 1, 3] = new Cell(Parser.Data.DATA[i].Adress);
                worksheet.Cells[i + 1, 4] = new Cell(Parser.Data.DATA[i].Adress2);
                worksheet.Cells[i + 1, 5] = new Cell(Parser.Data.DATA[i].Phone);
            }
            workbook.Worksheets.Add(worksheet);
            workbook.Save(fileName); 
        }
    }
}
