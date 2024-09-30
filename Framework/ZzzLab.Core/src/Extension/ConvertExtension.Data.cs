using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using ZzzLab.IO;
using ZzzLab.Json;

namespace ZzzLab
{
    public static partial class ConvertExtension
    {
        #region short

        /// <summary>
        /// Datarow Value To short (int16)
        /// </summary>
        /// <param name="row">The System.Data.Datarow</param>
        /// <param name="columnName">Column Name</param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static short ToShort(this DataRow row, string columnName)
            => ToShortNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        /// <summary>
        /// Datarow Value To short (int16)
        /// </summary>
        /// <param name="row">The System.Data.Datarow</param>
        /// <param name="columnIndex">column Index</param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static short ToShort(this DataRow row, int columnIndex)
            => ToShortNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        /// <summary>
        /// Datarow Value To short (int16)
        /// </summary>
        /// <param name="row">The System.Data.Datarow</param>
        /// <param name="columnName">Column Name</param>
        /// <param name="throwOnError">Whether to throw output</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static short? ToShortNullable(this DataRow row, string columnName, bool throwOnError = true)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch
            {
                return null;
            }

            short? convert()
            {
                if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
                if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

                return ToShortNullable(row[columnName]);
            }
        }

        /// <summary>
        /// Datarow Value To short (int16)
        /// </summary>
        /// <param name="row">The System.Data.Datarow</param>
        /// <param name="columnIndex">column Index</param>
        /// <param name="throwOnError">Whether to throw output</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static short? ToShortNullable(this DataRow row, int columnIndex, bool throwOnError = true)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch
            {
                return null;
            }

            short? convert()
            {
                if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"Max Count : {row.Table.Columns.Count} | {nameof(columnIndex)}:{columnIndex}");

