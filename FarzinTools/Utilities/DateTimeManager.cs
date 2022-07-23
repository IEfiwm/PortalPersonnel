using Ngra.Common.Models;
using System;
using System.Globalization;
using System.Reflection;

namespace Ngra.Common.Utilities
{
    public static class DateTimeManager
    {
        #region Constants

        public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";

        public const string LongDateFormat = "dddd, dd MMMM yyyy";
        
        public const string YearMonthFormat = "yyyy-MM";

        public const string ShortDateFormat = "yyyy/MM/dd";

        public const string LongDateTimeFormat = "yyyy/MM/dd HH:mm tt";

        public const string ShortDateTimeFormat = "yyyy/MM/dd HH:mm:ss";

        public const string DateTimeFormatWithDay = "dddd, d MMMM ساعت HH:mm";

        public const string HourMinuteTimeFormat = "HH:mm";
        
        public const string DateTimeFormatWithDayAndYear = "dddd, d MMMM yy ساعت HH:mm";
        
        public const string DateFormatWithDayAndYear = "dddd, d MMMM yy";

        #endregion

        private static CultureInfo _persianCultureInfo;

        public static CultureInfo GetPersianCultureInfo()
        {
            return _persianCultureInfo ?? (_persianCultureInfo = CreatePersianCultureInfo());
        }

        public static string ToPersianDateTimeString(this DateTime dateTime, string format = ShortDateFormat)
        {
            if (dateTime == DateTime.MinValue)
            {
                throw new ArgumentException("Cannot convert DateTime.MinValue to persian.", nameof(dateTime));
            }

            return dateTime.ToString(format, GetPersianCultureInfo());
        }

        public static string ToPersianDateTimeString(this DateTime? dateTime, string format = ShortDateFormat)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }

