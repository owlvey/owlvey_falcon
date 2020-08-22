using System.Collections.Generic;
using OfficeOpenXml;
using Owlvey.Falcon.Core.Models.Migrate;

namespace Owlvey.Falcon.Builders {
    public class SheetRowAdapter : ISheet {

        private ExcelWorksheet Sheet;
        public SheetRowAdapter(ExcelWorksheet sheet)        
        {
            this.Sheet = sheet;
        }

        public T get<T>(int row, int column)
        {
            return this.Sheet.Cells[row, column].GetValue<T>();
        }

        public int getRows()
        {
            return this.Sheet.Dimension.Rows;
        }
    }

}