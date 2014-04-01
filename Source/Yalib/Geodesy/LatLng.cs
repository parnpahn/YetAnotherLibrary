using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Yalib.Geodesy
{
    #region LatLon class
    /// <summary>
    /// Contains the latitude and longitude of a single point on the globe
    /// </summary>
    public struct LatLng
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        public static readonly LatLng Empty = new LatLng { Latitude = 0, Longitude = 0 };

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Latitude:{0};Longitude:{1}", this.Latitude.ToString(), this.Longitude.ToString());
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="l1">The l1.</param>
        /// <param name="l2">The l2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(LatLng l1, LatLng l2)
        {
            return l1.Equals(l2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="l1">The l1.</param>
        /// <param name="l2">The l2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(LatLng l1, LatLng l2)
        {
            return !(l1 == l2);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (LatLng)) return false;
            return Equals((LatLng) obj);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(LatLng other)
        {
            return other.Latitude.Equals(Latitude) && other.Longitude.Equals(Longitude);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode()*397) ^ Longitude.GetHashCode();
            }
        }
    }
    #endregion

    #region LatLongCollection
    /// <summary>
    /// An collection of LatLong Class objects
    /// </summary>
    public class LatLongCollection:Collection<LatLng>
    {
    }
    #endregion
}
