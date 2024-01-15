using System;
using System.Data;
using System.IO;

namespace ZzzLab.Office.Excel
{
    public interface IExcel : IDisposable
    {
        /// <summary>
        /// 파일경로. 파일에서 읽어 들일경우 사용한다.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// 파일명. 파일에서 읽어 들일경우 사용한다.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// 엑셀시트 목록
        /// </summary>
        string[] SheetNames { get; }

        /// <summary>
        /// 엑셀생성. 기본값은 xlsx로 생성한다. 메모리상에서 생성된다.
        /// </summary>
        /// <returns></returns>
        IExcel Create();

        IExcel Open(string filePath);

        IExcel Open(Stream stream);

        int GetSheetIndex(string name);

        string GetSheetName(int sheetIndex);

        bool CreateSheet(string sheetName);

        int CopySheet(int sheetIndex, string destSheetName);

        int CopySheet(string SourceSheetName, string destSheetName);

        void DeleteSheet(int sheetIndex);

        void DeleteSheet(string sheetName);

        void SetSheet(DataTable datatable, int sheetIndex, bool hasHeader = true, string address = "A1");

        void SetSheet(DataTable datatable, string sheetName, bool hasHeader = true, string address = "A1");

        DataTable ToDataTable(int sheetIndex, bool hasHeader = true, string address = "A1", int limit = 0);

        DataTable ToDataTable(string sheetName, bool hasHeader = true, string address = "A1", int limit = 0);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetIndex">sheet Index</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="filePath">이미지 경로</param>
        void SetImage(int sheetIndex, string address, string filePath);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="filePath">이미지 경로</param>
        void SetImage(string sheetName, string address, string filePath);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="bytes">이미지 데이터</param>
        /// <param name="imageType">이미지 포멧</param>
        void SetImage(int sheetIndex, string address, byte[] bytes, ImageType imageType);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="bytes">이미지 데이터</param>
        /// <param name="imageType">이미지 포멧</param>
        void SetImage(string sheetName, string address, byte[] bytes, ImageType imageType);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="stream">이미지 stream</param>
        /// <param name="imageType">이미지 포멧</param>
        void SetImage(int sheetIndex, string address, Stream stream, ImageType imageType);

        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="stream">이미지 stream</param>
        /// <param name="imageType">이미지 포멧</param>
        void SetImage(string sheetName, string address, Stream stream, ImageType imageType);

        /// <summary>
        /// 셀의 값을 설정한다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col  (Zero Base)</param>
        /// <param name="value">값</param>
        void SetValue(int sheetIndex, int rowNum, int colNum, object value);

        /// <summary>
        /// 셀의 값을 설정한다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col  (Zero Base)</param>
        /// <param name="value">값</param>
        void SetValue(string sheetName, int rowNum, int colNum, object value);

        /// <summary>
        /// 셀의 값을 설정한다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        void SetValue(int sheetIndex, string address, object value);

        /// <summary>
        /// 셀의 값을 설정한다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        void SetValue(string sheetName, string address, object value);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col (Zero Base)</param>
        /// <returns></returns>
        string GetValue(int sheetIndex, int rowNum, int colNum);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col (Zero Base)</param>
        /// <returns></returns>
        string GetValue(string sheetName, int rowNum, int colNum);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <returns>셀값</returns>
        string GetValue(int sheetIndex, string address);

        /// <summary>
        /// 셀의 값을 가져온다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <returns>셀값</returns>
        string GetValue(string sheetName, string address);

        /// <summary>
        /// 셀에 함수을 설정한다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col  (Zero Base)</param>
        /// <param name="value">값</param>
        void SetFormula(int sheetIndex, int rowNum, int colNum, string value);

        /// <summary>
        /// 셀에 함수을 설정한다..
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="rowNum">row (Zero Base)</param>
        /// <param name="colNum">col  (Zero Base)</param>
        /// <param name="value">값</param>
        void SetFormula(string sheetName, int rowNum, int colNum, string value);

        /// <summary>
        /// 셀에 함수을 설정한다.
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        void SetFormula(int sheetIndex, string address, string value);

        /// <summary>
        /// 셀에 함수을 설정한다.
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        /// <param name="value">값</param>
        void SetFormula(string sheetName, string address, string value);

        /// <summary>
        /// 줄을 삭제한다
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="rowNum">row (Zero Base)</param>
        void RemoveRow(int sheetIndex, int rowNum);

        /// <summary>
        /// 줄을 삭제한다
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="rowNum">row (Zero Base)</param>
        void RemoveRow(string sheetName, int rowNum);

        /// <summary>
        /// 줄을 삭제한다
        /// </summary>
        /// <param name="sheetIndex">sheetIndex(Zero Base)</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        void RemoveRow(int sheetIndex, string address);

        /// <summary>
        /// 줄을 삭제한다
        /// </summary>
        /// <param name="sheetName">시트명</param>
        /// <param name="address">엑셀 Reference 주소. ex) A1</param>
        void RemoveRow(string sheetName, string address);

        bool Save();

        bool SaveAs(string filepath);

        bool SaveAs(Stream stream);

        MemoryStream ToStream();

        void Close();
    }
}