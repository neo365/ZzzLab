using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZzzLab
{
    public static partial class ConvertExtension
    {
        #region short

        public static short ToShort(this object obj)
            => ToShortNullable(obj) ?? throw new InvalidCastException();

        public static short? ToShortNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (short.TryParse(obj.ToString(), out short result)) return result;

            return null;
        }

        #endregion short

        #region ushort

        public static ushort ToUShort(this object obj)
            => ToUShortNullable(obj) ?? throw new InvalidCastException();

        public static ushort? ToUShortNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (ushort.TryParse(obj.ToString(), out ushort result)) return result;

            return null;
        }

        #endregion ushort

        #region Int

        public static int ToInt(this object obj)
            => ToIntNullable(obj) ?? throw new InvalidCastException();

        public static int? ToIntNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (int.TryParse(obj.ToString(), out int result) == true) return result;
            throw new InvalidCastException();
        }

        #endregion Int

        #region Uint

        public static uint ToUInt(this object obj)
            => ToUIntNullable(obj) ?? throw new InvalidCastException();

        public static uint? ToUIntNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (uint.TryParse(obj.ToString(), out uint result) == true) return result;
            return null;
        }

        #endregion Uint

        #region long

        public static long ToLong(this object obj)
            => ToLongNullable(obj) ?? throw new InvalidCastException();

        public static long? ToLongNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (long.TryParse(obj.ToString(), out long result)) return result;

            return null;
        }

        #endregion long

        #region ulong

        public static ulong ToULong(this object obj)
            => ToULongNullable(obj) ?? throw new InvalidCastException();

        public static ulong? ToULongNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (ulong.TryParse(obj.ToString(), out ulong result)) return result;

            return null;
        }

        #endregion ulong

        #region decimal

        public static decimal ToDecimal(this object obj)
            => ToDecimalNullable(obj) ?? throw new InvalidCastException();

        public static decimal? ToDecimalNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (decimal.TryParse(obj.ToString(), out decimal result)) return result;

            return null;
        }

        #endregion decimal

        #region double

        public static double ToDouble(this object obj)
            => ToDoubleNullable(obj) ?? throw new InvalidCastException();

        public static double? ToDoubleNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (double.TryParse(obj.ToString(), out double result)) return result;

            return null;
        }

        #endregion double

        #region bool

        public static bool ToBoolean(this object obj)
            => ToBooleanNullable(obj) ?? throw new InvalidCastException();

        public static bool? ToBooleanNullable(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return null;
            if (obj is bool) return System.Convert.ToBoolean(obj);

            string str = obj.ToString();
            if (string.IsNullOrWhiteSpace(str)) return null;
            else if (bool.TryParse(str, out bool rValue)) return rValue;
            else if (str.EqualsOrIgnoreCase("Y", "YES", "TRUE", "1")) return true;
            else if (str.EqualsOrIgnoreCase("N", "NO", "FALSE", "0")) return false;

            return null;
        }

        #endregion bool

        #region string

        public static string ToString(this char[] array) => new string(array);

        #endregion string

        #region Enum

        public static T ToEnum<T>(this object obj)
#if NETSTANDARD2_1
        => ToEnumNullable<T>(obj) ?? throw new InvalidCastException();
#else
        {
            T result = ToEnumNullable<T>(obj);

            if (result == null) throw new InvalidCastException();

            return result;
        }

