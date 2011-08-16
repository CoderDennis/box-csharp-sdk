using System;


namespace BoxSync.Core
{
	/// <summary>
	/// The exception that is thrown when the operation returns unknown status string
	/// </summary>
	public class UnknownOperationStatusException : Exception
	{
		/// <summary>
		/// Initializes exception object
		/// </summary>
		/// <param name="statusString">Status string returned by the operation</param>
		public UnknownOperationStatusException(string statusString)
			: base(string.Format("The operation has returned unknown status string. Status string: {0}", statusString))
		{
			StatusString = statusString;
		}

		/// <summary>
		/// Gets the status string returned by operation
		/// </summary>
		public string StatusString
		{
			get; 
			private set;
		}
	}
}
