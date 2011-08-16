using System;
using System.Threading;

using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;

using NUnit.Framework;


namespace BoxSync.Core.IntegrationTests
{
	/// <summary>
	/// Set of tests for "GetAuthenticationToken" method
	/// </summary>
	public class GetAuthenticationTokenTests : IntegrationTestBase
	{
		/// <summary>
		/// Tests synchronous "GetAuthenticationToken" method
		/// </summary>
		[Test]
		public void TestSyncGetAuthenticationToken()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;
			string token;
			User user;

			manager.GetTicket(out ticket);
			
			SubmitAuthenticationInformation(ticket);
			
			GetAuthenticationTokenStatus status = manager.GetAuthenticationToken(ticket, out token, out user);

			Assert.AreEqual(GetAuthenticationTokenStatus.Successful, status);

			Assert.IsNotNull(token);
			Assert.IsNotNull(user);

			Assert.IsNotNull(user.Email);
			Assert.IsNotNull(user.Login);
			StringAssert.IsMatch(Login, user.Login);
			Assert.AreNotEqual(0, user.AccessID);
			Assert.AreNotEqual(0, user.ID);
			//Assert.AreNotEqual(0, user.MaxUploadSize);
			Assert.AreNotEqual(0, user.SpaceAmount);
			Assert.AreNotEqual(0, user.SpaceUsed);
		}

		/// <summary>
		/// Tests the behavior of synchronous "GetAuthenticationToken" method in case of wrong ticket passed as a parameter
		/// </summary>
		[Test]
		public void TestSyncGetAuthenticationTokenWithWrongTicket()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;
			string token;
			User user;

			manager.GetTicket(out ticket);

			SubmitAuthenticationInformation(ticket);

			GetAuthenticationTokenStatus status = manager.GetAuthenticationToken(Guid.Empty.ToString(), out token, out user);

			Assert.AreEqual(GetAuthenticationTokenStatus.Failed, status);

