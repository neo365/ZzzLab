﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZzzLab.Crypt
{
    /// <summary>
    /// SHA256
    /// </summary>
    public class SHA256Crypt
    {
        /// <summary>
        /// SHA256 Checksum
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>Checksum</returns>
        /// <exception cref="ArgumentNullException">스트림이 null일경우</exception>
        public static string Checksum(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (SHA256 sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }

        /// <summary>
        /// SHA256 Checksum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns>Checksum</returns>
        /// <exception cref="ArgumentNullException">입력문자가 null 또는 공백으로만 되어있을경우</exception>
        public static string Checksum(string value, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            encoding = encoding ?? Encoding.Default;

            using (SHA256 sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(encoding.GetBytes(value))).Replace("-", string.Empty);
            }
        }

        /// <summary>
        /// SHA256 Checksum
        /// </summary>
        /// <param name="filePath">파일경로</param>
        /// <returns>Checksum</returns>
        /// <exception cref="ArgumentNullException">파일 경로가 null일경우</exception>
        /// <exception cref="FileNotFoundException">파일이 없을 경우</exception>
        public static string FileChecksum(string filePath)
        {
            if (string.IsNullOrEmpty(filePath?.Trim())) throw new ArgumentNullException(nameof(filePath));
            if (File.Exists(filePath) == false) throw new FileNotFoundException(filePath);

            using (FileStream stream = File.OpenRead(filePath))
            {
                return SHA256Crypt.Checksum(stream);
            }
        }

        /// <summary>
        /// SHA256 Verify
        /// </summary>
        /// <param name="stream">원본값</param>
        /// <param name="hash">해쉬값</param>
        /// <returns>일치 여부</returns>
        public static bool Verify(Stream stream, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash)) throw new ArgumentNullException(nameof(hash));

            return (0 == StringComparer.OrdinalIgnoreCase.Compare(Checksum(stream), hash));
        }

        /// <summary>
        /// SHA256 Verify
        /// </summary>
        /// <param name="input">원본값</param>
        /// <param name="hash">해쉬값</param>
        /// <param name="encoding"> system.Text.Encoding</param>
        /// <returns>일치 여부</returns>
        public static bool Verify(string input, string hash, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(hash)) throw new ArgumentNullException(nameof(hash));

            return (0 == StringComparer.OrdinalIgnoreCase.Compare(Checksum(input, encoding), hash));
        }

        /// <summary>
        /// SHA256 Verify
        /// </summary>
        /// <param name="filePath">파일경로</param>
        /// <param name="hash">해쉬값</param>
        /// <returns>일치 여부</returns>
        public static bool FileVerify(string filePath, string hash)
            => (0 == StringComparer.OrdinalIgnoreCase.Compare(FileChecksum(filePath), hash));
    }
}