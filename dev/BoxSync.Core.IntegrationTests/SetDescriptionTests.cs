using System;
using System.Linq;

using BoxSync.Core.Primitives;
using BoxSync.Core.Statuses;

using NUnit.Framework;


namespace BoxSync.Core.IntegrationTests
{
	/// <summary>
	/// Set of tests for BoxManager.SetDescription(...) method
	/// </summary>
	[TestFixture]
	public class SetDescriptionTests : IntegrationTestBase
	{
		[Test]
		public void TestSetDescription_WhenObjectIsFileAndDescriptionIsNotEmpty_ThenResultIsSuccess()
		{
			const string fileDescription = "skdhvjbsjkdhvbfjkdshvbjdshgvfjdshv";
			byte[] fileContent = new byte[0];
			UploadFileResponse uploadResponse = UploadTemporaryFile(Context.Manager, fileContent, 0);

			File uploadedFileInfo = uploadResponse.UploadedFileStatus.Keys.ElementAt(0);

			SetDescriptionStatus status = Context.Manager.SetDescription(uploadedFileInfo.ID, ObjectType.File, fileDescription);
			GetFileInfoResponse fileInfo = Context.Manager.GetFileInfo(uploadedFileInfo.ID);

			DeleteTemporaryFile(Context.Manager, uploadedFileInfo.ID);

			Assert.AreEqual(SetDescriptionStatus.Successful, status);
			StringAssert.AreEqualIgnoringCase(fileDescription, fileInfo.File.Description);
		}

		[Test]
		public void TestSetDescription_WhenObjectIsFileAndDescriptionIsEmpty_ThenResultIsSuccess()
		{
			byte[] fileContent = new byte[0];
			UploadFileResponse uploadResponse = UploadTemporaryFile(Context.Manager, fileContent, 0);

			File uploadedFileInfo = uploadResponse.UploadedFileStatus.Keys.ElementAt(0);

			SetDescriptionStatus status = Context.Manager.SetDescription(uploadedFileInfo.ID, ObjectType.File, string.Empty);
			GetFileInfoResponse fileInfo = Context.Manager.GetFileInfo(uploadedFileInfo.ID);

			Assert.AreEqual(SetDescriptionStatus.Successful, status);
			Assert.IsEmpty(fileInfo.File.Description);
		}


		[Test]
		public void TestSetDescription_WhenObjectIsFolderAndDescriptionIsNotEmpty_ThenResultIsSuccess()
		{
			const string folderDescription = "skdhvjbsjkdfdbdfhvbfjkdshvbjdshgvfjdshv";

			CreateFolderResponse createFolderResponse = Context.Manager.CreateFolder(Guid.NewGuid().ToString(), 0, false);
			SetDescriptionStatus status = Context.Manager.SetDescription(createFolderResponse.Folder.ID, ObjectType.Folder, folderDescription);
			GetFolderStructureResponse folderStructure = Context.Manager.GetFolderStructure(createFolderResponse.Folder.ID,
																			  RetrieveFolderStructureOptions.NoFiles |
																			  RetrieveFolderStructureOptions.OneLevel);

			DeleteFolder(Context.Manager, createFolderResponse.Folder.ID);

			Assert.AreEqual(SetDescriptionStatus.Successful, status);
			StringAssert.AreEqualIgnoringCase(folderDescription, folderStructure.Folder.Description);
		}

		[Test]
		public void TestSetDescription_WhenObjectIsFolderAndDescriptionIsEmpty_ThenResultIsSuccess()
		{
			CreateFolderResponse createFolderResponse = Context.Manager.CreateFolder(Guid.NewGuid().ToString(), 0, false);

			SetDescriptionStatus status = Context.Manager.SetDescription(createFolderResponse.Folder.ID, ObjectType.Folder, string.Empty);
			GetFolderStructureResponse folderStructure = Context.Manager.GetFolderStructure(createFolderResponse.Folder.ID,
			                                                                  RetrieveFolderStructureOptions.NoFiles |
			                                                                  RetrieveFolderStructureOptions.OneLevel);

			DeleteFolder(Context.Manager, createFolderResponse.Folder.ID);

			Assert.AreEqual(SetDescriptionStatus.Successful, status);
			Assert.IsEmpty(folderStructure.Folder.Description);
		}


		[Test]
		public void TestSetDescription_WhenObjectIDBelongsToFileAndMethodIsCalledWithFileObjectType_ThenCallStatusIsFailed()
		{
			const string folderDescription = "skdhvjbsjkdfdbdfhvbfjkdshvbjdshgvfjdshv";
			byte[] fileContent = new byte[0];
			UploadFileResponse uploadResponse = UploadTemporaryFile(Context.Manager, fileContent, 0);

			File uploadedFileInfo = uploadResponse.UploadedFileStatus.Keys.ElementAt(0);

			CreateFolderResponse createFolderResponse = Context.Manager.CreateFolder(Guid.NewGuid().ToString(), 0, false);
			SetDescriptionStatus status = Context.Manager.SetDescription(uploadedFileInfo.ID, ObjectType.Folder, folderDescription);
			
			DeleteTemporaryFile(Context.Manager, uploadedFileInfo.ID);
			DeleteFolder(Context.Manager, createFolderResponse.Folder.ID);

			Assert.AreEqual(SetDescriptionStatus.Failed, status);
		}

		[Test]
		public void TestSetDescription_WhenUserHasLoggedOut_ThenCallStatusIsNotLoggedID()
		{
			const string fileDescription = "skdhvjbsjkdhvbfjkdshvbjdshgvfjdshv";
			byte[] fileContent = new byte[0];
			UploadFileResponse uploadResponse = UploadTemporaryFile(Context.Manager, fileContent, 0);

			File uploadedFileInfo = uploadResponse.UploadedFileStatus.Keys.ElementAt(0);

			Context.Manager.Logout();

			SetDescriptionStatus status = Context.Manager.SetDescription(uploadedFileInfo.ID, ObjectType.File, fileDescription);
			
			InitializeContext();

			GetFileInfoResponse fileInfo = Context.Manager.GetFileInfo(uploadedFileInfo.ID);

			DeleteTemporaryFile(Context.Manager, uploadedFileInfo.ID);

			Assert.AreEqual(SetDescriptionStatus.NotLoggedID, status);
			Assert.IsEmpty(fileInfo.File.Description);
		}
	}
}