                return ToShortNullable(row[columnIndex]);
            }
        }

        #endregion short

        #region ushort

        /// <summary>
        /// Datarow Value To ushort (uint16)
        /// </summary>
        /// <param name="row">The System.Data.Datarow</param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static ushort ToUShort(this DataRow row, string columnName)
            => ToUShortNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        /// <summary>
        /// Datarow Value To ushort (uint16)
        /// </summary>
        /// <param name="row">The System.Data.Datarow</param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static ushort ToUShort(this DataRow row, int columnIndex)
            => ToUShortNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        /// <summary>
        /// Datarow Value To ushort (uint16)
        /// </summary>
        /// <param name="row">The System.Data.Datarow</param>
        /// <param name="columnName"></param>
        /// <param name="throwOnError">Whether to throw output</param>
        /// <returns></returns>
        public static ushort? ToUShortNullable(this DataRow row, string columnName, bool throwOnError = true)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch
            {
                return null;
            }

            ushort? convert()
            {
                if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
                if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

                return ToUShortNullable(row[columnName]);
            }
        }

        /// <summary>
        /// Datarow Value To ushort (uint16)
        /// </summary>
        /// <param name="row">The System.Data.Datarow</param>
        /// <param name="columnIndex"></param>
        /// <param name="throwOnError">Whether to throw output</param>
        /// <returns></returns>
        public static ushort? ToUShortNullable(this DataRow row, int columnIndex, bool throwOnError = true)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch
            {
                return null;
            }

            ushort? convert()
            {
                if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"Max Count : {row.Table.Columns.Count} | {nameof(columnIndex)}:{columnIndex}");

                return ToUShortNullable(row[columnIndex]);
            }
        }

        #endregion ushort

        #region int

        /// <summary>
        ///  Datarow Value To int (int32)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>

        public static int ToInt(this DataRow row, string columnName)
            => ToIntNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        /// <summary>
        ///  Datarow Value To int (int32)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static int ToInt(this DataRow row, int columnIndex)
            => ToIntNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        /// <summary>
        ///  Datarow Value To int (int32)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="throwOnError">Whether to throw output</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>

        public static int? ToIntNullable(this DataRow row, string columnName, bool throwOnError = true)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch
            {
                return null;
            }

            int? convert()
            {
                if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
                if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

                return ToIntNullable(row[columnName]);
            }
        }

        /// <summary>
        ///  Datarow Value To int (int32)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="throwOnError">Whether to throw output</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int? ToIntNullable(this DataRow row, int columnIndex, bool throwOnError = true)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch
            {
                return null;
            }

            int? convert()
            {
                if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"Max Count : {row.Table.Columns.Count} | {nameof(columnIndex)}:{columnIndex}");

                return ToIntNullable(row[columnIndex]);
            }
        }

        #endregion int

        #region uint

        public static uint ToUInt(this DataRow row, string columnName)
            => ToUIntNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        public static uint ToUInt(this DataRow row, int columnIndex)
            => ToUIntNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        public static uint? ToUIntNullable(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            return ToUIntNullable(row[columnName]);
        }

        public static uint? ToUIntNullable(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"{nameof(columnIndex)}:{columnIndex}");

            return ToUIntNullable(row[columnIndex]);
        }

        #endregion uint

        #region long

        public static long ToLong(this DataRow row, string columnName)
            => ToLongNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        public static long ToLong(this DataRow row, int columnIndex)
            => ToLongNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        public static long? ToLongNullable(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            return ToLongNullable(row[columnName]);
        }

        public static long? ToLongNullable(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"{nameof(columnIndex)}:{columnIndex}");

            return ToLongNullable(row[columnIndex]);
        }

        #endregion long

        #region ulong

        public static ulong ToULong(this DataRow row, string columnName)
            => ToULongNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        public static ulong ToULong(this DataRow row, int columnIndex)
            => ToULongNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        public static ulong? ToULongNullable(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            return ToULongNullable(row[columnName]);
        }

        public static ulong? ToULongNullable(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"{nameof(columnIndex)}:{columnIndex}");

            return ToULongNullable(row[columnIndex]);
        }

        #endregion ulong

        #region decimal

        public static decimal ToDecimal(this DataRow row, string columnName)
            => ToDecimalNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        public static decimal ToDecimal(this DataRow row, int columnIndex)
            => ToDecimalNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        public static decimal? ToDecimalNullable(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            return ToDecimalNullable(row[columnName]);
        }

        public static decimal? ToDecimalNullable(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"{nameof(columnIndex)}:{columnIndex}");

            return ToDecimalNullable(row[columnIndex]);
        }

        #endregion decimal

        #region double

        public static double ToDouble(this DataRow row, string columnName)
            => ToDoubleNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        public static double ToDouble(this DataRow row, int columnIndex)
            => ToDoubleNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        public static double? ToDoubleNullable(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            return ToDoubleNullable(row[columnName]);
        }

        public static double? ToDoubleNullable(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"{nameof(columnIndex)}:{columnIndex}");

            return ToDoubleNullable(row[columnIndex]);
        }

        #endregion double

        #region boolean

        public static bool ToBoolean(this DataRow row, string columnName)
            => ToBooleanNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        public static bool ToBoolean(this DataRow row, int columnIndex)
            => ToBooleanNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        public static bool? ToBooleanNullable(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            return ToBooleanNullable(row[columnName]);
        }

        public static bool? ToBooleanNullable(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"{nameof(columnIndex)}:{columnIndex}");

            return ToBooleanNullable(row[columnIndex]);
        }

        #endregion boolean

        #region datetime

        public static DateTime ToDateTime(this DataRow row, string columnName, bool throwOnError = true)
        {
            if (string.IsNullOrEmpty(columnName?.Trim())) throw new ArgumentNullException(nameof(columnName));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ToDateTime Error:" + ex.ToString());
                return default;
            }

            DateTime convert()
            {
                if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

                object o = row[columnName];

                if (o == null || o == DBNull.Value || o.GetType() == typeof(DBNull)) throw new InvalidCastException();
                return System.Convert.ToDateTime(o);
            }
        }

        public static DateTime ToDateTime(this DataRow row, int columnIndex, bool throwOnError = true)
        {
            if (columnIndex < 0) throw new IndexOutOfRangeException();

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ToDateTime Error:" + ex.ToString());
                return default;
            }

            DateTime convert()
            {
                if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException();

                object o = row[columnIndex];

                if (o == null || o == DBNull.Value || o.GetType() == typeof(DBNull)) throw new InvalidCastException();
                return System.Convert.ToDateTime(o);
            }
        }

        public static DateTime? ToDateTimeNullable(this DataRow row, string columnName, bool throwOnError = true)
        {
            if (string.IsNullOrEmpty(columnName?.Trim())) throw new ArgumentNullException(nameof(columnName));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ToDateTimeNullable Error:" + ex.ToString());
                return default;
            }

            DateTime? convert()
            {
                if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"columnName: {columnName}");

                object o = row[columnName];

                if (o == null || o == DBNull.Value || o.GetType() == typeof(DBNull)) return null;
                return System.Convert.ToDateTime(o);
            }
        }

        public static DateTime? ToDateTimeNullable(this DataRow row, int columnIndex, bool throwOnError = true)
        {
            if (columnIndex < 0) throw new IndexOutOfRangeException();

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ToDateTimeNullable Error:" + ex.ToString());
                return default;
            }

            DateTime? convert()
            {
                if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException();

                object o = row[columnIndex];

                if (o == null || o == DBNull.Value || o.GetType() == typeof(DBNull)) return null;
                return System.Convert.ToDateTime(o);
            }
        }

        #endregion datetime

        #region string

        /// <summary>
        /// Datarow Value To string
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static string ToString(this DataRow row, string columnName)
            => ToStringNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        /// <summary>
        /// Datarow Value To string
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static string ToString(this DataRow row, int columnIndex)
            => ToStringNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        /// <summary>
        /// Datarow Value To string
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string ToStringNullable(this DataRow row, string columnName, bool throwOnError = true)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch
            {
                return null;
            }

            string convert()
            {
                if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
                if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

                object obj = row[columnName];

                if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
                return System.Convert.ToString(obj);
            }
        }

        /// <summary>
        /// Datarow Value To string
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static string ToStringNullable(this DataRow row, int columnIndex, bool throwOnError = true)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (throwOnError) return convert();

            try
            {
                return convert();
            }
            catch
            {
                return null;
            }

            string convert()
            {
                if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"columnIndex: {columnIndex}");

                object obj = row[(int)columnIndex];

                if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
                return System.Convert.ToString(obj);
            }
        }

        #endregion string

        #region object

        public static object ToObject(this DataRow row, string columnName)
            => ToToObjectNullable(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        public static object ToObject(this DataRow row, int columnIndex)
            => ToToObjectNullable(row, columnIndex) ?? throw new InvalidCastException($"columnIndex: {columnIndex}");

        public static object ToToObjectNullable(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrEmpty(columnName?.Trim())) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            object obj = row[columnName];

            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            return obj;
        }

        public static object ToToObjectNullable(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"columnIndex: {columnIndex}");

            object obj = row[(int)columnIndex];

            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            return obj;
        }

        #endregion object

        #region Array

        /// <summary>
        ///
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] ToArray(this DataRow row, string columnName, char separator = ',')
            => row.ToString(columnName).Split(separator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] ToArray(this DataRow row, int columnIndex, char separator = ',')
            => row.ToString(columnIndex).Split(separator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] ToArrayNullable(this DataRow row, string columnName, char separator = ',')
            => row.ToString(columnName)?.Split(separator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] ToArrayNullable(this DataRow row, int columnIndex, char separator = ',')
            => row.ToString(columnIndex)?.Split(separator);

        #endregion Array

        #region enum

        public static T ToEnum<T>(this DataRow row, string columnName)
