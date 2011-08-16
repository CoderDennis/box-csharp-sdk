using System;
using System.Linq;
using System.Threading;

using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;

using NUnit.Framework;


namespace BoxSync.Core.IntegrationTests
{
	/// <summary>
	/// Tests used to test add_comment web-method
	/// </summary>
	public class AddCommentTests : IntegrationTestBase
	{
		[Test]
		public void TestAddComment_Sync_OnFile()
		{
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";
			UploadFileResponse uploadFileResponse = UploadTemporaryFile(Context.Manager);

			AddCommentResponse addCommentResponse = Context.Manager.AddComment(uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID, ObjectType.File, commentText);

			DeleteTemporaryFile(Context.Manager, uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID);

			Assert.IsNotNull(addCommentResponse);
			Assert.AreNotEqual(0, addCommentResponse.PostedComment.ID);
			Assert.AreEqual(AddCommentStatus.Successful, addCommentResponse.Status);
			Assert.AreEqual(Context.AuthenticatedUser.ID, addCommentResponse.PostedComment.UserID);
			Assert.AreEqual(0, addCommentResponse.PostedComment.ReplyComments.Count);
			Assert.AreNotEqual(DateTime.MinValue, addCommentResponse.PostedComment.CreatedOn);

// HAS TO BE CLERIFIED:
//			StringAssert.AreEqualIgnoringCase(commentText, addCommentResponse.PostedComment.Text);
//			StringAssert.AreEqualIgnoringCase(Context.AuthenticatedUser.Login, addCommentResponse.PostedComment.UserName);
		}

		[Test]
		public void TestAddComment_Sync_OnFolder()
		{
			string folderName = Guid.NewGuid().ToString();
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";

			CreateFolderResponse createFolderResponse = CreateFolder(Context.Manager, folderName);

			AddCommentResponse addCommentResponse = Context.Manager.AddComment(createFolderResponse.Folder.ID, ObjectType.Folder, commentText);

			DeleteFolder(Context.Manager, createFolderResponse.Folder.ID);

			Assert.IsNotNull(addCommentResponse);
			Assert.AreNotEqual(0, addCommentResponse.PostedComment.ID);
			Assert.AreEqual(AddCommentStatus.Successful, addCommentResponse.Status);
			Assert.AreEqual(Context.AuthenticatedUser.ID, addCommentResponse.PostedComment.UserID);
			Assert.AreEqual(0, addCommentResponse.PostedComment.ReplyComments.Count);
			Assert.AreNotEqual(DateTime.MinValue, addCommentResponse.PostedComment.CreatedOn);

// HAS TO BE CLERIFIED:
//			StringAssert.AreEqualIgnoringCase(commentText, addCommentResponse.PostedComment.Text);
//			StringAssert.AreEqualIgnoringCase(Context.AuthenticatedUser.Login, addCommentResponse.PostedComment.UserName);
		}

		[Test]
		public void TestAddComment_Sync_OnComment()
		{
			string folderName = Guid.NewGuid().ToString();
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";

			CreateFolderResponse createFolderResponse = CreateFolder(Context.Manager, folderName);

			AddCommentResponse addParentCommentResponse = Context.Manager.AddComment(createFolderResponse.Folder.ID, ObjectType.Folder, commentText);
			AddCommentResponse addCommentResponse = Context.Manager.AddComment(addParentCommentResponse.PostedComment.ID, ObjectType.Comment, commentText);

			DeleteFolder(Context.Manager, createFolderResponse.Folder.ID);

			Assert.IsNotNull(addCommentResponse);
			Assert.AreNotEqual(0, addCommentResponse.PostedComment.ID);
			Assert.AreEqual(AddCommentStatus.Successful, addCommentResponse.Status);
			Assert.AreEqual(Context.AuthenticatedUser.ID, addCommentResponse.PostedComment.UserID);
			Assert.AreEqual(0, addCommentResponse.PostedComment.ReplyComments.Count);
			Assert.AreNotEqual(DateTime.MinValue, addCommentResponse.PostedComment.CreatedOn);

// HAS TO BE CLERIFIED:
//			StringAssert.AreEqualIgnoringCase(commentText, addCommentResponse.PostedComment.Text);
//			StringAssert.AreEqualIgnoringCase(Context.AuthenticatedUser.Login, addCommentResponse.PostedComment.UserName);
		}

		[Test]
		public void TestAddComment_Sync_WhenUserIsLoggedOut_ThenOperationStatusIsNotLoggedIn()
		{
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";
			UploadFileResponse uploadFileResponse = UploadTemporaryFile(Context.Manager);

			Context.Manager.Logout();

			AddCommentResponse addCommentResponse = Context.Manager.AddComment(uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID, ObjectType.File, commentText);

			InitializeContext();

			DeleteTemporaryFile(Context.Manager, uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID);

			Assert.IsNotNull(addCommentResponse);
			Assert.AreEqual(AddCommentStatus.NotLoggedIn, addCommentResponse.Status);

			Assert.IsNull(addCommentResponse.PostedComment);
		}

