using System;
using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;

namespace BoxSync.Core.Extensions
{
	internal sealed class AuthenticationInformation
	{
		public string Ticket { get; set; }
		public string Token { get; set; }
		public User User { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public AuthenticationProcessFinished LoginCompleted;
		public UpdateStatus UpdateAuthenticationStatus;
		public Action<AuthenticationInformation> CurrentOperationCompleted;
		public AuthenticationStatus Status { get; set; }
		public BoxManager Manager { get; set; }
	}
}
