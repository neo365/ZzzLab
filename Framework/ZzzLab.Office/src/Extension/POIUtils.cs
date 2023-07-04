using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace ZzzLab.Office.Excel
{
    public static class POIUtils
    {
        public const int ROW_OFFSET = -1;
        public const int CELL_OFFSET = -1;

        public static byte[] ToRGBArray(this System.Drawing.Color color)
            => new byte[3] { color.R, color.G, color.B };

        public static CellReference ToRef(this string address)
            => new CellReference(address);

        public static string ToAddress(int rowNum, int colNum)
        {
            CellReference cellRef = new CellReference(rowNum + ROW_OFFSET, colNum + CELL_OFFSET);

            return $"{cellRef.CellRefParts[2]}{cellRef.CellRefParts[1]}";
        }

        public static ICellStyle GetStyle(this ICell cell)
            => cell.CellStyle;

        public static ICell CopyStyle(this ICell source, ICell target)
        {
            ICellStyle style = source.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(source.CellStyle);
            target.CellStyle = style;
            return source;
        }

        #region FillColor

        public static ICell SetFillColor(this ICell cell, System.Drawing.Color color)
        {
            ICellStyle style = cell.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);

            style.FillPattern = FillPattern.SolidForeground;
            ((XSSFCellStyle)style).SetFillForegroundColor(new XSSFColor(color.ToRGBArray()));

            cell.CellStyle = style;
            return cell;
        }

        #endregion FillColor

        #region align

        public static ICell SetAlign(this ICell cell, HorizontalAlignment align = HorizontalAlignment.Center)
        {
            ICellStyle style = cell.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);

            style.Alignment = align;

            cell.CellStyle = style;
            return cell;
        }

        public static ICell SetValign(this ICell cell, VerticalAlignment valign = VerticalAlignment.Center)
        {
            ICellStyle style = cell.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);

            style.VerticalAlignment = valign;

            cell.CellStyle = style;
            return cell;
        }

        #endregion align

        #region Border

        public static ICell SetBorder(this ICell cell, BorderStyle border, System.Drawing.Color? color = null)
            => cell.SetBorderTop(border, color)
                   .SetBorderBottom(border, color)
                   .SetBorderLeft(border, color)
                   .SetBorderRight(border, color);

        public static ICell SetBorderTop(this ICell cell, BorderStyle border, System.Drawing.Color? color = null)
        {
            ICellStyle style = cell.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);

            style.BorderTop = border;
            if (color != null) ((XSSFCellStyle)style).SetTopBorderColor(new XSSFColor(color?.ToRGBArray()));
            cell.CellStyle = style;
            return cell;
        }

        public static ICell SetBorderBottom(this ICell cell, BorderStyle border, System.Drawing.Color? color = null)
        {
            ICellStyle style = cell.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);

            style.BorderBottom = border;
            if (color != null) ((XSSFCellStyle)style).SetBottomBorderColor(new XSSFColor(color?.ToRGBArray()));
            cell.CellStyle = style;
            return cell;
        }

        public static ICell SetBorderLeft(this ICell cell, BorderStyle border, System.Drawing.Color? color = null)
        {
            ICellStyle style = cell.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);

            style.BorderLeft = border;
            if (color != null) ((XSSFCellStyle)style).SetLeftBorderColor(new XSSFColor(color?.ToRGBArray()));
            cell.CellStyle = style;
            return cell;
        }

        public static ICell SetBorderRight(this ICell cell, BorderStyle border, System.Drawing.Color? color = null)
        {
            ICellStyle style = cell.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);

            style.BorderRight = border;
            if (color != null) ((XSSFCellStyle)style).SetRightBorderColor(new XSSFColor(color?.ToRGBArray()));
            cell.CellStyle = style;
            return cell;
        }

        #endregion Border

        public static ICell SetFont(this ICell cell,
            string fontName = null,
            System.Drawing.Color? color = null,
            double heightPoint = 0D)
        {
            ICellStyle style = cell.Row.Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);

            IFont font = cell.Row.Sheet.Workbook.CreateFont();

            if (color != null) ((XSSFFont)font).SetColor(new XSSFColor(color?.ToRGBArray()));
            if (heightPoint > 0) font.FontHeightInPoints = heightPoint;
            if (string.IsNullOrWhiteSpace(fontName) == false) font.FontName = fontName;

            style.SetFont(font);

            cell.Row.HeightInPoints = 25f;

            cell.CellStyle = style;
            return cell;
        }

        public static ICell SetHeight(this ICell cell, float heightPoint)
        {
            cell.Row.HeightInPoints = heightPoint;
            return cell;
        }

        public static ICell SetWidth(this ICell cell, int width)
        {
            cell.Row.Sheet.SetColumnWidth(cell.ColumnIndex, width);
            return cell;
        }
    }
}
