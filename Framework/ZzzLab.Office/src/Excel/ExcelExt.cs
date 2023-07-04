using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;

namespace ZzzLab.Office.Excel
{
    public class ExcelExt : ExcelPOI
    {
        public ExcelExt() : base()
        {
        }

        public virtual void SetValue(string sheetName, int rowNum, int colNum, object value)
            => SetValue(sheetName,POIUtils.ToAddress(rowNum, colNum), value);

        public virtual string GetValue(string sheetName, int rowNum, int colNum)
            => GetValue(sheetName, POIUtils.ToAddress(rowNum, colNum));

        public virtual void RemoveRow(string sheetName, int rowNum)
        {
            if (string.IsNullOrWhiteSpace(sheetName)) throw new ArgumentNullException(nameof(sheetName));

            ISheet sheet = this.GetSheet(sheetName);

            CellReference cellref = new CellReference($"A{rowNum}");
            int rowposition = cellref.Row;
            int cellposition = cellref.Col;

            IRow row = sheet.GetRow(rowposition);

            sheet.RemoveRow(row);
        }

        public ICell GetCell(ISheet sheet, string address)
        {
            CellReference cellRef = address.ToRef();
            return sheet.GetRow(cellRef.Row).GetCell(cellRef.Col);
        }

        public ICell GetCell(string seetName, string address)
            => GetCell(this.GetSheet(seetName), address);

        public ICell GetCell(string seetName, int rowNum, int colNum)
            => this.GetSheet(seetName).GetRow(rowNum + ROW_OFFSET).GetCell(colNum + CELL_OFFSET);

        public ICell MergeCell(ISheet sheet, string startAddress, string endAddress)
        {
            CellReference startRef = startAddress.ToRef();
            CellReference endRef = endAddress.ToRef();

            CellRangeAddress range = new CellRangeAddress(
                startRef.Row,
                endRef.Row,
                startRef.Col,
                endRef.Col);

            sheet.AddMergedRegion(range);

            return GetCell(sheet, startAddress);
        }

        public ICell MergeCell(string sheetName, string startAddress, string endAddress)
            => MergeCell(this.GetSheet(sheetName), startAddress, endAddress);
    }
}