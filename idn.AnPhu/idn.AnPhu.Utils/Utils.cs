using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace idn.AnPhu.Utils
{
    public class CUtils
    {
        public static string TidNext(object strTidOrg, ref int n)
        {
            return string.Format("{0}.{1}", strTidOrg, n++);
        }
        public static string StdParam(object objParam)
        {
            if (objParam == null || objParam == DBNull.Value) return null;
            string str = Convert.ToString(objParam).Trim().ToUpper();
            if (str.Length <= 0) return null;
            return str;
        }
        public static object StdInt(object objParam)
        {
            if (objParam == null || objParam == DBNull.Value) return null;
            return Convert.ToInt32(objParam);
        }
        public static object StdDouble(object objParam)
        {
            if (objParam == null || objParam == DBNull.Value) return null;
            return Convert.ToDouble(objParam);
        }
        public static string StdDate(object objDate)
        {
            if (objDate == null || objDate == DBNull.Value) return null;
            DateTime dtime = Convert.ToDateTime(objDate);
            if (dtime < Convert.ToDateTime("1900-01-01 00:00:00") || dtime > Convert.ToDateTime("2100-01-01 23:59:59"))
                throw new Exception("MyBiz.DateTimeOutOfRange");
            return dtime.ToString("yyyy-MM-dd");
        }
        public static string StdT24Date(object objDate)
        {
            ////
            var dtfi = new DateTimeFormatInfo
            {
                ShortDatePattern = "yyyy-MM-dd",
                DateSeparator = "-"
            };
            string strT24format = "yyyyMMdd";
            ////
            if (objDate == null || objDate == DBNull.Value || string.IsNullOrEmpty(Convert.ToString(objDate))) return null;
            DateTime dtime = DateTime.ParseExact(Convert.ToString(objDate), strT24format, dtfi);
            if (dtime < Convert.ToDateTime("1900-01-01 00:00:00") || dtime > Convert.ToDateTime("2100-01-01 23:59:59"))
                throw new Exception("MyBiz.DateTimeOutOfRange");
            return dtime.ToString("yyyy-MM-dd");
        }
        public static string StdDTime(object objDTime)
        {
            if (objDTime == null || objDTime == DBNull.Value || objDTime.ToString().Length == 0) return null;
            DateTime dtime = Convert.ToDateTime(objDTime);
            if (dtime < Convert.ToDateTime("1900-01-01 00:00:00") || dtime > Convert.ToDateTime("2100-01-01 23:59:59"))
                throw new Exception("MyBiz.DateTimeOutOfRange");
            return dtime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string StdT24DTime(object objDTime)
        {
            ////
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
            dtfi.DateSeparator = "-";
            string strT24format = "yyyyMMddHHmmss";
            ////
            if (objDTime == null || objDTime == DBNull.Value || objDTime.ToString().Length == 0) return null;
            DateTime dtime = DateTime.ParseExact(Convert.ToString(objDTime), strT24format, dtfi);
            if (dtime < Convert.ToDateTime("1900-01-01 00:00:00") || dtime > Convert.ToDateTime("2100-01-01 23:59:59"))
                throw new Exception("MyBiz.DateTimeOutOfRange");
            return dtime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string StdDTimeBeginDay(object objDTime)
        {
            string strDTime = StdDate(objDTime);
            if (strDTime == null) return null;
            return string.Format("{0} 00:00:00", strDTime);
        }
        public static string StdDTimeEndDay(object objDTime)
        {
            string strDTime = StdDate(objDTime);
            if (strDTime == null) return null;
            return string.Format("{0} 23:59:59", strDTime);
        }
        public static string GetDateToSearch(object objDate)
        {
            int nHeuristicPriviousDays = -10; // days
            DateTime dtime = Convert.ToDateTime(objDate);
            dtime = dtime.AddDays(nHeuristicPriviousDays);
            dtime = new DateTime(dtime.Year, dtime.Month, 1);
            return dtime.ToString("yyyy-MM-dd");
        }
        public static string GetDateToSearch(object objDate, string dateTimeFormat)
        {
            int nHeuristicPriviousDays = -10; // days
            DateTime dtime = Convert.ToDateTime(objDate);
            dtime = dtime.AddDays(nHeuristicPriviousDays);
            dtime = new DateTime(dtime.Year, dtime.Month, 1);
            return dtime.ToString(dateTimeFormat);
        }
        //Is datetime
        public static bool IsDateTime(object Object)
        {
            try
            {
                var strDate = Object.ToString();
                DateTime dt;
                DateTime.TryParse(strDate,
                    System.Globalization.CultureInfo.CurrentCulture,
                    System.Globalization.DateTimeStyles.None, out dt);

                if (dt > DateTime.MinValue && dt < DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static TimeSpan TimeSpan(DateTime startdate, DateTime enddate)
        {
            TimeSpan timeSpan = startdate - enddate;
            return timeSpan;
        }

        public static TimeSpan DateTimeSubtract(DateTime startdate, DateTime enddate)
        {
            TimeSpan timeSpan = startdate.Subtract(enddate);
            return timeSpan;
        }

        public static DateTime MinDateTime(DateTime startdate, DateTime enddate)
        {
            var compare = DateTime.Compare(startdate, enddate);
            return compare < 0 ? startdate : enddate;
        }

        public static DateTime MaxDateTime(DateTime startdate, DateTime enddate)
        {
            var compare = DateTime.Compare(startdate, enddate);
            return compare < 0 ? enddate : startdate;
        }

        public static int TotalMonths(DateTime enddate, DateTime startdate)
        {
            var totalMonths = ((enddate.Year - startdate.Year) * 12) + enddate.Month - startdate.Month;
            return totalMonths;
        }

        public static DateTime ConvertToDateTime(string sdatetime)
        {
            DateTime dt;
            DateTime.TryParse(sdatetime,
                System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.None, out dt);
            return dt;
        }

        public static string FormatDateTime(string sdatetime, string sformat)
        {
            if (IsDateTime(sdatetime))
            {
                return ConvertToDateTime(sdatetime).ToString(sformat);
            }
            return "";
        }

        public static DateTime ConvertToDateTimeAddDay(string sdatetime, int iday)
        {
            return ConvertToDateTime(sdatetime).AddDays(iday);
        }

        public static string StrConvertToDateTimeAddDay(string sdatetime, int iday, string strformat)
        {
            return ConvertToDateTimeAddDay(sdatetime, iday).ToString(strformat);
        }

        //Isnumeric
        public static bool IsNumeric(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                double OutValue;
                return double.TryParse(obj.ToString().Trim(),
                 System.Globalization.NumberStyles.Any,
                   System.Globalization.CultureInfo.CurrentCulture,
                   out OutValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsInteger(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                int outValue;
                string strValue = Convert.ToString(obj).Trim();
                return Int32.TryParse(strValue, out outValue);
            }
        }

        public static int ConvertToInt32(object obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// return value: 1 - 10 - 100 - 1,000
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="strformat"></param>
        /// <returns></returns>
        public static string FormatInteger(int obj, string strformat)
        {
            var strNumber = "";
            if (obj < 0)
            {
                obj = obj * (-1);
                strNumber = String.Format(CultureInfo.InvariantCulture, strformat, obj);
                if (obj < 10)
                {

                    strNumber = '-' + strNumber.Substring(1, strNumber.Length - 1);
                }
                else
                {
                    strNumber = '-' + strNumber;
                }
            }
            else
            {
                strNumber = String.Format(CultureInfo.InvariantCulture, strformat, obj);
                if (obj < 10)
                {

                    strNumber = strNumber.Substring(1, strNumber.Length - 1);
                }
            }

            return strNumber;
        }

        /// <summary>
        /// return value: 1.00 - 10.00 - 100.00 - 1,000.00
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="strformat"></param>
        /// <returns></returns>
        public static string FormatNumeric(double obj, string strformat)
        {
            var strNumber = "";
            strNumber = String.Format(CultureInfo.InvariantCulture, strformat, obj);
            if (obj < 0)
            {
                obj = obj * (-1);
                strNumber = String.Format(CultureInfo.InvariantCulture, strformat, obj);
                if (obj < 10)
                {

                    strNumber = '-' + strNumber.Substring(1, strNumber.Length - 1);
                }
                else
                {
                    strNumber = '-' + strNumber;
                }
            }
            else
            {
                strNumber = String.Format(CultureInfo.InvariantCulture, strformat, obj);
                if (obj < 10)
                {
                    strNumber = strNumber.Substring(1, strNumber.Length - 1);
                }
            }

            return strNumber;
        }

        public static bool IsNumber(string number)
        {
            foreach (Char c in number)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// data
        /// separator = new[] { "hello" }
        /// </summary>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] StrSplit(string data, string[] separator)
        {
            return data.Split(separator, StringSplitOptions.None);
        }

        public static bool IsNullOrEmpty(object obj)
        {
            if (obj != null && obj.ToString().Trim().Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string StrTrim(object obj)
        {
            return !IsNullOrEmpty(obj) ? obj.ToString().Trim() : null;
        }

        public static string StrValue(object obj)
        {
            return !IsNullOrEmpty(obj) ? obj.ToString().Trim() : "";
        }

        public static string StrToUpper(object obj)
        {
            return !IsNullOrEmpty(obj) ? obj.ToString().ToUpper().Trim() : null;
        }

        public static string StrToLower(object obj)
        {
            return !IsNullOrEmpty(obj) ? obj.ToString().ToLower().Trim() : null;
        }

        #region["Email"]
        public static bool IsValidEmail(string email)
        {
            if (!IsNullOrEmpty(email))
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool IsEmail(string email)
        {
            if (!IsNullOrEmpty(email))
            {
                return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
