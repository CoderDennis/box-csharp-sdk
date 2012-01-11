using BoxSync.Core.Primitives;


namespace BoxSync.Core.IntegrationTests
{
	public class TestContext
	{
		public BoxManager Manager
		{
			get; 
			set;
		}

		public string Token
		{
			get; 
			set;
		}

		public string Ticket
		{
			get; 
			set;
		}

		public User AuthenticatedUser
		{
			get; 
			set;
		}
	}
}