			Assert.IsEmpty(token);
			Assert.IsNull(user);
		}

		/// <summary>
		/// Tests the behavior of synchronous "GetAuthenticationToken" method in case if user didn't log in
		/// </summary>
		[Test]
		public void TestSyncGetAuthenticationTokenIfUserIsNotLoggedIn()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;
			string token;
			User user;

			manager.GetTicket(out ticket);

			GetAuthenticationTokenStatus status = manager.GetAuthenticationToken(ticket, out token, out user);

			Assert.AreEqual(GetAuthenticationTokenStatus.NotLoggedID, status);

			Assert.IsEmpty(token);
			Assert.IsNull(user);
		}


		/// <summary>
		/// Tests the behavior of asynchronous "GetAuthenticationToken" method in case if callback delegate is <c>null</c>
		/// </summary>
		[Test]
		public void TestAsyncGetAuthenticationTokenWithNoCallbackDelegate()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;
			
			manager.GetTicket(out ticket);

			SubmitAuthenticationInformation(ticket);

			try
			{
				manager.GetAuthenticationToken(ticket, null);

				Assert.Fail("GetAuthenticationToken(ticket, null) has to fail");
			}
			catch (Exception ex)
			{
				Assert.IsInstanceOfType(typeof(ArgumentException), ex);
			}
		}

		/// <summary>
		/// Tests asynchronous "GetAuthenticationToken" method
		/// </summary>
		[Test]
		public void TestAsyncGetAuthenticationToken()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;
			ManualResetEvent wait = new ManualResetEvent(false);
			bool callbackWasExecuted = false;
			const int state = 874532489;

			OperationFinished<GetAuthenticationTokenResponse> callback = resp =>
			                                                             	{
																				Assert.IsNull(resp.Error);

																				Assert.IsInstanceOfType(typeof(int), resp.UserState);
			                                                             		
																				Assert.AreEqual(state, (int) resp.UserState);

																				Assert.AreEqual(GetAuthenticationTokenStatus.Successful, resp.Status);

																				Assert.IsNotNull(resp.AuthenticationToken);
																				Assert.IsNotNull(resp.AuthenticatedUser);

																				Assert.IsNotNull(resp.AuthenticatedUser.Email);
																				Assert.IsNotNull(resp.AuthenticatedUser.Login);
																				StringAssert.IsMatch(Login, resp.AuthenticatedUser.Login);
																				Assert.AreNotEqual(0, resp.AuthenticatedUser.AccessID);
																				Assert.AreNotEqual(0, resp.AuthenticatedUser.ID);
																				//Assert.AreNotEqual(0, resp.AuthenticatedUser.MaxUploadSize);
																				Assert.AreNotEqual(0, resp.AuthenticatedUser.SpaceAmount);
																				Assert.AreNotEqual(0, resp.AuthenticatedUser.SpaceUsed);

			                                                             		callbackWasExecuted = true;
			                                                             		wait.Reset();
			                                                             	};


			manager.GetTicket(out ticket);

			SubmitAuthenticationInformation(ticket);

			manager.GetAuthenticationToken(ticket, callback, state);
			wait.WaitOne(30000);

			Assert.IsTrue(callbackWasExecuted, "Callback was not executed. The operation has timed out");
		}

		/// <summary>
		/// Tests the behavior of asynchronous "GetAuthenticationToken" method in case if user didn't log in
		/// </summary>
		[Test]
		public void TestAsyncGetAuthenticationTokenIfUserIsNotLoggedIn()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;
			ManualResetEvent wait = new ManualResetEvent(false);
			bool callbackWasExecuted = false;
			const int state = 874524489;

			OperationFinished<GetAuthenticationTokenResponse> callback = resp =>
			                                                             	{
			                                                             		Assert.IsNull(resp.Error);

			                                                             		Assert.IsInstanceOfType(typeof (int), resp.UserState);

			                                                             		Assert.AreEqual(state, (int) resp.UserState);

																				Assert.AreEqual(GetAuthenticationTokenStatus.NotLoggedID, resp.Status);

																				Assert.IsEmpty(resp.AuthenticationToken);
																				Assert.IsNull(resp.AuthenticatedUser);

			                                                             		callbackWasExecuted = true;
			                                                             		wait.Reset();
			                                                             	};


			manager.GetTicket(out ticket);

			manager.GetAuthenticationToken(ticket, callback, state);
			wait.WaitOne(30000);

			Assert.IsTrue(callbackWasExecuted, "Callback was not executed. The operation has timed out");
		}

		[Test]
		public void TestAsyncGetAuthenticationTokenWithWrongTicket()
		{
			BoxManager manager = new BoxManager(ApplicationKey, ServiceUrl, null);
			string ticket;
			ManualResetEvent wait = new ManualResetEvent(false);
			bool callbackWasExecuted = false;
			const int state = 874532489;

			OperationFinished<GetAuthenticationTokenResponse> callback = resp =>
			                                                             	{
			                                                             		Assert.IsNull(resp.Error);

			                                                             		Assert.IsInstanceOfType(typeof (int), resp.UserState);

			                                                             		Assert.AreEqual(state, (int) resp.UserState);
																				
																				Assert.AreEqual(GetAuthenticationTokenStatus.Failed, resp.Status);

																				Assert.IsEmpty(resp.AuthenticationToken);
																				Assert.IsNull(resp.AuthenticatedUser);

			                                                             		callbackWasExecuted = true;
			                                                             		wait.Reset();
			                                                             	};


			manager.GetTicket(out ticket);

			SubmitAuthenticationInformation(ticket);

			manager.GetAuthenticationToken(Guid.Empty.ToString(), callback, state);
			wait.WaitOne(30000);

			Assert.IsTrue(callbackWasExecuted, "Callback was not executed. The operation has timed out");
		}
	}
}
