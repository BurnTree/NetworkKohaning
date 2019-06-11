using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace KohaningNeuralNetwork.component
{
    class Excel
    {
        string path = " ";
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;

        public Excel() { }

        public Excel(string path, int Sheet)
        {
            this.path = path;
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[Sheet];
        }

        public string read(int i, int j)
        {
            if (ws.Cells[i, j].Value2 != null)
                return Convert.ToString(ws.Cells[i, j].Value2);
            else
                return "";
        }

        public void write(int i, int j, string s)
        {
            i++;
            j++;
            ws.Cells[i, j].Value2 = s;
        }
        public void save()
        {
            wb.Save();
        }
        public void saveAs(string path)
        {
            wb.SaveAs(path);
        }
        public void close()
        {
            wb.Close();
        }

        public void createNewFile()
        {
            this.wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            this.ws = wb.Worksheets[1];
        }

        public void createNewSheet()
        {
            Worksheet tempSheet = wb.Worksheets.Add(After: ws);
        }
    }
}
