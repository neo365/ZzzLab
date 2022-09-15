using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ZzzLab
{
    public static partial class ConvertExtension
    {
        public static string To24Hours(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string To24Hours(this DateTime? date)
        {
            if (date != null)
            {
                return ((DateTime)date).ToString("yyyy-MM-dd HH:mm:ss");
            }

            return string.Empty;
        }

        public static string TimeAgo(this DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0) years += 1;
                return string.Format("{0}년전", years);
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0) months += 1;
                return string.Format("{0}달전", months);
            }
            if (span.Days > 0) return string.Format(" {0}일전", span.Days);
            if (span.Hours > 0) return string.Format("{0} 시간전", span.Hours);
            if (span.Minutes > 0) return string.Format("{0} 분전", span.Minutes);
            if (span.Seconds > 3) return string.Format("{0} 초전", span.Seconds);
            if (span.Seconds <= 3) return "지금";
            return string.Empty;
        }

        public static string ToTimeString(this long value)
        {
            if (value >= (1000L * 60 * 60 * 24 * 365)) return ((double)value / (1000D * 60 * 60 * 24 * 365)).ToString("F2") + "year";
            if (value >= (1000L * 60 * 60 * 24 * 30)) return ((double)value / (1000D * 60 * 60 * 24 * 30)).ToString("F2") + "month";
            if (value >= (1000L * 60 * 60 * 24)) return ((double)value / (1000D * 60 * 60 * 24)).ToString("F2") + "day";
            if (value >= (1000L * 60 * 60)) return ((double)value / (1000D * 60 * 60)).ToString("F2") + "hour";
            if (value >= (1000L * 60)) return ((double)value / (1000D * 60)).ToString("F2") + "min";
            if (value >= (1000L)) return ((double)value / (1000D)).ToString("F2") + "sec";
            else return value + "ms";
        }

        public static string ToTimeString(this Stopwatch watch)
        {
            return watch.ElapsedMilliseconds.ToTimeString();
        }

        public static DateTime ToDateTime(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) throw new ArgumentNullException(nameof(str));
            if (str.IsDateTime())
            {
                int count = 0;
                List<char> charlist = new List<char>();
                foreach (char c in str)
                {
                    char addChr = c;
                    if (c == ':')
                    {
                        count++;
                        if (count > 2) addChr = '.';
                    }

                    charlist.Add(addChr);
                }

                str = new string(charlist.ToArray());

                if (DateTime.TryParse(str, out DateTime dateTime)) return dateTime;
                else
                {
                    string[] formats =
                    {
                        "yyyyMMdd",
                        "yyyy-MM-dd"
                    };

                    if (DateTime.TryParseExact(str, formats, null, System.Globalization.DateTimeStyles.None, out DateTime convertedValue))
                    {
                        return convertedValue;
                    }
                }
            }

            throw new InvalidCastException("\\" + str + "\\ is Not DateTime.");
        }

        public static DateTime ToDateTime(this object obj)
        {
            if (obj.IsDateTime()) return ToDateTime(obj.ToString());

            throw new InvalidCastException("object is Not DateTime.");
        }

        public static DateTime? ToDateTimeNullable(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str)) return null;
                return str.ToDateTime();
            }
            catch
            {
                return null;
            }
        }

        public static DateTime ToDateTime(this long timestamp)
        {
            DateTime datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return datetime.AddSeconds(timestamp).ToLocalTime();
        }

        public static DateTime? ToDateTimeNullable(this long v)
        {
            try
            {
                return v.ToDateTime();
            }
#if DEBUG && TRACE
            catch (Exception ex)
            {
                Debug.WriteLine("ToDateTimeNullable : " + v);
                Debug.WriteLine(ex.ToString());
            }
#else
            catch { }
#endif

            return null;
        }

        public static readonly string[][] TIMEZONES = {
                new string[] {"Morocco Standard Time", "(GMT) Casablanca", "+00:00", "Casablanca"},
                new string[] {"GMT Standard Time", "(GMT) Greenwich Mean Time : Dublin Edinburgh Lisbon London", "+00:00", "Greenwich Mean Time : Dublin Edinburgh Lisbon London"},
                new string[] {"Greenwich Standard Time", "(GMT) Monrovia Reykjavik", "+00:00", "Monrovia Reykjavik"},
                new string[] {"W. Europe Standard Time", "(GMT+01:00) Amsterdam Berlin Bern Rome Stockholm Vienna", "+01:00", "Amsterdam Berlin Bern Rome Stockholm Vienna"},
                new string[] {"Central Europe Standard Time", "(GMT+01:00) Belgrade Bratislava Budapest Ljubljana Prague", "+01:00", "Belgrade Bratislava Budapest Ljubljana Prague"},
                new string[] {"Romance Standard Time", "(GMT+01:00) Brussels Copenhagen Madrid Paris", "+01:00", "Brussels Copenhagen Madrid Paris"},
                new string[] {"Central European Standard Time", "(GMT+01:00) Sarajevo Skopje Warsaw Zagreb", "+01:00", "Sarajevo Skopje Warsaw Zagreb"},
                new string[] {"W. Central Africa Standard Time", "(GMT+01:00) West Central Africa", "+01:00", "West Central Africa"},
                new string[] {"Jordan Standard Time", "(GMT+02:00) Amman", "+02:00", "Amman"},
                new string[] {"GTB Standard Time", "(GMT+02:00) Athens Bucharest Istanbul", "+02:00", "Athens Bucharest Istanbul"},
                new string[] {"Middle East Standard Time", "(GMT+02:00) Beirut", "+02:00", "Beirut"},
                new string[] {"Egypt Standard Time", "(GMT+02:00) Cairo", "+02:00", "Cairo"},
                new string[] {"South Africa Standard Time", "(GMT+02:00) Harare Pretoria", "+02:00", "Harare Pretoria"},
                new string[] {"FLE Standard Time", "(GMT+02:00) Helsinki Kyiv Riga Sofia Tallinn Vilnius", "+02:00", "Helsinki Kyiv Riga Sofia Tallinn Vilnius"},
                new string[] {"Israel Standard Time", "(GMT+02:00) Jerusalem", "+02:00", "Jerusalem"},
                new string[] {"E. Europe Standard Time", "(GMT+02:00) Minsk", "+02:00", "Minsk"},
                new string[] {"Namibia Standard Time", "(GMT+02:00) Windhoek", "+02:00", "Windhoek"},
                new string[] {"Arabic Standard Time", "(GMT+03:00) Baghdad", "+03:00", "Baghdad"},
                new string[] {"Arab Standard Time", "(GMT+03:00) Kuwait Riyadh", "+03:00", "Kuwait Riyadh"},
                new string[] {"Russian Standard Time", "(GMT+03:00) Moscow St. Petersburg Volgograd", "+03:00", "Moscow St. Petersburg Volgograd"},
                new string[] {"E. Africa Standard Time", "(GMT+03:00) Nairobi", "+03:00", "Nairobi"},
                new string[] {"Georgian Standard Time", "(GMT+03:00) Tbilisi", "+03:00", "Tbilisi"},
                new string[] {"Iran Standard Time", "(GMT+03:30) Tehran", "+03:30", "Tehran"},
                new string[] {"Arabian Standard Time", "(GMT+04:00) Abu Dhabi Muscat", "+04:00", "Abu Dhabi Muscat"},
                new string[] {"Azerbaijan Standard Time", "(GMT+04:00) Baku", "+04:00", "Baku"},
                new string[] {"Mauritius Standard Time", "(GMT+04:00) Port Louis", "+04:00", "Port Louis"},
                new string[] {"Caucasus Standard Time", "(GMT+04:00) Yerevan", "+04:00", "Yerevan"},
                new string[] {"Afghanistan Standard Time", "(GMT+04:30) Kabul", "+04:30", "Kabul"},
                new string[] {"Ekaterinburg Standard Time", "(GMT+05:00) Ekaterinburg", "+05:00", "Ekaterinburg"},
                new string[] {"Pakistan Standard Time", "(GMT+05:00) Islamabad Karachi", "+05:00", "Islamabad Karachi"},
                new string[] {"West Asia Standard Time", "(GMT+05:00) Tashkent", "+05:00", "Tashkent"},
                new string[] {"India Standard Time", "(GMT+05:30) Chennai Kolkata Mumbai New Delhi", "+05:30", "Chennai Kolkata Mumbai New Delhi"},
                new string[] {"Sri Lanka Standard Time", "(GMT+05:30) Sri Jayawardenepura", "+05:30", "Sri Jayawardenepura"},
                new string[] {"Nepal Standard Time", "(GMT+05:45) Kathmandu", "+05:45", "Kathmandu"},
                new string[] {"N. Central Asia Standard Time", "(GMT+06:00) Almaty Novosibirsk", "+06:00", "Almaty Novosibirsk"},
                new string[] {"Central Asia Standard Time", "(GMT+06:00) Astana Dhaka", "+06:00", "Astana Dhaka"},
                new string[] {"Myanmar Standard Time", "(GMT+06:30) Yangon (Rangoon)", "+06:30", "Yangon (Rangoon)"},
                new string[] {"SE Asia Standard Time", "(GMT+07:00) Bangkok Hanoi Jakarta", "+07:00", "Bangkok Hanoi Jakarta"},
                new string[] {"North Asia Standard Time", "(GMT+07:00) Krasnoyarsk", "+07:00", "Krasnoyarsk"},
                new string[] {"China Standard Time", "(GMT+08:00) Beijing Chongqing Hong Kong Urumqi", "+08:00", "Beijing Chongqing Hong Kong Urumqi"},
                new string[] {"North Asia East Standard Time", "(GMT+08:00) Irkutsk Ulaan Bataar", "+08:00", "Irkutsk Ulaan Bataar"},
                new string[] {"Singapore Standard Time", "(GMT+08:00) Kuala Lumpur Singapore", "+08:00", "Kuala Lumpur Singapore"},
                new string[] {"W. Australia Standard Time", "(GMT+08:00) Perth", "+08:00", "Perth"},
                new string[] {"Taipei Standard Time", "(GMT+08:00) Taipei", "+08:00", "Taipei"},
                new string[] {"Korea Standard Time", "(GMT+09:00) Seoul", "+09:00", "Seoul"},
                new string[] {"Tokyo Standard Time", "(GMT+09:00) Osaka Sapporo Tokyo", "+09:00", "Osaka Sapporo Tokyo"},
                new string[] {"Yakutsk Standard Time", "(GMT+09:00) Yakutsk", "+09:00", "Yakutsk"},
                new string[] {"Cen. Australia Standard Time", "(GMT+09:30) Adelaide", "+09:30", "Adelaide"},
                new string[] {"AUS Central Standard Time", "(GMT+09:30) Darwin", "+09:30", "Darwin"},
                new string[] {"E. Australia Standard Time", "(GMT+10:00) Brisbane", "+10:00", "Brisbane"},
                new string[] {"AUS Eastern Standard Time", "(GMT+10:00) Canberra Melbourne Sydney", "+10:00", "Canberra Melbourne Sydney"},
                new string[] {"West Pacific Standard Time", "(GMT+10:00) Guam Port Moresby", "+10:00", "Guam Port Moresby"},
                new string[] {"Tasmania Standard Time", "(GMT+10:00) Hobart", "+10:00", "Hobart"},
                new string[] {"Vladivostok Standard Time", "(GMT+10:00) Vladivostok", "+10:00", "Vladivostok"},
                new string[] {"Central Pacific Standard Time", "(GMT+11:00) Magadan Solomon Is. New Caledonia", "+11:00", "Magadan Solomon Is. New Caledonia"},
                new string[] {"New Zealand Standard Time", "(GMT+12:00) Auckland Wellington", "+12:00", "Auckland Wellington"},
                new string[] {"Fiji Standard Time", "(GMT+12:00) Fiji Kamchatka Marshall Is.", "+12:00", "Fiji Kamchatka Marshall Is."},
                new string[] {"Tonga Standard Time", "(GMT+13:00) Nuku'alofa", "+13:00", "Nuku'alofa"},
                new string[] {"Azores Standard Time", "(GMT-01:00) Azores", "-01:00", "Azores"},
                new string[] {"Cape Verde Standard Time", "(GMT-01:00) Cape Verde Is.", "-01:00", "Cape Verde Is."},
                new string[] {"Mid-Atlantic Standard Time", "(GMT-02:00) Mid-Atlantic", "-02:00", "Mid-Atlantic"},
                new string[] {"E. South America Standard Time", "(GMT-03:00) Brasilia", "-03:00", "Brasilia"},
                new string[] {"Argentina Standard Time", "(GMT-03:00) Buenos Aires", "-03:00", "Buenos Aires"},
                new string[] {"SA Eastern Standard Time", "(GMT-03:00) Georgetown", "-03:00", "Georgetown"},
                new string[] {"Greenland Standard Time", "(GMT-03:00) Greenland", "-03:00", "Greenland"},
                new string[] {"Montevideo Standard Time", "(GMT-03:00) Montevideo", "-03:00", "Montevideo"},
                new string[] {"Newfoundland Standard Time", "(GMT-03:30) Newfoundland", "-03:30", "Newfoundland"},
                new string[] {"Atlantic Standard Time", "(GMT-04:00) Atlantic Time (Canada)", "-04:00", "Atlantic Time (Canada)"},
                new string[] {"SA Western Standard Time", "(GMT-04:00) La Paz", "-04:00", "La Paz"},
                new string[] {"Central Brazilian Standard Time", "(GMT-04:00) Manaus", "-04:00", "Manaus"},
                new string[] {"Pacific SA Standard Time", "(GMT-04:00) Santiago", "-04:00", "Santiago"},
                new string[] {"Venezuela Standard Time", "(GMT-04:30) Caracas", "-04:30!", "Caracas"},
                new string[] {"SA Pacific Standard Time", "(GMT-05:00) Bogota Lima Quito Rio Branco", "-05:00", "Bogota Lima Quito Rio Branco"},
                new string[] {"Eastern Standard Time", "(GMT-05:00) Eastern Time (US & Canada)", "-05:00", "Eastern Time (US & Canada)"},
                new string[] {"US Eastern Standard Time", "(GMT-05:00) Indiana (East)", "-05:00", "Indiana (East)"},
                new string[] {"Central America Standard Time", "(GMT-06:00) Central America", "-06:00", "Central America"},
                new string[] {"Central Standard Time", "(GMT-06:00) Central Time (US & Canada)", "-06:00", "Central Time (US & Canada)"},
                new string[] {"Central Standard Time (Mexico)", "(GMT-06:00) Guadalajara Mexico City Monterrey", "-06:00", "Guadalajara Mexico City Monterrey"},
                new string[] {"Canada Central Standard Time", "(GMT-06:00) Saskatchewan", "-06:00", "Saskatchewan"},
                new string[] {"US Mountain Standard Time", "(GMT-07:00) Arizona", "-07:00", "Arizona"},
                new string[] {"Mountain Standard Time (Mexico)", "(GMT-07:00) Chihuahua La Paz Mazatlan", "-07:00", "Chihuahua La Paz Mazatlan"},
                new string[] {"Mountain Standard Time", "(GMT-07:00) Mountain Time (US & Canada)", "-07:00", "Mountain Time (US & Canada)"},
                new string[] {"Pacific Standard Time", "(GMT-08:00) Pacific Time (US & Canada)", "-08:00", "Pacific Time (US & Canada)"},
                new string[] {"Pacific Standard Time (Mexico)", "(GMT-08:00) Tijuana Baja California", "-08:00", "Tijuana Baja California"},
                new string[] {"Alaskan Standard Time", "(GMT-09:00) Alaska", "-09:00", "Alaska"},
                new string[] {"Hawaiian Standard Time", "(GMT-10:00) Hawaii", "-10:00", "Hawaii"},
                new string[] {"Samoa Standard Time", "(GMT-11:00) Midway Island Samoa", "-11:00", "Midway Island Samoa"},
                new string[] {"Dateline Standard Time", "(GMT-12:00) International Date Line West", "-12:00", "International Date Line West"}
            };

        public static string ToTimezoneId(this string s)
        {
            string timezoneId = string.Empty;

            s = s.ToUpper();

            if (s == "GMT") s = "+00:00";

            if (s == "00:00" || s == "-00:00") s = "+00:00";
            if (s.ToUpper().StartsWith("GMT")) s.Remove("GMT");

            foreach (string[] timezone in TIMEZONES)
            {
                if (timezone[2].Equals(s, StringComparison.OrdinalIgnoreCase))
                {
                    timezoneId = timezone[0];
                    break;
                }
                else if (("GMT" + timezone[2]).Equals(s, StringComparison.OrdinalIgnoreCase))
                {
                    timezoneId = timezone[0];
                    break;
                }
            }

            return timezoneId;
        }

        public static DateTime ToDateTime(this long timestamp, string gmt)
        {
            try
            {
                DateTime utcDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

                TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(gmt.ToTimezoneId());
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.AddSeconds(timestamp), timezone);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new TimeZoneNotFoundException("The registry does not define the Central Standard Time zone.");
            }
            catch (InvalidTimeZoneException)
            {
                throw new InvalidTimeZoneException("Registry data on the Central Standard Time zone has been corrupted.");
            }
            catch { throw; }
        }

        public static long ToUnixTimeStamp(this DateTime dt)
        {
            DateTime utcdt = dt.ToUniversalTime();
            DateTimeOffset dto = new DateTimeOffset(utcdt.Year, utcdt.Month, utcdt.Day, utcdt.Hour, utcdt.Minute, utcdt.Second, TimeSpan.Zero);
            return dto.ToUnixTimeSeconds();
        }

        public static DateTime FromUnixTimestamp(this double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static DateTime? ToDateTimeNullable(this object o)
        {
            if (o == null || o == DBNull.Value || o.GetType() == typeof(DBNull)) return null;
            return System.Convert.ToDateTime(o);
        }

        /// <summary>
        /// DateTimeOffset 을 Datetime으로 변경한다.
        /// </summary>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeNullable(this DateTimeOffset dateTimeOffset)
            => dateTimeOffset.LocalDateTime;

        /// <summary>
        /// DateTime 을 DateTimeOffset으로 변경한다.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
            => (DateTimeOffset)DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

        public static int ToQuarter(this DateTime dt)
        {
            switch (dt.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;

                case 4:
                case 5:
                case 6:
                    return 2;

                case 7:
                case 8:
                case 9:
                    return 3;

                case 10:
                case 11:
                case 12:
                    return 4;

                default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(dt));
        }

        public static int ToQuarter(this int month)
        {
            switch (month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;

                case 4:
                case 5:
                case 6:
                    return 2;

                case 7:
                case 8:
                case 9:
                    return 3;

                case 10:
                case 11:
                case 12:
                    return 4;

                default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(month));
        }

        public static DateTime AddQuarter(this DateTime dt, int quarter)
        {
            return dt.AddMonths(quarter * 3);
        }
    }
}