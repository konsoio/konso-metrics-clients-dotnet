using System;

namespace Konso.Clients.Metrics.Clients.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToEpoch(this DateTime dateTime) => (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToEpochUtc(this DateTime dateTime) => (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromEpoch(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromEpochUtc(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}