		[Test]
		public void TestAddComment_Sync_WhenObjectTypeIsFolderAndObjectIDBelongsToFile_ThenOperationStatusIsFailed()
		{
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";
			UploadFileResponse uploadFileResponse = UploadTemporaryFile(Context.Manager);

			AddCommentResponse addCommentResponse = Context.Manager.AddComment(uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID, ObjectType.Folder, commentText);

			DeleteTemporaryFile(Context.Manager, uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID);

			Assert.IsNotNull(addCommentResponse);
			Assert.AreEqual(AddCommentStatus.Failed, addCommentResponse.Status);
			Assert.IsNull(addCommentResponse.PostedComment);
		}

		[Test]
		public void TestAddComment_Sync_WhenCommentTextIsEmpty_ThenOperationStatusIsFailed()
		{
			string commentText = string.Empty;
			UploadFileResponse uploadFileResponse = UploadTemporaryFile(Context.Manager);

			AddCommentResponse addCommentResponse = Context.Manager.AddComment(uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID, ObjectType.File, commentText);

			DeleteTemporaryFile(Context.Manager, uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID);

			Assert.IsNotNull(addCommentResponse);
			Assert.AreEqual(AddCommentStatus.Failed, addCommentResponse.Status);
			Assert.IsNull(addCommentResponse.PostedComment);
		}

		[Test] 
		public void TestAddComment_Sync_WhenCommentTextIsNull_ThenArgumentExceptionIsThrown()
		{
			const string commentText = null;
			UploadFileResponse uploadFileResponse = UploadTemporaryFile(Context.Manager);

			try
			{
				Context.Manager.AddComment(uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID, ObjectType.File, commentText);
			}
			catch (Exception ex)
			{
				Assert.IsInstanceOf(typeof(ArgumentException), ex);
			}
			finally
			{
				DeleteTemporaryFile(Context.Manager, uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID);
			}
		}


		[Test]
		public void TestAddComment_Async_OnFile()
		{
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";
			const long userState = 276365141562342165;
			UploadFileResponse uploadFileResponse = UploadTemporaryFile(Context.Manager);

			ManualResetEvent mre = new ManualResetEvent(false);
			AddCommentResponse addCommentResponse = null;

			Context.Manager.AddComment(uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID, ObjectType.File, commentText,
			                           (arg) =>
			                           	{
			                           		addCommentResponse = arg;

			                           		mre.Set();
			                           	},
										userState);

			DeleteTemporaryFile(Context.Manager, uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID);

			bool isCompletedOnTime = WaitHandle.WaitAll(new[] { mre }, new TimeSpan(0, 0, 0, 15));

			Assert.IsTrue(isCompletedOnTime);
			Assert.IsNotNull(addCommentResponse);
			Assert.AreEqual(AddCommentStatus.Successful, addCommentResponse.Status);
			Assert.AreEqual(Context.AuthenticatedUser.ID, addCommentResponse.PostedComment.UserID);
			Assert.AreEqual(0, addCommentResponse.PostedComment.ReplyComments.Count);
			Assert.AreNotEqual(DateTime.MinValue, addCommentResponse.PostedComment.CreatedOn);

// HAS TO BE CLERIFIED:
//            StringAssert.AreEqualIgnoringCase(commentText, addCommentResponse.PostedComment.Text);
//			StringAssert.AreEqualIgnoringCase(Context.AuthenticatedUser.Login, addCommentResponse.PostedComment.UserName);

			Assert.AreEqual(userState, (long)addCommentResponse.UserState);
		}

		[Test]
		public void TestAddComment_Async_OnFolder()
		{
			string folderName = Guid.NewGuid().ToString();
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";
			const long userState = 763245273458254;

			CreateFolderResponse createFolderResponse = CreateFolder(Context.Manager, folderName);

			ManualResetEvent mre = new ManualResetEvent(false);
			AddCommentResponse addCommentResponse = null;

			Context.Manager.AddComment(createFolderResponse.Folder.ID, ObjectType.Folder, commentText,
			                           (arg) =>
			                           	{
			                           		addCommentResponse = arg;

			                           		mre.Set();
			                           	},
			                           userState);

			DeleteFolder(Context.Manager, createFolderResponse.Folder.ID);

			bool isCompletedOnTime = WaitHandle.WaitAll(new[] { mre }, new TimeSpan(0, 0, 0, 15));

			Assert.IsTrue(isCompletedOnTime);
			Assert.IsNotNull(addCommentResponse);
			Assert.AreNotEqual(0, addCommentResponse.PostedComment.ID);
			Assert.AreEqual(AddCommentStatus.Successful, addCommentResponse.Status);
			Assert.AreEqual(Context.AuthenticatedUser.ID, addCommentResponse.PostedComment.UserID);
			Assert.AreEqual(0, addCommentResponse.PostedComment.ReplyComments.Count);
			Assert.AreNotEqual(DateTime.MinValue, addCommentResponse.PostedComment.CreatedOn);

// HAS TO BE CLERIFIED:
//			StringAssert.AreEqualIgnoringCase(commentText, addCommentResponse.PostedComment.Text);
//			StringAssert.AreEqualIgnoringCase(Context.AuthenticatedUser.Login, addCommentResponse.PostedComment.UserName);

			Assert.AreEqual(userState, (long)addCommentResponse.UserState);
		}

