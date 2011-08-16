using BoxSync.Core.Statuses;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents response from 'PublicShare' method
	/// </summary>
	public sealed class PublicShareResponse : ResponseBase<PublicShareStatus>
	{
		/// <summary>
		/// Unique identifier of a publicly shared object
		/// </summary>
		public string PublicName
		{
			get;
			internal set;
		}
	}
}
