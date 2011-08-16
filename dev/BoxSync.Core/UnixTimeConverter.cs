using System;


namespace BoxSync.Core
{
	internal class UnixTimeConverter
	{
		private readonly static UnixTimeConverter _instance = new UnixTimeConverter();
		private readonly static DateTime _unixStartDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		private UnixTimeConverter(){}

		public static UnixTimeConverter Instance
		{
			get { return _instance; }
		}

		public DateTime FromUnixTime(double unixTime)
		{
			return _unixStartDate.AddSeconds(unixTime);
		}

		public double ToUnixTime(DateTime dateTime)
		{
			return dateTime.Subtract(_unixStartDate).TotalSeconds;
		}
	}
}