#endif

        public static T ToEnumNullable<T>(this object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.GetType() == typeof(DBNull)) return default;

            if (obj is string str) return (T)Enum.Parse(typeof(T), str);
            else if (obj is int) return (T)obj;

            return default;
        }

        #endregion Enum

        #region string <-> byte

        public static byte[] ToByte(this string s)
            => ToByte(s, Encoding.Default);

        public static byte[] ToByte(this string s, Encoding encoding)
            => (encoding ?? Encoding.Default).GetBytes(s);

        public static string ToString(this byte[] bytes)
            => ToString(bytes, Encoding.Default);

        public static string ToString(this byte[] bytes, Encoding encoding)
            => (encoding ?? Encoding.Default).GetString(bytes);

        #endregion string <-> byte

        #region byte

        //public static byte[] ToBytes(this Stream stream)
        //{
        //    using (BinaryReader br = new BinaryReader(stream))
        //    {
        //        byte[] bytes = br.ReadBytes((int)stream.Length);
        //        stream.Position = 0;

        //        return bytes;
        //    }
        //}

        public static byte[] ToBytes(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        #endregion byte

        #region Extra

        public static string ToYN(this bool b) => (b ? "Y" : "N");

        public static string ToPassFail(this bool b) => (b ? "Pass" : "Fail");

        public static char[] SubChar(this char[] array, int index, int length)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if ((length - index) > array.Length) length = array.Length - index;

            char[] result = new char[length];
            Array.Copy(array, index, result, 0, length);
            return array;
        }

        public static T[] SubArray<T>(this T[] array, int index, int length)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if ((length - index) > array.Length) length = array.Length - index;

            T[] result = new T[length];
            Array.Copy(array, index, result, 0, length);
            return result;
        }

        #endregion Extra

        #region Hex Convert

        public static byte[] HexToByte(this string hex)
        {
            // 공백은 몽땅 제거하자.
            hex = hex.Remove(" ");

            byte[] bytes = new byte[hex.Length / 2];
            try
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = System.Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw new InvalidCastException("hex is not a valid hex number!");
            }
        }

        public static string HexToString(this string hex, Encoding encoding)
        {
            encoding = encoding ?? Encoding.Default;
            byte[] bytes = hex.HexToByte();

            if (bytes != null && bytes.Length > 0)
            {
                // 0x00 를 변환하면 에러가 발생하니 공백 (0x20)으로 변환한다.
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (bytes[i] == 0x00) bytes[i] = 0x20;
                }

                return encoding.GetString(bytes);
            }
            else return string.Empty;
        }

        public static string ToAsciiHex(this string input)
        {
            string output = string.Empty;

            char[] values = input.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                // Convert the decimal value to a hexadecimal value in string form.
                output += string.Format("{0:X}", System.Convert.ToInt32(letter));
            }

            return output;
        }

        public static string ToAsciiHex(this byte[] bytes, int limit = 0)
        {
            StringBuilder sb = new StringBuilder();

            if (bytes != null)
            {
                int maxLength = (limit <= 0 ? bytes.Length : limit);

                for (int i = 0; i < maxLength; i++)
                {
                    sb.AppendFormat("{0:X2}", bytes[i]);
                }
            }
            return sb.ToString();
        }

        #endregion Hex Convert

        #region GUID

        public static Guid ToGuid(this string guid) => Guid.Parse(guid);

        public static int ToInt(this Guid guid)
        {
            byte[] _bytes = guid.ToByteArray();
            return ((int)_bytes[0]) | ((int)_bytes[1] << 8) | ((int)_bytes[2] << 16) | ((int)_bytes[3] << 24);
        }

        public static long ToLong(this Guid guid)
        {
            byte[] _bytes = guid.ToByteArray();
            return ((long)_bytes[0]) | ((long)_bytes[1] << 8) | ((long)_bytes[2] << 16) | ((long)_bytes[3] << 24);
        }

        public static Guid ToGuid(this int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        #endregion GUID

        public static long ToFileTime(this string text)
        {
            if (text.StartsWith("0001")) text = string.Concat("2001", text.Substring(4));
            return (DateTime.TryParse(text, out DateTime dt) ? dt.ToFileTime() : 0L);
        }

        #region UNIX <=> DOS 포멧변환

        /// <summary>
        /// Unix 포멧의 파일을 Dos 포멧의 파일로 바꾼다.
        /// </summary>
        /// <param name="filePath"></param>
        public static void UnixToDos(string filePath)
        {
            const byte CR = 0x0D;
            const byte LF = 0x0A;
            byte[] DOS_LINE_ENDING = new byte[] { CR, LF };
            byte[] data = File.ReadAllBytes(filePath);
            using (FileStream fileStream = File.OpenWrite(filePath))
            {
                using (BinaryWriter bw = new BinaryWriter(fileStream))
                {
                    int position = 0;
                    int index = 0;
                    do
                    {
                        index = Array.IndexOf<byte>(data, LF, position);
                        if (index >= 0)
                        {
                            if ((index > 0) && (data[index - 1] == CR))
                            {
                                // already dos ending
                                bw.Write(data, position, index - position + 1);
                            }
                            else
                            {
                                bw.Write(data, position, index - position);
                                bw.Write(DOS_LINE_ENDING);
                            }
                            position = index + 1;
                        }
                    }
                    while (index > 0);
                    bw.Write(data, position, data.Length - position);
                    fileStream.SetLength(fileStream.Position);
                }
            }
        }

        /// <summary>
        /// Dos 포멧의 파일을 Unix 포멧의 파일로 바꾼다.
        /// </summary>
        /// <param name="filePath"></param>
        public static void DosToUnix(string filePath)
        {
            const byte CR = 0x0D;
            const byte LF = 0x0A;
            byte[] data = File.ReadAllBytes(filePath);
            using (FileStream fileStream = File.OpenWrite(filePath))
            {
                using (BinaryWriter bw = new BinaryWriter(fileStream))
                {
                    int position = 0;
                    int index = 0;
                    do
                    {
                        index = Array.IndexOf<byte>(data, CR, position);
                        if ((index >= 0) && (data[index + 1] == LF))
                        {
                            // Write before the CR
                            bw.Write(data, position, index - position);
                            // from LF
                            position = index + 1;
                        }
                    }
                    while (index > 0);
                    bw.Write(data, position, data.Length - position);
                    fileStream.SetLength(fileStream.Position);
                }
            }
        }

        #endregion UNIX <=> DOS 포멧변환

        public static dynamic ToDynamic(this IDictionary<string, object> dictionary)
            => dictionary.Aggregate(new ExpandoObject() as IDictionary<string, object>, (a, p) => { a.Add(p); return a; });

        #region IEnumerable

        public static IEnumerable<T> ToIEnumerable<T>(this T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            IList<T> list = new List<T>(1) { value };

            return list.ToArray<T>();
        }

        public static IEnumerable<T> ToIEnumerable<T>(params T[] collection)
        {
            if (collection == null || collection.Any() == false) return Enumerable.Empty<T>();

            List<T> list = new List<T>(collection.Length);
            list.AddRange(collection);
            return list.ToArray<T>();
        }

        #endregion IEnumerable
    }
}