using BoxSync.Core.Primitives;

using NUnit.Framework;


namespace BoxSync.Core.UnitTests.Primitives
{
	[TestFixture]
	public class FolderStructureRetrieveModeExtensionsTests
	{
		[Test]
		public void TestToStringArray_WhenNoOptionsWereSpecified_ThenEmptyStringArrayReturned()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.None;

			string[] convertedOptions = options.ToStringArray();

			Assert.IsEmpty(convertedOptions);
		}

		[Test]
		public void TestToStringArray_WhenNoFilesOptionWasSpecified_ThenArrayWithOneElementReturned()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.NoFiles;

			string[] convertedOptions = options.ToStringArray();

			Assert.AreEqual(1, convertedOptions.Length);
			StringAssert.AreEqualIgnoringCase("nofiles", convertedOptions[0]);
		}

		[Test]
		public void TestToStringArray_WhenNoZipOptionWasSpecified_ThenArrayWithOneElementReturned()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.NoZip;

			string[] convertedOptions = options.ToStringArray();

			Assert.AreEqual(1, convertedOptions.Length);
			StringAssert.AreEqualIgnoringCase("nozip", convertedOptions[0]);
		}

		[Test]
		public void TestToStringArray_WhenOneLevelOptionWasSpecified_ThenArrayWithOneElementReturned()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.OneLevel;

			string[] convertedOptions = options.ToStringArray();

			Assert.AreEqual(1, convertedOptions.Length);
			StringAssert.AreEqualIgnoringCase("onelevel", convertedOptions[0]);
		}

		[Test]
		public void TestToStringArray_WhenSimpleOptionWasSpecified_ThenArrayWithOneElementReturned()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.Simple;

			string[] convertedOptions = options.ToStringArray();

			Assert.AreEqual(1, convertedOptions.Length);
			StringAssert.AreEqualIgnoringCase("simple", convertedOptions[0]);
		}

		[Test]
		public void TestToStringArray_WhenNoFilesAndNoZipOptionsWereSpecified_ThenArrayWithTwoElementsReturned()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.NoZip | RetrieveFolderStructureOptions.NoFiles;

			string[] convertedOptions = options.ToStringArray();

			Assert.AreEqual(2, convertedOptions.Length);

			Assert.Contains("nozip", convertedOptions);
			Assert.Contains("nofiles", convertedOptions);
		}

		[Test]
		public void TestToStringArray_WhenAllOptionsWasSpecified_ThenResultArrayContainsAllElements()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.NoZip | RetrieveFolderStructureOptions.NoFiles |
				RetrieveFolderStructureOptions.OneLevel | RetrieveFolderStructureOptions.Simple;

			string[] convertedOptions = options.ToStringArray();

			Assert.AreEqual(4, convertedOptions.Length);

			Assert.Contains("nozip", convertedOptions);
			Assert.Contains("nofiles", convertedOptions);
			Assert.Contains("simple", convertedOptions);
			Assert.Contains("onelevel", convertedOptions);
		}

		[Test]
		public void TestContains_WhenTheOptionsFlagArrayContainsElement_ThenTrueIsReturned()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.Simple | RetrieveFolderStructureOptions.OneLevel;

			Assert.IsTrue(options.Contains(RetrieveFolderStructureOptions.Simple));
		}

		[Test]
		public void TestContains_WhenTheOptionsFlagArrayDoesNotContainElement_ThenFalseIsReturned()
		{
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.NoZip | RetrieveFolderStructureOptions.OneLevel;

			Assert.IsFalse(options.Contains(RetrieveFolderStructureOptions.Simple));
		}

		[Test]
		public void TestContains_WhenElementPassedInTheArgumentIsNotDefinedInEnum_ThenFalseIsReturned()
		{
			const RetrieveFolderStructureOptions notDefinedElement = (RetrieveFolderStructureOptions) 25;
			const RetrieveFolderStructureOptions options = RetrieveFolderStructureOptions.NoZip | RetrieveFolderStructureOptions.OneLevel;

			Assert.IsFalse(options.Contains(notDefinedElement));
		}
	}
}
