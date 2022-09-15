using System.Text;
using ZzzLab;
using ZzzLab.Crypt;
using ZzzLab.Json;
using ZzzLab.Logging;

namespace ConsoleSample
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("== Console Test Start ==============");

            try
            {
#if false
                using (Bitmap source = (Bitmap)Image.FromFile(@"c:\temp\com\1.jpg"))
                {
                    using (Bitmap dest = (Bitmap)Image.FromFile(@"c:\temp\com\2.jpg"))
                    {
                        var stopwatch = Stopwatch.StartNew(); // 시간측정 시작

                        using (Bitmap diff = source.ImageDiff(dest))
                        {
                            Console.WriteLine($"yddiff : {stopwatch.GetElapsedTime()}");
                            diff.Save(@"c:\temp\com\diff_01.png", ImageFormat.Png);
                        }

                        stopwatch = Stopwatch.StartNew(); // 시간측정 시작
                        using (Bitmap diff = source.ImageDiff(dest, Color.Red))
                        {
                            Console.WriteLine($"yddiff : {stopwatch.GetElapsedTime()}");
                            diff.Save(@"c:\temp\com\diff_02.png", ImageFormat.Png);

                            stopwatch = Stopwatch.StartNew(); // 시간측정 시작
                            using (Bitmap m = source.ImageMerge(diff))
                            {
                                Console.WriteLine($"merge : {stopwatch.GetElapsedTime()}");
                                m.Save(@"c:\temp\com\merge_02.png", ImageFormat.Png);
                            }
                        }

                        stopwatch = Stopwatch.StartNew(); // 시간측정 시작
                        using (Bitmap diff = source.ImageDiff(dest, Color.Red, true))
                        {
                            Console.WriteLine($"yddiff : {stopwatch.GetElapsedTime()}");
                            diff.Save(@"c:\temp\com\diff_03.png", ImageFormat.Png);
                        }
                    }
                }

                using (Mat source = Cv2.ImRead(@"c:\temp\com\1.jpg"))
                {
                    using (Mat dest = Cv2.ImRead(@"c:\temp\com\2.jpg"))
                    {
                        using (Mat absDiffImage = new Mat())
                        {
                            var stopwatch = Stopwatch.StartNew(); // 시간측정 시작

                            Cv2.Absdiff(source, dest, absDiffImage);

                            Console.WriteLine($"Absdiff : {stopwatch.GetElapsedTime()}");

                            using (var ms = absDiffImage.ToMemoryStream())
                            {
                                using (Bitmap bitmap = (Bitmap)Image.FromStream(ms))
                                {
                                    bitmap.MakeTransparent(System.Drawing.Color.Black);
                                    bitmap.Save(@"c:\temp\com\diff_cv.png");
                                }
                            }
                        }
                    }
                }
#endif

                //ZzzLab.IO.PathUtils.GetAppDataPath();
                ILogger logger = new PrintLogger();
                logger.Message += Logger_Message;

                Configurator.Initialize<ConfigurationLoader>("test")
                            //.Use<IServiceBuilder>();
                            //.AddLogger(logger)
                            .AddLogger<PrintLogger>("dummy", Logger_Message)
                    ;

                Logger.Debug("Hello, World!");

                Console.WriteLine($"Configurator.Get => {Configurator.Get("ASDF_KO")}");
                Configurator.Set("test", "aaaaa");
                Console.WriteLine($"Configurator.Set => {Configurator.Get("TEST")}");

                string encryptResult;
                string decryptResult;

                string encryptText = RandomCrypt.Create(CharType.All, 50);
                string encryptPassword = RandomCrypt.Create(CharType.All, 50);

                encryptResult = BouncyCastleCrypt.Encrypt(encryptText, encryptPassword);
                decryptResult = BouncyCastleCrypt.Decrypt(encryptResult, encryptPassword);

                Console.WriteLine($"BouncyCastleCrypt : {encryptText.Equals(decryptResult)}");

                encryptResult = AESCrypt.Encrypt(encryptText, encryptPassword);
                decryptResult = AESCrypt.Decrypt(encryptResult, encryptPassword);

                Console.WriteLine($"AESCrypt : {encryptText.Equals(decryptResult)}");

                encryptResult = DESCrypt.Encrypt(encryptText, encryptPassword);
                decryptResult = DESCrypt.Decrypt(encryptResult, encryptPassword);

                Console.WriteLine($"DESCrypt : {encryptText.Equals(decryptResult)}");

                encryptResult = TripleDESCrypt.Encrypt(encryptText, encryptPassword);
                decryptResult = TripleDESCrypt.Decrypt(encryptResult, encryptPassword);

                Console.WriteLine($"TripleDESCrypt : {encryptText.Equals(decryptResult)}");

                JsonTest jsonText = new JsonTest();

                jsonText.ToJson();

                //decryptResult = TripleDESCrypt.Decrypt("$2a$10$8cFqxylLllbWDe1Ljba1nefoa45BJgPKQ04K2uQ5H.5FSm5DyMiTa", "YullinTech123456", Encoding.Default);
                //decryptResult = AESCrypt.Decrypt("$2a$10$8cFqxylLllbWDe1Ljba1nefoa45BJgPKQ04K2uQ5H.5FSm5DyMiTa", "YullinTech123456", Encoding.Default);

                Console.WriteLine($"TestCrypt : {decryptResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("== Console Test End  ==============");
            Console.ReadKey();
        }

        private static void Logger_Message(object sender, LogEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public class JsonTest
    {
        public string? Text { set; get; }
    }
}