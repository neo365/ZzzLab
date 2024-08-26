using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ZzzLab.Office.Excel
{
    public class ExcelPOI : ExcelBase, IExcel, IDisposable
    {
        public const int SHEET_OFFSET = 0;
        public const int ROW_OFFSET = 0;
        public const int CELL_OFFSET = 0;

        protected virtual IWorkbook Excel { set; get; }

        public override string[] SheetNames
        {
            get
            {
                if (this.Excel == null) return null;

                List<string> SheetNames = new List<string>();

                foreach (ISheet sheet in this.Excel)
                {
                    SheetNames.Add(sheet.SheetName);
                }

                return SheetNames.ToArray();
            }
        }

        public ExcelPOI() : base()
        {
        }

        public override IExcel Create()
        {
            this.Excel = new XSSFWorkbook();

            return this;
        }

        public override IExcel Open(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (File.Exists(filePath) == false) throw new FileNotFoundException(filePath);

            switch (Path.GetExtension(filePath).ToUpper())
            {
                case ".XLS":
                    this.Excel = new HSSFWorkbook(File.OpenRead(filePath));
                    break;

                case ".XLSX":
                case ".XLSM":
                    this.Excel = new XSSFWorkbook(File.OpenRead(filePath));
                    break;

                default:
                    throw new ArgumentException("File is incorrect.");
            }

            this.FilePath = filePath;

            return this;
        }

        public override IExcel Open(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            try
            {
                this.Excel = new XSSFWorkbook(stream);
            }
            catch
            {
                this.Excel = new HSSFWorkbook(stream);
            }

            return this;
        }

        private ISheet FindSheet(string sheetName)
        {
            if (this.Excel == null) return null;

            foreach (ISheet sheet in this.Excel)
            {
                if (sheet.SheetName.EqualsIgnoreCase(sheetName)) return sheet;
            }
            return null;
        }

        private ISheet FindSheet(int sheetIndex)
        {
            if (sheetIndex < 0) throw new InvalidArgumentException("Sheet index starts at 1");
            if (this.Excel == null) return null;

            int index = 1;
            foreach (ISheet sheet in this.Excel)
            {
                if (sheetIndex == index) return sheet;
                index++;
            }

            return null;
        }

        public override int GetSheetIndex(string sheetName)
        {
            if (this.Excel == null) return -1;
            return this.Excel.GetSheetIndex(sheetName);
        }

        public override string GetSheetName(int sheetIndex)
        {
            if (this.Excel == null) return null;
            return this.Excel.GetSheetAt(sheetIndex)?.SheetName;
        }

        private ISheet CreateISheet(string sheetname)
        {
            if (this.Excel == null) throw new InvalidOperationException();

            if (FindSheet(sheetname) != null) throw new DuplicateNameException($"{sheetname}이 이미 존재합니다.");

            return this.Excel.CreateSheet(sheetname);
        }

        public override bool CreateSheet(string sheetName)
            => (CreateISheet(sheetName) != null);

        public override int CopySheet(int sheetIndex, string destSheetName)
            => CopySheet(this.GetSheetName(sheetIndex), destSheetName);

        public override int CopySheet(string sheetName, string destSheetName)
        {
            if (this.Excel == null) return -1;
            ISheet sheet = this.Excel.GetSheet(sheetName);

            if (sheet == null) return -1;

            sheet = sheet.CopySheet(destSheetName);
            return this.Excel.GetSheetIndex(sheet);
        }

        public override void DeleteSheet(int sheetNum) => this.Excel?.RemoveSheetAt(sheetNum);

        public override void DeleteSheet(string sheetName)
        {
            if (Excel == null) throw new InvalidOperationException("Excel is Close");

            DeleteSheet(this.GetSheetIndex(sheetName));
        }

        protected ISheet GetSheet(string sheetname, bool isCreate = false)
        {
            ISheet sheet = FindSheet(sheetname);

            if (sheet == null && isCreate) return this.Excel?.CreateSheet(sheetname);

            return sheet;
        }

        private ISheet GetSheet(int sheetIndex)
            => FindSheet(sheetIndex);

        private static ICell CreateCell(ISheet sheet, int rowposition, int cellposition, object value, int rowsize = 1, int cellsize = 1)
        {
            IRow row = sheet.GetRow(rowposition) ?? sheet.CreateRow(rowposition);
            ICell cell = row.GetCell(cellposition) ?? row.CreateCell(cellposition);

            if (cellsize > 1 || rowsize > 1)
            {
                int cellendposition = cellposition + cellsize - 1;
                int rowendposition = rowposition + rowsize - 1;

                for (int i = rowposition; i <= rowendposition; i++)
                {
                    IRow tmprow = sheet.GetRow(i) ?? sheet.CreateRow(i);

                    for (int j = cellposition; j <= cellendposition; j++)
                    {
                        _ = tmprow.GetCell(j) ?? tmprow.CreateCell(j);
                    }
                }

                sheet.AddMergedRegion(new CellRangeAddress(rowposition, rowendposition, cellposition, cellendposition));
            }

            if (value == null) value = string.Empty;

            switch (value.GetType().ToString())
            {
                case "System.Boolean":
                    cell.SetCellValue(Convert.ToBoolean(value));
                    break;

                case "System.Single":
                case "System.Double":
                case "System.Int":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                    cell.SetCellValue(Convert.ToDouble(value));
                    break;

                case "System.DateTime":
                    cell.SetCellValue(Convert.ToDateTime(value));
                    break;

                case "System.Data.DataColumn":
                case "System.string":
                case "System.String":
                default:
                    // Excel cell Max: 32767
                    // 대강 32760에서 자르자

                    string str = Convert.ToString(value);

                    if (str != null && str.Length > 32760)
                    {
                        str = string.Concat(str.Substring(0, 32760), "...");
                    }

                    cell.SetCellValue(str);
                    break;
            }

#if WRAP_TEXT
            ICellStyle cs = Excel.CreateCellStyle();
            cs.WrapText = true;
            cell.CellStyle = cs;
#endif
            return cell;
        }

        /// <summary>
        /// 셀에 함수을 설정한다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col  (Zero Base)</param>
        /// <param name="value">값</param>
        public override void SetFormula(int sheetIndex, int rowNum, int colNum, string value)
            => CreatCellFormula(this.FindSheet(sheetIndex), rowNum + ROW_OFFSET, colNum + CELL_OFFSET, value);

        /// <summary>
        /// 셀에 함수을 설정한다..
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col  (Zero Base)</param>
        /// <param name="value">값</param>
        public override void SetFormula(string sheetName, int rowNum, int colNum, string value)
            => CreatCellFormula(this.FindSheet(sheetName), rowNum + ROW_OFFSET, colNum + CELL_OFFSET, value);

        /// <summary>
        /// 셀에 함수을 설정한다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        public override void SetFormula(int sheetIndex, string address, string value)
            => SetFormula(this.FindSheet(sheetIndex), address, value);

        /// <summary>
        /// 셀에 함수을 설정한다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        public override void SetFormula(string sheetName, string address, string value)
            => SetFormula(this.FindSheet(sheetName), address, value);

        protected virtual void SetFormula(ISheet sheet, string address, string value)
        {
            CellReference cellref = new CellReference(address);
            int rowposition = cellref.Row;
            int cellposition = cellref.Col;

            CreatCellFormula(sheet, rowposition, cellposition, value);
        }

        protected virtual ICell CreatCellFormula(ISheet sheet, int rowposition, int cellposition, string value, int rowsize = 1, int cellsize = 1)
        {
            IRow row = sheet.GetRow(rowposition) ?? sheet.CreateRow(rowposition);
            ICell cell = row.GetCell(cellposition) ?? row.CreateCell(cellposition);

            if (cellsize > 1 || rowsize > 1)
            {
                int cellendposition = cellposition + cellsize - 1;
                int rowendposition = rowposition + rowsize - 1;

                for (int i = rowposition; i <= rowendposition; i++)
                {
                    IRow tmprow = sheet.GetRow(i) ?? sheet.CreateRow(i);

                    for (int j = cellposition; j <= cellendposition; j++)
                    {
                        _ = tmprow.GetCell(j) ?? tmprow.CreateCell(j);
                    }
                }

                sheet.AddMergedRegion(new CellRangeAddress(rowposition, rowendposition, cellposition, cellendposition));
            }

            value = value ?? value.TrimStart('=');

            if (string.IsNullOrWhiteSpace(value)) cell.SetCellValue(string.Empty);
            else
            {
                cell.SetCellType(CellType.Formula);
                cell.SetCellFormula(value);
            }

#if WRAP_TEXT
            ICellStyle cs = Excel.CreateCellStyle();
            cs.WrapText = true;
            cell.CellStyle = cs;
#endif
            return cell;
        }

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="bytes">이미지 데이터</param>
        /// <param name="imageType">이미지 포멧</param>
        public override void SetImage(int sheetIndex, string address, byte[] bytes, ImageType imageType)
           => SetImage(this.GetSheet(sheetIndex), address, bytes, imageType);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="bytes">이미지 데이터</param>
        /// <param name="imageType">이미지 포멧</param>
        public override void SetImage(string sheetName, string address, byte[] bytes, ImageType imageType)
            => SetImage(this.GetSheet(sheetName), address, bytes, imageType);

        private void SetImage(ISheet sheet, string address, byte[] bytes, ImageType imageType)
        {
            ICreationHelper helper = Excel.GetCreationHelper();
            IDrawing drawing = sheet.CreateDrawingPatriarch();
            IClientAnchor anchor = helper.CreateClientAnchor();

            CellReference cellref = new CellReference(address);
            int rowposition = cellref.Row;
            int cellposition = cellref.Col;

            // 이미지 출력할 cell 위치
            anchor.Row1 = rowposition;
            anchor.Col1 = cellposition;

            // 이미지 그리기
            IPicture picture = drawing.CreatePicture(anchor, this.Excel.AddPicture(bytes, ToPictureType(imageType)));
            picture.Resize(1);
        }

        protected virtual PictureType ToPictureType(ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.Unknown: return PictureType.Unknown;
                case ImageType.None: return PictureType.None;
                case ImageType.EMF: return PictureType.EMF;
                case ImageType.WMF: return PictureType.WMF;
                case ImageType.PICT: return PictureType.PICT;
                case ImageType.JPG: return PictureType.JPEG;
                case ImageType.PNG: return PictureType.PNG;
                case ImageType.DIB: return PictureType.DIB;
                case ImageType.GIF: return PictureType.GIF;
                case ImageType.TIFF: return PictureType.TIFF;
                case ImageType.EPS: return PictureType.EPS;
                case ImageType.BMP: return PictureType.BMP;
                case ImageType.WPG: return PictureType.WPG;
                default: return PictureType.Unknown;
            }
        }

        /// <summary>
        /// 셀의 값을 설정한다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col  (Zero Base)</param>
        /// <param name="value">값</param>
        public override void SetValue(int sheetIndex, int rowNum, int colNum, object value)
            => CreateCell(this.FindSheet(sheetIndex), rowNum + ROW_OFFSET, colNum + CELL_OFFSET, value);

        /// <summary>
        /// 셀의 값을 설정한다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col  (Zero Base)</param>
        /// <param name="value">값</param>
        public override void SetValue(string sheetName, int rowNum, int colNum, object value)
            => CreateCell(this.FindSheet(sheetName), rowNum + ROW_OFFSET, colNum + CELL_OFFSET, value);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        public override void SetValue(int sheetIndex, string address, object value)
            => SetValue(this.FindSheet(sheetIndex), address, value);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        public override void SetValue(string sheetName, string address, object value)
            => SetValue(this.FindSheet(sheetName), address, value);

        protected virtual void SetValue(ISheet sheet, string address, object value)
        {
            CellReference cellref = new CellReference(address);
            int rowposition = cellref.Row;
            int cellposition = cellref.Col;

            CreateCell(sheet, rowposition, cellposition, value);
        }

        public override string GetValue(int sheetIndex, int rowNum, int colNum)
            => GetValue(this.FindSheet(sheetIndex), rowNum + ROW_OFFSET, colNum + CELL_OFFSET);

        public override string GetValue(string sheetName, int rowNum, int colNum)
            => GetValue(this.FindSheet(sheetName), rowNum + ROW_OFFSET, colNum + CELL_OFFSET);

        public override string GetValue(int sheetIndex, string address)
            => GetValue(this.FindSheet(sheetIndex), address);

        public override string GetValue(string sheetName, string address)
            => GetValue(this.FindSheet(sheetName), address);

        protected virtual string GetValue(ISheet sheet, string address)
        {
            if (sheet == null) throw new ArgumentNullException(nameof(sheet));

            CellReference cellref = new CellReference(address);
            int rowposition = cellref.Row;
            int cellposition = cellref.Col;

            return GetValue(sheet, rowposition, cellposition);
        }

        protected virtual string GetValue(ISheet sheet, int rowNum, int colNum)
        {
            if (sheet == null) throw new ArgumentNullException(nameof(sheet));

            IRow row = sheet.GetRow(rowNum);

            if (row == null) return string.Empty;

            ICell cell = row.GetCell(colNum);

            if (cell == null) return string.Empty;

            if (cell.CellType == CellType.Formula) return cell.CellFormula;
            else return cell.StringCellValue;
        }

        public override void RemoveRow(int sheetIndex, int rowNum)
            => RemoveRow(this.FindSheet(sheetIndex), rowNum + ROW_OFFSET);

        public override void RemoveRow(string sheetName, int rowNum)
            => RemoveRow(this.FindSheet(sheetName), rowNum + ROW_OFFSET);

        public override void RemoveRow(int sheetIndex, string address)
            => RemoveRow(this.FindSheet(sheetIndex), new CellReference(address).Row);

        public override void RemoveRow(string sheetName, string address)
            => RemoveRow(this.FindSheet(sheetName), new CellReference(address).Row);

        protected virtual void RemoveRow(ISheet sheet, int rowNum)
        {
            if (sheet == null) throw new ArgumentNullException(nameof(sheet));

            IRow row = sheet.GetRow(rowNum);

            if (row == null) return;

            sheet.RemoveRow(row);
        }

        public override void SetSheet(DataTable datatable, string sheetName, bool hasHeader = true, string address = "A1")
        {
            if (string.IsNullOrWhiteSpace(sheetName)) throw new ArgumentNullException(nameof(sheetName));
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException(nameof(address));

            ISheet sheet = this.GetSheet(sheetName, true) ?? throw new ArgumentOutOfRangeException(nameof(sheetName));
            SetSheet(datatable, sheet, hasHeader, address);
        }

        public ISheet SetSheet(DataTable datatable, ISheet sheet, bool hasHeader = true, string address = "A1")
        {
            if (sheet == null) throw new ArgumentNullException(nameof(sheet));
            if (datatable == null || datatable.Rows.Count == 0) return sheet;

            if (sheet == null)
            {
                string sheetname = string.IsNullOrEmpty(datatable.TableName) ? DateTime.Now.ToString("yyyyMMddhhmmssfff") : datatable.TableName;
                sheet = this.CreateISheet(sheetname);
            }

            CellReference cellref = new CellReference(address);
            int ExcelRowNum = cellref.Row;
            int ExcelColumnNum = cellref.Col;

            if (hasHeader)
            {
                int columnNum = ExcelColumnNum;
                foreach (DataColumn column in datatable.Columns)
                {
                    CreateCell(sheet, ExcelRowNum, columnNum++, column);
                }
                ExcelRowNum++;
            }

            foreach (DataRow datarow in datatable.Rows)
            {
                int columnNum = cellref.Col;

                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    if (datarow.IsNull(i))
                    {
                        columnNum++;
                        continue;
                    }

                    CreateCell(sheet, ExcelRowNum, columnNum++, datarow[i]);
                }
                ExcelRowNum++;
            }

            return sheet;
        }

        public override DataTable ToDataTable(string sheetName, bool hasHeader = true, string address = "A1", int limit = 0)
        {
            if (string.IsNullOrWhiteSpace(sheetName)) throw new ArgumentNullException(nameof(sheetName));

            ISheet sheet = (this.Excel?.GetSheet(sheetName)) ?? throw new ArgumentOutOfRangeException(nameof(sheetName));
            return ToDataTable(sheet, hasHeader, address, limit);
        }

        private static DataTable ToDataTable(ISheet sheet, bool hasHeader = true, string address = "A1", int limit = 0)
        {
            if (sheet == null) throw new ArgumentNullException(nameof(sheet));

            DataTable datatable = new DataTable(sheet.SheetName);

            IEnumerator rows = sheet.GetRowEnumerator();

            CellReference cellref = new CellReference(address);

            int ExcelRowNum = cellref.Row;
            int ExcelColumnNum = cellref.Col;

            if (ExcelRowNum > 0)
            {
                for (int i = 0; i < ExcelRowNum; i++)
                {
                    rows.MoveNext();
                }
            }

            if (hasHeader)
            {
                if (rows.MoveNext())
                {
                    IRow row = (IRow)rows.Current;
                    if (row == null) return datatable;

                    for (int i = ExcelColumnNum; i < row.LastCellNum; i++)
                    {
                        ICell cell = row.GetCell(i);
                        string columnname = (cell?.ToString() ?? "Column" + i).Trim();

                        if (string.IsNullOrWhiteSpace(columnname)) columnname = "Column" + i;

                        if (datatable.Columns.Contains(columnname))
                        {
                            columnname += i;
                        }

                        datatable.Columns.Add(columnname.Trim());
                    }
                }
            }
            else
            {
                IRow row = sheet.GetRow(ExcelRowNum);
                if (row == null) return datatable;

                for (int i = ExcelColumnNum; i < row.LastCellNum; i++)
                {
                    string columnname = CellReference.ConvertNumToColString(i);
                    datatable.Columns.Add(columnname);
                }
            }

            int count = 0;
            while (rows.MoveNext())
            {
                IRow excelrow = (IRow)rows.Current;
                DataRow dataRow = datatable.NewRow();

                for (int i = excelrow.FirstCellNum; i < excelrow.LastCellNum; i++)
                {
                    ICell excelcell = excelrow.GetCell(i);

                    if (excelcell != null)
                    {
                        switch (excelcell.CellType)
                        {
                            case CellType.String:
                                dataRow[i] = excelcell.StringCellValue;
                                break;

                            case CellType.Numeric:
                                if (DateUtil.IsCellInternalDateFormatted(excelcell)) dataRow[i] = excelcell.DateCellValue;
                                else dataRow[i] = excelcell.NumericCellValue;
                                break;

                            case CellType.Boolean:
                                dataRow[i] = excelcell.BooleanCellValue;
                                break;

                            case CellType.Formula:
                                dataRow[i] = excelcell.StringCellValue;
                                break;

                            case CellType.Blank:
                                dataRow[i] = "";
                                break;

                            case CellType.Error:
                                dataRow[i] = excelcell.ErrorCellValue;
                                break;

                            default:
                                dataRow[i] = excelcell.StringCellValue;
                                break;
                        }
                    }
                }

                datatable.Rows.Add(dataRow);

                count++;

                if (limit > 0 && limit <= count) break;
            }

            return datatable;
        }

        private void ReloadHSSFFormula()
        {
            if (this.Excel == null) return;
            foreach (ISheet sheet in this.Excel)
            {
                sheet.ForceFormulaRecalculation = true;
            }
        }

        public override bool Save()
        {
            if (Excel == null) throw new InvalidOperationException("Excel is Close");
            if (string.IsNullOrEmpty(this.FilePath)) throw new ArgumentNullException("FilePath");

            using (FileStream filestream = File.Create(this.FilePath))
            {
                ReloadHSSFFormula();
                this.Excel.Write(filestream, true);
            }

            return true;
        }

        public override bool SaveAs(Stream stream)
        {
            if (Excel == null) throw new InvalidOperationException("Excel is Close");

            ReloadHSSFFormula();
            this.Excel.Write(stream, true);

            return true;
        }

        public override bool SaveAs(string filepath)
        {
            if (Excel == null) throw new InvalidOperationException("Excel is Close");
            if (string.IsNullOrEmpty(filepath)) throw new ArgumentNullException(filepath);

            using (FileStream filestream = File.Create(filepath))
            {
                ReloadHSSFFormula();
                this.Excel.Write(filestream, true);
            }

            return true;
        }

        public override void Close()
        {
            if (this.Excel == null) return;

            this.Excel.Close();
        }

        #region IDisposable Support

        private void Dispose(bool disposing)
        {
            if (this.Excel == null)
            {
                IsDispose = true;
                return;
            }

            if (!IsDispose)
            {
                if (disposing)
                {
                    this.Close();
                    GC.Collect();
                }

                this.IsDispose = true;
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}