#if NETSTANDARD2_1
        => ToEnumNullable<T>(row, columnName) ?? throw new InvalidCastException();
#else
        {
            T result = ToEnumNullable<T>(row, columnName);

            return result == null ? throw new InvalidCastException() : result;
        }

#endif

        public static T ToEnum<T>(this DataRow row, int columnIndex)
#if NETSTANDARD2_1
        => ToEnumNullable<T>(row, columnIndex) ?? throw new InvalidCastException();
#else
        {
            T result = ToEnumNullable<T>(row, columnIndex);

            return result == null ? throw new InvalidCastException() : result;
        }

#endif

        public static T ToEnumNullable<T>(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrEmpty(columnName?.Trim())) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            return ToEnumNullable<T>(row[columnName]);
        }

        public static T ToEnumNullable<T>(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException();

            return ToEnumNullable<T>(row[columnIndex]);
        }

        #endregion enum

        #region Guid

        public static Guid ToGuid(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrEmpty(columnName?.Trim())) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            object obj = row[columnName];

            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return Guid.Empty;
            else if (Guid.TryParse(obj.ToString(), out Guid guid)) return guid;

            throw new InvalidCastException();
        }

        public static Guid ToGuid(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException();

            object obj = row[columnIndex];

            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return Guid.Empty;
            else if (Guid.TryParse(obj?.ToString(), out Guid guid)) return guid;

            throw new InvalidCastException();
        }

        public static Guid? ToGuidNullable(this DataRow row, string columnName)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrEmpty(columnName?.Trim())) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            object obj = row[columnName];

            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            else if (Guid.TryParse(obj.ToString(), out Guid guid)) return guid;
            throw new InvalidCastException();
        }

        public static Guid? ToGuidNullable(this DataRow row, int columnIndex)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException();

            object obj = row[columnIndex];

            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            else if (Guid.TryParse(obj.ToString(), out Guid guid)) return guid;
            throw new InvalidCastException();
        }

        #endregion Guid

        #region dynamic

        public static IEnumerable<dynamic> ToDynamic(this DataTable table, bool isLower = true, int limit = 0)
        {
            List<dynamic> list = new List<dynamic>();

            if (table == null || table.Rows.Count == 0) return list.ToArray();

            int processCount = 0;

            foreach (DataRow row in table.Rows)
            {
                processCount++;
                dynamic dyn = new ExpandoObject();
                list.Add(dyn);
                foreach (DataColumn column in table.Columns)
                {
                    IDictionary<string, object> dic = (IDictionary<string, object>)dyn;
                    object value = row[column];
                    dic[(isLower ? column.ColumnName.ToLower() : column.ColumnName)] = (value == DBNull.Value ? null : value);
                }

                if (limit > 0 && processCount >= limit) break;
            }
            return list;
        }

        public static dynamic ToDynamic(this DataRow row, bool isLower = true)
        {
            dynamic dyn = new ExpandoObject();

            if (row == null) return dyn;

            foreach (DataColumn column in row.Table.Columns)
            {
                IDictionary<string, object> dic = (IDictionary<string, object>)dyn;
                dic[(isLower ? column.ColumnName.ToLower() : column.ColumnName)] = row[column];
            }

            return dyn;
        }

        public static dynamic ToModeling(this DataRow row)
        {
            dynamic dyn = new ExpandoObject();

            if (row == null) return dyn;

            foreach (DataColumn column in row.Table.Columns)
            {
                IDictionary<string, object> dic = (IDictionary<string, object>)dyn;
                object value = row[column];
                dic[ToPascalCase(column.ColumnName)] = (value == DBNull.Value ? null : value);
            }

            return dyn;
        }

        public static IEnumerable<dynamic> ToModeling(this DataTable table)
        {
            List<dynamic> list = new List<dynamic>();

            if (table == null || table.Rows.Count == 0) return list.ToArray();

            foreach (DataRow row in table.Rows)
            {
                dynamic dyn = new ExpandoObject();
                list.Add(dyn);
                foreach (DataColumn column in table.Columns)
                {
                    IDictionary<string, object> dic = (IDictionary<string, object>)dyn;
                    object value = row[column];
                    dic[ToPascalCase(column.ColumnName)] = (value == DBNull.Value ? null : value);
                }
            }
            return list;
        }

        #endregion dynamic

        #region json

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static T FromJson<T>(this DataRow row, string columnName) where T : class
            => FromJsonNullable<T>(row, columnName) ?? throw new InvalidCastException($"columnName: {columnName}");

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static T FromJson<T>(this DataRow row, int columnIndex) where T : class
            => FromJsonNullable<T>(row, columnIndex) ?? throw new InvalidCastException();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T FromJsonNullable<T>(this DataRow row, string columnName) where T : class
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (string.IsNullOrWhiteSpace(columnName)) throw new ArgumentNullException(nameof(columnName));
            if (row.Table.Columns.Contains(columnName) == false) throw new ArgumentOutOfRangeException($"{nameof(columnName)}: {columnName}");

            string json = row.ToStringNullable(columnName);
            if (string.IsNullOrWhiteSpace(json)) return null;
            if (json.StartsWith("{") == false) return null;

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T FromJsonNullable<T>(this DataRow row, int columnIndex) where T : class
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Table.Columns.Count <= columnIndex) throw new IndexOutOfRangeException($"columnIndex: {columnIndex}");

            string json = row.ToStringNullable(columnIndex);
            if (string.IsNullOrWhiteSpace(json)) return null;
            if (json.StartsWith("{") == false) return null;

            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion json

        #region CSV

        /// <summary>
        /// DataTable To CSV File
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath"></param>
        /// <param name="dimiter"></param>
        /// <param name="hasHeader"></param>
        /// <param name="isOverwrite"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OperationFailedException"></exception>
        public static bool ToCSV(this DataTable table, string filePath, string dimiter = "|", bool hasHeader = false, bool isOverwrite = true)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            // 경로가 비어있으면 에러처리
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            // Overwrite가 아닌데 파일이 있는 경우
            if (isOverwrite == false && File.Exists(filePath)) throw new FileFoundException("File Exist.", nameof(filePath));

            // 폴더가 없으면 만들자.
            if (Directory.Exists(Path.GetDirectoryName(filePath)) == false)
            {
                string dirPath = Path.GetDirectoryName(filePath);
                if (dirPath == null) throw new OperationFailedException(nameof(dirPath));
                Directory.CreateDirectory(dirPath);
            }

            StringBuilder sb = new StringBuilder();

            if (hasHeader == true)
            {
                string[] columnNames = table.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                sb.AppendLine(string.Join(dimiter, columnNames));
            }

            foreach (DataRow row in table.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field?.ToString()).ToArray();
                sb.AppendLine(string.Join(dimiter, fields));
            }

            if (File.Exists(filePath)) File.Delete(filePath);

            File.WriteAllText(filePath, sb.ToString());

            return true;
        }

        #endregion CSV
    }
}