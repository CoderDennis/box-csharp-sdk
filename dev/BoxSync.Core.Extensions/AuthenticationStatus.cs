namespace BoxSync.Core.Extensions
{
	/// <summary>
	/// Specifies statuses of authentication process
	/// </summary>
	internal enum AuthenticationStatus : byte
	{
		/// <summary>
		/// Unknown status
		/// </summary>
		Unknown = 0,

		ReadyToStartAuthentication = 1,
		GetTicketFinishedSuccessfully = 2,
		GetTicketFailed = 3,
		SubmitUserCredentialsFinishedSuccessfully = 4,
		SubmitUserCredentialsFailed = 5,
		GetAuthenticationTokenFinishedSuccessfully = 6,
		GetAuthenticationTokenFailed = 7,
		AuthenticationFinishedSuccessfuly = 8
	}
}
