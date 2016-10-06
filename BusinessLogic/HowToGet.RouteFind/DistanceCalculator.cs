using System;

namespace HowToGet.RouteEngine
{
	public class DistanceCalculator
	{
		public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
		{
			double theta = lon1 - lon2;
			double dist = Math.Sin(Deg2Rad(lat1))*Math.Sin(Deg2Rad(lat2)) +
			              Math.Cos(Deg2Rad(lat1))*Math.Cos(Deg2Rad(lat2))*Math.Cos(Deg2Rad(theta));
			
			dist = Math.Acos(dist);
			dist = Rad2Deg(dist);
			dist = dist*60*1.1515;
			dist = dist*1.609344;
			return (dist);
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//::  This function converts decimal degrees to radians             :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		private static double Deg2Rad(double deg)
		{
			return (deg * Math.PI / 180.0);
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//::  This function converts radians to decimal degrees             :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		private static double Rad2Deg(double rad)
		{
			return (rad / Math.PI * 180.0);
		}
	}
}