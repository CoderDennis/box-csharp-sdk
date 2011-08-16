using System;
using System.Net;
using System.Threading;

using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;


namespace BoxSync.Core.Extensions
{
	public static class BoxManagerExtensions
	{
		#region Authentication methods

		/// <summary>
		/// Does user authentication
		/// </summary>
		/// <param name="manager">BoxManager object which will be used for communication with Box.NET service</param>
		/// <param name="accountLogin">Account login</param>
		/// <param name="accountPassword">Account password</param>
		/// <returns>Indicates if authorization process was successful</returns>
		public static bool Login(this BoxManager manager, string accountLogin, string accountPassword)
		{
			bool toReturn = false;
			ManualResetEvent loginFinishedEvent = new ManualResetEvent(false);

			AuthenticationProcessFinished loginFinished = loginResult =>
			{
				toReturn = loginResult;
				loginFinishedEvent.Set();
			};

			Login(manager, accountLogin, accountPassword, loginFinished, null);

			loginFinishedEvent.WaitOne();

			return toReturn;
		}

		/// <summary>
		/// Does asynchronous user authentication
		/// </summary>
		/// <param name="manager">BoxManager object which will be used for communication with Box.NET service</param>
		/// <param name="userLogin">User login</param>
		/// <param name="userPassword">User password</param>
		/// <param name="authenticationCompleted">Callback method which will be invoked after authorization process completes</param>
		/// <param name="updateAuthenticationStatus">Callback method which will be invoked on each step of authorization process. Can be null</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="authenticationCompleted"/> is null</exception>
		public static void Login(this BoxManager manager, string userLogin, string userPassword, AuthenticationProcessFinished authenticationCompleted, UpdateStatus updateAuthenticationStatus)
		{
			BoxManager.ThrowIfParameterIsNull(authenticationCompleted, "authenticationCompleted");

			AuthenticationInformation authenticationInformation = new AuthenticationInformation
			{
				CurrentOperationCompleted = ProcessUserAuthentication,
				LoginCompleted = authenticationCompleted,
				Password = userPassword,
				UserName = userLogin,
				Status = AuthenticationStatus.ReadyToStartAuthentication,
				Ticket = null,
				UpdateAuthenticationStatus = updateAuthenticationStatus,
				Manager = manager
			};

			ProcessUserAuthentication(authenticationInformation);
		}

		/// <summary>
		/// Manages user authentication process
		/// </summary>
		/// <param name="authenticationInformation">Authorization information</param>
		private static void ProcessUserAuthentication(AuthenticationInformation authenticationInformation)
		{
			switch (authenticationInformation.Status)
			{
				case AuthenticationStatus.ReadyToStartAuthentication:
					EventHandlerExtensions.SafeInvoke(authenticationInformation.UpdateAuthenticationStatus, "Ready to start authorization...");
					
					authenticationInformation.Manager.GetTicket(GetTicket, authenticationInformation);
					
					EventHandlerExtensions.SafeInvoke(authenticationInformation.UpdateAuthenticationStatus, "Retrieving ticket...");
					break;
				case AuthenticationStatus.GetTicketFinishedSuccessfully:
					EventHandlerExtensions.SafeInvoke(authenticationInformation.UpdateAuthenticationStatus, "Submiting login/password...");
					SubmitAuthenticationInformation(authenticationInformation);
					ProcessUserAuthentication(authenticationInformation);
					break;
				case AuthenticationStatus.SubmitUserCredentialsFinishedSuccessfully:
					EventHandlerExtensions.SafeInvoke(authenticationInformation.UpdateAuthenticationStatus, "Retrieving authorization token...");
					authenticationInformation.Manager.GetAuthenticationToken(authenticationInformation.Ticket,
																	 GetAuthenticationTokenFinished, authenticationInformation);
					break;
				case AuthenticationStatus.GetAuthenticationTokenFinishedSuccessfully:
					EventHandlerExtensions.SafeInvoke(authenticationInformation.UpdateAuthenticationStatus, "Authorization finished successfuly...");
					authenticationInformation.Status = AuthenticationStatus.AuthenticationFinishedSuccessfuly;

					authenticationInformation.Manager.AuthenticationToken = authenticationInformation.Token;

					authenticationInformation.LoginCompleted(true);
					break;
				case AuthenticationStatus.GetAuthenticationTokenFailed:
					EventHandlerExtensions.SafeInvoke(authenticationInformation.UpdateAuthenticationStatus, "Failed to retrieve authorization tocken...");
					authenticationInformation.LoginCompleted(false);
					break;
				case AuthenticationStatus.GetTicketFailed:
					EventHandlerExtensions.SafeInvoke(authenticationInformation.UpdateAuthenticationStatus, "Failed to retrieve user ticket...");
					authenticationInformation.LoginCompleted(false);
					break;
				case AuthenticationStatus.SubmitUserCredentialsFailed:
					EventHandlerExtensions.SafeInvoke(authenticationInformation.UpdateAuthenticationStatus, "Failed submit login/password...");
					authenticationInformation.LoginCompleted(false);
					break;
			}


		}