            return dateTime.Value.ToPersianDateTimeString(format);
        }


        public static string ToEnglishMonthString(this int month)
        {
            return new DateTime(2015, month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("en"));
        }

        public static string ToPersianMonthString(this int month)
        {
            switch (month)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return string.Empty;
            }
        }

        public static string ToPersianString(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "یک شنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنج شنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                case DayOfWeek.Saturday:
                    return "شنبه";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null);
            }
        }

        public static PersianDateTime ToPersianDateTime(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime));
            }

            string persianDateTimeString = dateTime.ToString("yyyy/MM/dd/HH/mm/ss/fff", GetPersianCultureInfo());

            var array = persianDateTimeString.Split('/');

            return new PersianDateTime(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]), int.Parse(array[3]), int.Parse(array[4]), int.Parse(array[5]), int.Parse(array[6]));
        }

        public static PersianDateTime? ToPersianDateTime(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            string persianDateTimeString = dateTime.Value.ToString("yyyy/MM/dd/HH/mm/ss/fff", GetPersianCultureInfo());

            var array = persianDateTimeString.Split('/');

            return new PersianDateTime(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]), int.Parse(array[3]), int.Parse(array[4]), int.Parse(array[5]), int.Parse(array[6]));
        }

        public static DateTime GetStartDateTimeOfCurrentPersianMonth()
        {
            var persianCalendar = new PersianCalendar();

            //  Create english DateTime that equals first day of current year and current month in persian DateTime
            return persianCalendar.ToDateTime(persianCalendar.GetYear(DateTime.Now), //  Get current year in Persian
                persianCalendar.GetMonth(DateTime.Now), //  Get current month in Persian
                1, 0, 0, 0, 0);
        }

        public static DateTime GetEndDateTimeOfCurrentPersianMonth()
        {
            var persianCalendar = new PersianCalendar();

            //  Get current year in Persian
            int year = persianCalendar.GetYear(DateTime.Now);

            //  Get current month in Persian
            int month = persianCalendar.GetMonth(DateTime.Now);

            //  Create english DateTime that equals last day of current year in persian DateTime
            return persianCalendar.ToDateTime(year, month, persianCalendar.GetDaysInMonth(year, month), 23, 59, 59, 0);
        }

        public static DateTime GetStartDateTimeOfCurrentPersianYear()
        {
            var persianCalendar = new PersianCalendar();

            //  Create english DateTime that equals first day of current year in persian DateTime
            return persianCalendar.ToDateTime(persianCalendar.GetYear(DateTime.Now), //  Get current year in Persian
                1, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndDateTimeOfCurrentPersianYear()
        {
            var persianCalendar = new PersianCalendar();

            //  Create english DateTime that equals last day of current year in persian DateTime
            return persianCalendar.ToDateTime(persianCalendar.GetYear(DateTime.Now), //  Get current year in Persian
                12, 29, 23, 59, 59, 0);
        }

        public static DateTime GetDateTimeOfNextPersianMonth(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var christianDateTime = Convert.ToDateTime(value);

            var persianCalendar = new PersianCalendar();

            return persianCalendar.AddMonths(christianDateTime, 1);
        }


        public static PersianDateTime ToPersianDateTime(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var christianDateTime = Convert.ToDateTime(value);

            return christianDateTime.ToPersianDateTime();
        }

        public static PersianDateTime? ToPersianDateTimeNullable(object value)
        {
            if (value == null)
            {
                return null;
            }

            var christianDateTime = Convert.ToDateTime(value);

            return christianDateTime.ToPersianDateTime();
        }

        public static string ToShortPersianDateTimeString(object value)
        {
            //  Examples:
            //  امروز 10:25 PM
            //  دیروز 8:35 AM
            //  1393/08/25 2:35 PM
            //  ---------------------

            if (value == null)
            {
                return string.Empty;
            }

            var christianDateTime = Convert.ToDateTime(value);

            var persianCalendar = new PersianCalendar();

            var stringPersianDateTime = $"{persianCalendar.GetYear(christianDateTime):D4}/{persianCalendar.GetMonth(christianDateTime):D2}/{persianCalendar.GetDayOfMonth(christianDateTime):D2} {persianCalendar.GetHour(christianDateTime):D2}:{persianCalendar.GetMinute(christianDateTime):D2}:{persianCalendar.GetSecond(christianDateTime):D2}";

            var persianDateTime = DateTime.ParseExact(stringPersianDateTime, "yyyy/MM/dd HH:mm:ss", GetPersianCultureInfo());

            string date;

            if (christianDateTime.Year == DateTime.Now.Year && christianDateTime.Month == DateTime.Now.Month && christianDateTime.Day == DateTime.Now.Day)
            {
                date = "امروز";
            }
            else if (christianDateTime.Year == DateTime.Now.Year && christianDateTime.Month == DateTime.Now.Month && christianDateTime.Day == DateTime.Now.Day - 1)
            {
                date = "دیروز";
            }
            else
            {
                date = $"{persianCalendar.GetYear(christianDateTime):D4}/{persianCalendar.GetMonth(christianDateTime):D2}/{persianCalendar.GetDayOfMonth(christianDateTime):D2}";
            }

            return $"{date} {persianDateTime.ToString("h:mm tt")}";
        }

        public static string ToLongPersianDateString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var persianCalendar = new PersianCalendar();

            var christianDateTime = Convert.ToDateTime(value);

            var dayName = string.Empty;

            switch (persianCalendar.GetDayOfWeek(christianDateTime))
            {
                case DayOfWeek.Saturday:
                    dayName = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    dayName = "یکشنبه";
                    break;
                case DayOfWeek.Monday:
                    dayName = "دوشنبه";
                    break;
                case DayOfWeek.Tuesday:
                    dayName = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    dayName = "چهارشنبه";
                    break;
                case DayOfWeek.Thursday:
                    dayName = "پنجشنبه";
                    break;
                case DayOfWeek.Friday:
                    dayName = "جمعه";
                    break;
            }

            var monthName = string.Empty;

            switch (persianCalendar.GetMonth(christianDateTime))
            {
                case 1:
                    monthName = "فروردین";
                    break;
                case 2:
                    monthName = "اردیبهشت";
                    break;
                case 3:
                    monthName = "خرداد";
                    break;
                case 4:
                    monthName = "تیر";
                    break;
                case 5:
                    monthName = "مرداد";
                    break;
                case 6:
                    monthName = "شهریور";
                    break;
                case 7:
                    monthName = "مهر";
                    break;
                case 8:
                    monthName = "آبان";
                    break;
                case 9:
                    monthName = "آذر";
                    break;
                case 10:
                    monthName = "دی";
                    break;
                case 11:
                    monthName = "بهمن";
                    break;
                case 12:
                    monthName = "اسفند";
                    break;
            }

            return $"{dayName} {persianCalendar.GetDayOfMonth(christianDateTime)} {monthName} ماه {persianCalendar.GetYear(christianDateTime)}";
        }

        public static string ToShortDateString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var dateTime = Convert.ToDateTime(value);

            return $"{dateTime.Year:D4}/{dateTime.Month:D2}/{dateTime.Day:D2}";
        }

        public static string ToShortDateTimeString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var dateTime = Convert.ToDateTime(value);

            return $"{dateTime.Year:D4}/{dateTime.Month:D2}/{dateTime.Day:D2} {dateTime.Hour:D2}:{dateTime.Minute:D2}:{dateTime.Second:D2}";
        }

        public static string ToPersianDateTimeString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var christianDateTime = (DateTime)value;

            var persianCalendar = new PersianCalendar();

            return $"{persianCalendar.GetYear(christianDateTime):D4}/{persianCalendar.GetMonth(christianDateTime):D2}/{persianCalendar.GetDayOfMonth(christianDateTime):D2} {persianCalendar.GetHour(christianDateTime):D2}:{persianCalendar.GetMinute(christianDateTime):D2}:{persianCalendar.GetSecond(christianDateTime):D2}";
        }

        public static string ToPersianDateString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var christianDateTime = (DateTime)value;

            var persianCalendar = new PersianCalendar();

            return $"{persianCalendar.GetYear(christianDateTime):D4}/{persianCalendar.GetMonth(christianDateTime):D2}/{persianCalendar.GetDayOfMonth(christianDateTime):D2}";
        }

        public static string ToPersianTimeString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var christianDateTime = (DateTime)value;

            var persianCalendar = new PersianCalendar();

            return $"{persianCalendar.GetHour(christianDateTime):D2}:{persianCalendar.GetMinute(christianDateTime):D2}:{persianCalendar.GetSecond(christianDateTime):D2}";
        }

        public static DateTime ToChristianDateTime(object value)
        {
            if (value == null)
            {
                return DateTime.MinValue;
            }

            value = CommonHelper.ConvertToEnglishNumber(value.ToString());

            return Convert.ToDateTime(value, GetPersianCultureInfo());
        }

        public static DateTime? ToChristianDateTimeNullable(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return null;
            }

            value = CommonHelper.ConvertToEnglishNumber(value.ToString());

            return Convert.ToDateTime(value, GetPersianCultureInfo());
        }

        private static CultureInfo CreatePersianCultureInfo()
        {
            var cultureInfo = new CultureInfo("fa-IR");

            DateTimeFormatInfo formatInfo = cultureInfo.DateTimeFormat;

            formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };

            formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };

            var monthNames = new[]
            {
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
            };

            formatInfo.AbbreviatedMonthNames = formatInfo.MonthNames = formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;

            formatInfo.AMDesignator = "ق.ظ";

            formatInfo.PMDesignator = "ب.ظ";

            formatInfo.ShortDatePattern = "yyyy/MM/dd";

            formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";

            formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;

            var persianCalendar = new PersianCalendar();

            var fieldInfo = cultureInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);

            fieldInfo?.SetValue(cultureInfo, persianCalendar);

            var info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);

            info?.SetValue(formatInfo, persianCalendar);

            cultureInfo.NumberFormat.NumberDecimalSeparator = "/";

            cultureInfo.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;

            cultureInfo.NumberFormat.NumberNegativePattern = 0;

            return cultureInfo;
        }

        public static bool CanConvertToPersianDateTime(object value)
        {
            try
            {
                //  The result doesn't matter.
                //  just convert it.
                ToPersianDateTime(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CanConvertToChristianDateTime(object value)
        {
            try
            {
                //  The result doesn't matter.
                //  just convert it.
                ToChristianDateTime(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string FromNow(this DateTime dateTime)
        {
            var span = DateTime.Now - dateTime;

            if (span.Days > 365)
            {
                int years = span.Days / 365;

                if (span.Days % 365 != 0)
                {
                    years += 1;
                }

                return $"{years} سال پیش";
            }

            if (span.Days > 30)
            {
                int months = (span.Days / 30);

                if (span.Days % 31 != 0)
                {
                    months += 1;
                }

                return $"{months} ماه پیش";
            }

            if (span.Days > 0)
            {
                return $"{span.Days} روز پیش";
            }

            if (span.Hours > 0)
            {
                return $"{span.Hours} ساعت پیش";
            }

            if (span.Minutes > 0)
            {
                return $"{span.Minutes} دقیقه پیش";
            }

            if (span.Seconds > 5)
            {
                return $"{span.Seconds} ثانیه پیش";
            }

            if (span.Seconds <= 5)
            {
                return "همین حالا";
            }

            return string.Empty;
        }
    }
}