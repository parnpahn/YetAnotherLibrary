using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hlt.Geodesy
{
    public static class GisHelper
    {
        /// <summary>
        /// EARTH_RADIUS (kilometers)
        /// </summary>
        public const double EARTH_RADIUS = 6378.137;

        /// <summary>
        /// RADs the specified angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static double Rad(double angle)
        {
            return angle * Math.PI / 180.0;
        }

        /// <summary>
        /// Gets the distance.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns></returns>
        public static double GetDistance(LatLng p1, LatLng p2)
        {
            var radLat1 = Rad(p1.Latitude);
            var radLat2 = Rad(p2.Latitude);
            var a = radLat1 - radLat2;
            var b = Rad(p1.Longitude) - Rad(p2.Longitude);
            var s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
            Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS; ;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        /// <summary>
        /// Gets the distance in KM of 2 coordinate.
        /// </summary>
        /// <param name="lat1">The lat1.</param>
        /// <param name="long1">The long1.</param>
        /// <param name="lat2">The lat2.</param>
        /// <param name="long2">The long2.</param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double long1, double lat2, double long2)
        {
            const int EARTH_RADIUS_KM = 6371; // KM.

            double Lat1r = ConvertDegreeToRadians(lat1);
            double Lat2r = ConvertDegreeToRadians(lat2);
            double Long1r = ConvertDegreeToRadians(long1);
            double Long2r = ConvertDegreeToRadians(long2);

            double d = Math.Acos(Math.Sin(Lat1r) *
                        Math.Sin(Lat2r) + Math.Cos(Lat1r) *
                        Math.Cos(Lat2r) *
                        Math.Cos(Long2r - Long1r)) * EARTH_RADIUS_KM;
            return d;

        }

        public static double GetDistanceInMiles(double lat1, double long1, double lat2, double long2)
        {
            return GetDistance(lat1, long1, lat2, long2) * 1000 / 1609.344;
        }

        private static double ConvertDegreeToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }
    }
}
