using System;

using BoxSync.Core.Statuses;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents the result of 'GetServerTime' method call
	/// </summary>
	public class GetServerTimeResponse : ResponseBase<GetServerTimeStatus>
	{
		/// <summary>
		/// Initializes type instance
		/// </summary>
		/// <param name="serverTime">Server time</param>
		public GetServerTimeResponse(DateTime serverTime)
		{
			ServerTime = serverTime;
		}

		/// <summary>
		/// Gets server time
		/// </summary>
		public DateTime ServerTime
		{
			get; 
			private set;
		}
	}
}
