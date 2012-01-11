using System;
using System.Threading;

using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;

using NUnit.Framework;


namespace BoxSync.Core.IntegrationTests
{
    /// <summary>
	/// Set of tests for "GetTicket" method
    /// </summary>
	public class GetTicketTests : IntegrationTestBase
	{
		/// <summary>
		/// Tests synchronous version of "GetTicket" method
		/// </summary>
		[Test]
		public void TestSyncGetTicket()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;

			GetTicketStatus operationStatus = manager.GetTicket(out ticket);

			Assert.AreEqual(GetTicketStatus.Successful, operationStatus);
			Assert.IsNotNull(ticket);
		}

		/// <summary>
		/// Tests the behavior of asynchronous "GetTicket" method if passed callback delegate is <c>null</c>
		/// </summary>
		[Test]
		public void TestAsyncGetTicketWithNoCallbackDelegate()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			
            try
			{
				manager.GetTicket(null);

				Assert.Fail("GetTicket(null) call has to fail");
			}
			catch (Exception ex)
			{
				Assert.IsInstanceOfType(typeof(ArgumentException), ex);
			}
		}

		/// <summary>
		/// Tests the behavior of asynchronous "GetTicket" method
		/// </summary>
		[Test]
		public void TestAsyncGetTicket()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			const int someValue = 78435;
			ManualResetEvent wait = new ManualResetEvent(false);
			bool callbackWasExecuted = false;

			OperationFinished<GetTicketResponse> response = resp =>
			                                                	{
																	Assert.IsNull(resp.Error);
																	Assert.IsNotNull(resp.Ticket);
																	Assert.IsInstanceOfType(typeof(int), resp.UserState);
																	Assert.AreEqual(GetTicketStatus.Successful,  resp.Status);

			                                                		int userStatus = (int) resp.UserState;
																	Assert.AreEqual(someValue, userStatus);
			                                                		
																	callbackWasExecuted = true;
																	
																	wait.Reset();
			                                                	};
			
			manager.GetTicket(response, someValue);
			
			wait.WaitOne(30000);

			Assert.IsTrue(callbackWasExecuted, "Callback was not executed. The operation has timed out");
		}
	}
}
