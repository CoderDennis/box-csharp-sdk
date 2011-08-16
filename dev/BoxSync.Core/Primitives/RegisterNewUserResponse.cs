using BoxSync.Core.ServiceReference;
using BoxSync.Core.Statuses;


namespace BoxSync.Core.Primitives
{
	/// <summary>
	/// Represents response from 'RegisterNewUser' method
	/// </summary>
	public sealed class RegisterNewUserResponse : ResponseBase<RegisterNewUserStatus>
	{
		/// <summary>
		/// Gets or sets authorization token
		/// </summary>
		public string Token
		{
			get; 
			internal set;
		}

		/// <summary>
		/// Gets or sets user information
		/// </summary>
		public SOAPUser User
		{
			get; 
			internal set;
		}
	}
}
