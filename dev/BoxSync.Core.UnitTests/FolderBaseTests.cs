using System;

using BoxSync.Core.Primitives;
using BoxSync.Core.ServiceReference;

using NUnit.Framework;


namespace BoxSync.Core.UnitTests
{
	[TestFixture]
	public class FolderBaseTests
	{
		[Test]
		public void TestConstructor()
		{
			const long folderID = 375235982;
			string folderName = Guid.NewGuid().ToString();
			const long folderTypeID = 21;
			const long parentFolderID = 3451;
			string password = Guid.NewGuid().ToString();
			string path = Guid.NewGuid().ToString();
			string publicName = Guid.NewGuid().ToString();
			const int shared = 1;
			const int userID = 124124;

			SOAPFolder soapFolder = new SOAPFolder
			                        	{
											folder_id = folderID,
											folder_name = folderName,
											folder_type_id = folderTypeID,
											parent_folder_id = parentFolderID,
											password = password,
											path = path,
											public_name = publicName,
											shared = shared,
											user_id = userID
			                        	};

			FolderBase folder = new FolderBase(soapFolder);

			Assert.AreEqual(folderID, folder.ID);
			StringAssert.IsMatch(folderName, folder.Name);
			Assert.AreEqual(folderTypeID, folder.FolderTypeID);
			Assert.AreEqual(parentFolderID, folder.ParentFolderID);
			StringAssert.IsMatch(password, folder.Password);
			StringAssert.IsMatch(path, folder.Path);
			StringAssert.IsMatch(publicName, folder.PublicName);
			Assert.IsTrue(folder.IsShared.HasValue);
			Assert.IsTrue(folder.IsShared.Value);
			Assert.AreEqual(userID, folder.OwnerID);
		}
	}
}
