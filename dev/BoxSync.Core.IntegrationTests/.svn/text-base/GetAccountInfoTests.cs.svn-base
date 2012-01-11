using System.Threading;

using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;

using NUnit.Framework;


namespace BoxSync.Core.IntegrationTests
{
	/// <summary>
	/// Set of tests for "GetAccountInfo" method
	/// </summary>
	public class GetAccountInfoTests : IntegrationTestBase
	{
		/// <summary>
		/// Tests synchronous "GetAccountInfo" method
		/// </summary>
		[Test]
		public void TestSyncGetAccountInfo()
		{
			GetAccountInfoResponse response = Context.Manager.GetAccountInfo();

			Assert.AreEqual(GetAccountInfoStatus.Successful, response.Status);

			Assert.IsNull(response.Error);
			Assert.IsNull(response.UserState);

			Assert.IsNotNull(response.User);

			Assert.AreEqual(Context.AuthenticatedUser.AccessID, response.User.AccessID);
			Assert.AreEqual(Context.AuthenticatedUser.ID, response.User.ID);
			Assert.AreEqual(Context.AuthenticatedUser.MaxUploadSize, response.User.MaxUploadSize);
			Assert.AreEqual(Context.AuthenticatedUser.SpaceAmount, response.User.SpaceAmount);
			Assert.AreEqual(Context.AuthenticatedUser.SpaceUsed, response.User.SpaceUsed);

			StringAssert.IsMatch(Context.AuthenticatedUser.Email, response.User.Email);
			StringAssert.IsMatch(Context.AuthenticatedUser.Login, response.User.Login);
		}

		/// <summary>
		/// Tests the behavior of synchronous "GetAccountInfo" method in case if user didn't log in
		/// </summary>
		[Test]
		public void TestSyncGetAccountInfo_WhenUserIsNotLoggedIn_ThenStatusIsNotLoggedIn()
		{
			Context.Manager.Logout();

			GetAccountInfoResponse response = Context.Manager.GetAccountInfo();

			Assert.AreEqual(GetAccountInfoStatus.NotLoggedIn, response.Status);

			Assert.IsNull(response.Error);
			Assert.IsNull(response.UserState);
			Assert.IsNull(response.User);
		}

		/// <summary>
		/// Tests asynchronous "GetAccountInfo" method
		/// </summary>
		[Test]
		public void TestAsyncGetAccountInfo()
		{
			const int status = 847587532;
			ManualResetEvent wait = new ManualResetEvent(false);
			bool callbackWasExecuted = false;

			OperationFinished<GetAccountInfoResponse> callback = resp =>
			                                                     	{
			                                                     		Assert.AreEqual(GetAccountInfoStatus.Successful, resp.Status);
			                                                     		Assert.IsNull(resp.Error);
			                                                     		Assert.IsInstanceOf(typeof(int), resp.UserState);
																		Assert.AreEqual(status, (int)resp.UserState);


																		Assert.IsNotNull(resp.User);



																		Assert.AreEqual(Context.AuthenticatedUser.AccessID, resp.User.AccessID);
																		Assert.AreEqual(Context.AuthenticatedUser.ID, resp.User.ID);
																		Assert.AreEqual(Context.AuthenticatedUser.MaxUploadSize, resp.User.MaxUploadSize);
																		Assert.AreEqual(Context.AuthenticatedUser.SpaceAmount, resp.User.SpaceAmount);
																		Assert.AreEqual(Context.AuthenticatedUser.SpaceUsed, resp.User.SpaceUsed);

																		StringAssert.IsMatch(Context.AuthenticatedUser.Email, resp.User.Email);
																		StringAssert.IsMatch(Context.AuthenticatedUser.Login, resp.User.Login);

			                                                     		callbackWasExecuted = true;

			                                                     		wait.Reset();
			                                                     	};

			Context.Manager.GetAccountInfo(callback, status);

			wait.WaitOne(30000);

			Assert.IsTrue(callbackWasExecuted, "Callback was not executed. The operation has timed out");
		}


		/// <summary>
		/// Tests the behavior of asynchronous "GetAccountInfo" method in case if user didn't log in
		/// </summary>
		[Test]
		public void TestAsyncGetAccountInfo_WhenUserIsNotLoggedIn_ThenStatusIsNotLoggedIn()
		{
			const int status = 847587532;
			ManualResetEvent wait = new ManualResetEvent(false);
			bool callbackWasExecuted = false;

			OperationFinished<GetAccountInfoResponse> callback = resp =>
			                                                     	{
																		Assert.AreEqual(GetAccountInfoStatus.NotLoggedIn, resp.Status);

																		Assert.IsInstanceOf(typeof(int), resp.UserState);
																		Assert.AreEqual(status, (int)resp.UserState);
																		
																		Assert.IsNull(resp.Error);
																		Assert.IsNull(resp.User);

			                                                     		callbackWasExecuted = true;

			                                                     		wait.Reset();
			                                                     	};

			Context.Manager.Logout();

			Context.Manager.GetAccountInfo(callback, status);

			wait.WaitOne(30000);

			Assert.IsTrue(callbackWasExecuted, "Callback was not executed. The operation has timed out");
		}
	}
}