		[Test]
		public void TestAddComment_Async_OnComment()
		{
			string folderName = Guid.NewGuid().ToString();
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";
			const long userState = 283723874652734;

			CreateFolderResponse createFolderResponse = CreateFolder(Context.Manager, folderName);

			AddCommentResponse addParentCommentResponse = Context.Manager.AddComment(createFolderResponse.Folder.ID, ObjectType.Folder, commentText);
			
			ManualResetEvent mre = new ManualResetEvent(false);
			AddCommentResponse addCommentResponse = null;

			Context.Manager.AddComment(addParentCommentResponse.PostedComment.ID, ObjectType.Comment, commentText,
			                           (arg) =>
			                           	{
			                           		addCommentResponse = arg;

			                           		mre.Set();
			                           	},
			                           userState);

			DeleteFolder(Context.Manager, createFolderResponse.Folder.ID);

			bool isCompletedOnTime = WaitHandle.WaitAll(new[] { mre }, new TimeSpan(0, 0, 5, 15));

			Assert.IsTrue(isCompletedOnTime);
			Assert.IsNotNull(addCommentResponse);
			Assert.AreNotEqual(0, addCommentResponse.PostedComment.ID);
			Assert.AreEqual(AddCommentStatus.Successful, addCommentResponse.Status);
			Assert.AreEqual(Context.AuthenticatedUser.ID, addCommentResponse.PostedComment.UserID);
			Assert.AreEqual(0, addCommentResponse.PostedComment.ReplyComments.Count);
			Assert.AreNotEqual(DateTime.MinValue, addCommentResponse.PostedComment.CreatedOn);

// HAS TO BE CLERIFIED:
//			StringAssert.AreEqualIgnoringCase(commentText, addCommentResponse.PostedComment.Text);
//			StringAssert.AreEqualIgnoringCase(Context.AuthenticatedUser.Login, addCommentResponse.PostedComment.UserName);

			Assert.AreEqual(userState, (long)addCommentResponse.UserState);
		}


		[Test]
		public void TestAddComment_Async_WhenUserIsLoggedOut_ThenOperationStatusIsNotLoggedIn()
		{
			const double userState = 12.4545d;
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";
			UploadFileResponse uploadFileResponse = UploadTemporaryFile(Context.Manager);

			Context.Manager.Logout();

			ManualResetEvent mre = new ManualResetEvent(false);
			AddCommentResponse addCommentResponse = null;

			Context.Manager.AddComment(uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID, ObjectType.File, commentText,
									   (arg) =>
									   {
										   addCommentResponse = arg;

										   mre.Set();
									   },
										userState);

			bool isCompletedOnTime = WaitHandle.WaitAll(new[] { mre }, new TimeSpan(0, 0, 0, 15));

			InitializeContext();

			DeleteTemporaryFile(Context.Manager, uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID);

			Assert.IsTrue(isCompletedOnTime);
			Assert.IsNotNull(addCommentResponse);
			Assert.AreEqual(AddCommentStatus.NotLoggedIn, addCommentResponse.Status);
			Assert.IsNull(addCommentResponse.PostedComment);
			Assert.IsNull(addCommentResponse.Error);

			Assert.AreEqual(userState, (double)addCommentResponse.UserState);
		}

		[Test]
		public void TestAddComment_Async_WhenObjectTypeIsFolderAndObjectIDBelongsToFile_ThenOperationStatusIsFailed()
		{
			const double userState = 12.4545d;
			const string commentText = "djkfrbgvdjhfbgd3465346@#$%^&YU*(fjvbg dzjf  idfbgdfjkh ifbg ds";
			UploadFileResponse uploadFileResponse = UploadTemporaryFile(Context.Manager);

			ManualResetEvent mre = new ManualResetEvent(false);
			AddCommentResponse addCommentResponse = null;

			Context.Manager.AddComment(uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID, ObjectType.Folder, commentText,
									   (arg) =>
									   {
										   addCommentResponse = arg;

										   mre.Set();
									   },
										userState);

			bool isCompletedOnTime = WaitHandle.WaitAll(new[] { mre }, new TimeSpan(0, 0, 0, 15));

			DeleteTemporaryFile(Context.Manager, uploadFileResponse.UploadedFileStatus.Keys.ElementAt(0).ID);

			Assert.IsTrue(isCompletedOnTime);
			Assert.IsNotNull(addCommentResponse);
			Assert.AreEqual(AddCommentStatus.Failed, addCommentResponse.Status);
			Assert.IsNull(addCommentResponse.PostedComment);
			Assert.IsNull(addCommentResponse.Error);
			Assert.AreEqual(userState, (double)addCommentResponse.UserState);
		}
	}
}
