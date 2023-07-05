using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ZzzLab.IO;

namespace ZzzLab.Office.Excel
{
    public abstract class ExcelBase : IExcel, IDisposable
    {
        public string FilePath { protected set; get; }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(this.FilePath?.Trim())) return null;

                return Path.GetFileName(this.FilePath);
            }
        }

        public virtual string[] SheetNames { get; }

        public abstract IExcel Create();

        public abstract IExcel Open(string filePath);

        public abstract IExcel Open(Stream stream);

        public abstract int GetSheetIndex(string sheetName);

        public abstract string GetSheetName(int sheetIndex);

        public abstract bool CreateSheet(string sheetName);

        public abstract int CopySheet(int sheetIndex, string destSheetName);

        public abstract int CopySheet(string SourceSheetName, string destSheetName);

        public virtual void DeleteSheet(string sheetName) => DeleteSheet(GetSheetIndex(sheetName));

        public abstract void DeleteSheet(int sheetIndex);

        public void SetSheet(DataTable datatable, bool hasHeader = true, string address = "A1")
            => SetSheet(datatable, datatable.TableName, hasHeader, address);

        public abstract void SetSheet(DataTable datatable, string sheetName, bool hasHeader = true, string address = "A1");

        public void SetSheet(DataTable datatable, int sheetIndex, bool hasHeader = true, string address = "A1")
        {
            string sheetName = GetSheetName(sheetIndex);
            if (string.IsNullOrWhiteSpace(sheetName)) throw new ArgumentOutOfRangeException(nameof(sheetIndex));

            SetSheet(datatable, sheetName, hasHeader, address);
        }

        public DataTable ToDataTable(int sheetIndex = 0, bool hasHeader = true, string address = "A1", int limit = 0)
        {
            string sheetName = GetSheetName(sheetIndex);
            if (string.IsNullOrWhiteSpace(sheetName)) throw new ArgumentOutOfRangeException(nameof(sheetIndex));

            return ToDataTable(sheetName, hasHeader, address, limit);
        }

        public abstract DataTable ToDataTable(string sheetName, bool hasHeader = true, string address = "A1", int limit = 0);

        public bool ToCSV(int sheetIndex, string filePath, int limit = 0, bool hasHeader = false, string dimiter = ",", string address = "A1")
        {
            // 경로가 비어있으면 에러처리
            if (string.IsNullOrEmpty(filePath?.Trim())) throw new ArgumentNullException(nameof(filePath));

            DataTable table = this.ToDataTable(sheetIndex, hasHeader, address, limit) ?? throw new InvalidDataException("Convert Fail!");

            // 폴더가 없으면 만들자.
            if (Directory.Exists(Path.GetDirectoryName(filePath)) == false)
            {
                PathUtils.CheckDirectory(Path.GetDirectoryName(filePath), true);
            }

            StringBuilder sb = new StringBuilder();

            if (hasHeader == true)
            {
                string[] columnNames = table.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).
                                                  ToArray();
                sb.AppendLine(string.Join(dimiter, columnNames));
            }

            foreach (DataRow row in table.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field?.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(dimiter, fields));
            }

            File.WriteAllText(filePath, sb.ToString());

            return true;
        }

        public abstract bool SaveAs(Stream stream);

        public MemoryStream ToStream()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                this.SaveAs(ms);
                //ms.Position = 0;
                //ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
            catch { throw; }
            finally { }
        }

        public byte[] ToArray()
        {
            try
            {
                using (MemoryStream ms = this.ToStream())
                {
                    return ms.ToArray();
                }
            }
            catch { throw; }
            finally { }
        }

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetIndex">sheet Index</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="filePath">이미지 경로</param>
        public virtual void SetImage(int sheetIndex, string address, string filePath)
            => SetImage(sheetIndex, address, File.ReadAllBytes(filePath), filePath.ToImageType());

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="filePath">이미지 경로</param>
        public virtual void SetImage(string sheetName, string address, string filePath)
            => SetImage(sheetName, address, File.ReadAllBytes(filePath), filePath.ToImageType());

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="bytes">이미지 데이터</param>
        /// <param name="imageType">이미지 포멧</param>
        public abstract void SetImage(int sheetIndex, string address, byte[] bytes, ImageType imageType);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="bytes">이미지 데이터</param>
        /// <param name="imageType">이미지 포멧</param>
        public abstract void SetImage(string sheetName, string address, byte[] bytes, ImageType imageType);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="stream">이미지 stream</param>
        /// <param name="imageType">이미지 포멧</param>
        public virtual void SetImage(int sheetIndex, string address, Stream stream, ImageType imageType)
            => SetImage(sheetIndex, address, (byte[])stream.ToBytes(), imageType);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="stream">이미지 stream</param>
        /// <param name="imageType">이미지 포멧</param>
        public virtual void SetImage(string sheetName, string address, Stream stream, ImageType imageType)
            => SetImage(sheetName, address, stream.ToBytes(), imageType);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        public abstract void SetValue(int sheetIndex, string address, object value);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        public abstract void SetValue(string sheetName, string address, object value);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <returns>셀값</returns>
        public abstract string GetValue(int sheetIndex, string address);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <returns>셀값</returns>
        public abstract string GetValue(string sheetName, string address);

        public abstract bool Save();

        public abstract bool SaveAs(string filepath);

        public abstract void Close();

        protected bool IsDispose = false;

        public abstract void Dispose();
    }
}