		private static void GetTicket(GetTicketResponse result)
		{
			AuthenticationInformation authenticationInformation = (AuthenticationInformation)result.UserState;

			if (!string.IsNullOrEmpty(result.Ticket) && result.Status == GetTicketStatus.Successful)
			{
				authenticationInformation.Status = AuthenticationStatus.GetTicketFinishedSuccessfully;
				authenticationInformation.Ticket = result.Ticket;
			}
			else
			{
				authenticationInformation.Status = AuthenticationStatus.GetTicketFailed;
			}

			authenticationInformation.CurrentOperationCompleted(authenticationInformation);
		}

		private static void GetAuthenticationTokenFinished(GetAuthenticationTokenResponse result)
		{
			AuthenticationInformation authenticationInformation = (AuthenticationInformation)result.UserState;

			if(!string.IsNullOrEmpty(result.AuthenticationToken) && result.Status == GetAuthenticationTokenStatus.Successful)
			{
				authenticationInformation.Token = result.AuthenticationToken;
				authenticationInformation.User = result.AuthenticatedUser;

				authenticationInformation.Status = AuthenticationStatus.GetAuthenticationTokenFinishedSuccessfully;
			}
			else
			{
				authenticationInformation.Status = AuthenticationStatus.GetAuthenticationTokenFailed;
			}

			authenticationInformation.CurrentOperationCompleted(authenticationInformation);
		}

		/// <summary>
		/// Submits login/password
		/// </summary>
		/// <param name="authenticationInformation">Authentication information</param>
		/// <returns>Result which server returns after login/password submit</returns>
		private static string SubmitAuthenticationInformation(AuthenticationInformation authenticationInformation)
		{
			string uploadResult = null;

			using (WebClient client = new WebClient { Proxy = authenticationInformation.Manager.Proxy })
			{

				client.Headers.Add("Content-Type:application/x-www-form-urlencoded");

				Uri destinationAddress = new Uri("http://www.box.net/api/1.0/auth/" + authenticationInformation.Ticket);

				ManualResetEvent submitFinishedEvent = new ManualResetEvent(false);

				Action submitLoginPassword = () =>
				{
					uploadResult = client.UploadString(destinationAddress, "POST",
													   "login=" + authenticationInformation.UserName +
													   "&password=" +
													   authenticationInformation.Password +
													   "&dologin=1&__login=1");
				};

				AsyncCallback callback = asyncResult =>
				{
					ManualResetEvent submitFinished = (ManualResetEvent)asyncResult.AsyncState;

					submitFinished.Set();
				};

				IAsyncResult asyncResult2 = submitLoginPassword.BeginInvoke(callback, submitFinishedEvent);

				submitFinishedEvent.WaitOne();

				submitLoginPassword.EndInvoke(asyncResult2);
			}

			authenticationInformation.Status = !string.IsNullOrEmpty(uploadResult)
												? AuthenticationStatus.SubmitUserCredentialsFinishedSuccessfully
												: AuthenticationStatus.SubmitUserCredentialsFailed;

			return uploadResult;
		}

		#endregion
	}
}
