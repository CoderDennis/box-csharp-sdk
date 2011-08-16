using System;
using System.Linq.Expressions;

using BoxSync.Core.Primitives;

using NUnit.Framework;


namespace BoxSync.Core.UnitTests
{
	[TestFixture]
	public class TagPrimitiveCollectionTests
	{
		[Test]
		public void TestSimpleAdd()
		{
			TagPrimitiveCollection collection = new TagPrimitiveCollection();

			TagPrimitive firstTag = new TagPrimitive(1236, "tag_1236");
			TagPrimitive secondTag = new TagPrimitive(654709, "tag_654709");

			collection.AddTag(firstTag);
			collection.AddTag(secondTag);

			Assert.AreEqual(2, collection.Count);
		}

		[Test]
		public void TestMaterializeAdd()
		{
			TagPrimitiveCollection collection = new TagPrimitiveCollection();

			int firstTagID = 1236;
			int secondTagID = 76409;

			Expression<Func<long, TagPrimitive>> materializeFirstTag = tagID => GetTag(firstTagID);
			Expression<Func<long, TagPrimitive>> materializeSecondTag = tagID => GetTag(secondTagID);

			TagPrimitive firstTag = new TagPrimitive(firstTagID, materializeFirstTag);
			TagPrimitive secondTag = new TagPrimitive(secondTagID, materializeSecondTag);

			collection.AddTag(firstTag);
			collection.AddTag(secondTag);

			Assert.AreEqual(2, collection.Count);

			TagPrimitive firstRetrievedTag = collection.GetTag(firstTagID);

			Assert.AreEqual(firstTag.ID, firstRetrievedTag.ID);
			StringAssert.IsMatch(firstTag.Text, firstRetrievedTag.Text);

			TagPrimitive secondRetrievedTag = collection.GetTag(secondTagID);

			Assert.AreEqual(secondTag.ID, secondRetrievedTag.ID);
			StringAssert.IsMatch(secondTag.Text, secondRetrievedTag.Text);
		}

		[Test]
		public void TestClear()
		{
			TagPrimitiveCollection collection = new TagPrimitiveCollection();

			TagPrimitive firstTag = new TagPrimitive(1236, "tag_1236");
			TagPrimitive secondTag = new TagPrimitive(654709, "tag_654709");

			collection.AddTag(firstTag);
			collection.AddTag(secondTag);

			Assert.AreEqual(2, collection.Count);

			collection.Clear();

			Assert.AreEqual(0, collection.Count);
		}

		[Test]
		public void TestGetEnumerator()
		{
			TagPrimitiveCollection collection = new TagPrimitiveCollection();

			const int firstTagID = 1236;
			const int secondTagID = 76409;

			TagPrimitive firstTag = new TagPrimitive(firstTagID, "tag_1236");
			TagPrimitive secondTag = new TagPrimitive(secondTagID, "tag_654709");

			collection.AddTag(firstTag);
			collection.AddTag(secondTag);

			Assert.AreEqual(2, collection.Count);

			foreach (TagPrimitive tagPrimitive in collection)
			{
				switch (tagPrimitive.ID)
				{
					case firstTagID:
					case secondTagID:
						continue;
					default:
						Assert.Fail(string.Format("TagID = [{0}] shouldn't be in collection", tagPrimitive.ID));
						break;
				}
			}
		}

		[Test]
		public void TestIsEmpty()
		{
			TagPrimitiveCollection collection = new TagPrimitiveCollection();

			Assert.IsTrue(collection.IsEmpty);

			TagPrimitive firstTag = new TagPrimitive(1236, "tag_1236");
			TagPrimitive secondTag = new TagPrimitive(654709, "tag_654709");

			collection.AddTag(firstTag);
			collection.AddTag(secondTag);

			Assert.AreEqual(2, collection.Count);

			collection.Clear();

			Assert.IsTrue(collection.IsEmpty);
		}

		private TagPrimitive GetTag(long tagID)
		{
			return new TagPrimitive(tagID, string.Format("tag_{0}", tagID));
		}
	}
}
