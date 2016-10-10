using System;
using Xunit;

namespace VersionChecker.Tests
{
	public class VersionNoteTests
    {
        [Fact]
        public void Ctor()
        {
            var note = new VersionNote("Title", "Content");

            Assert.Equal("Title", note.Title);
            Assert.Equal("Content", note.Content);
        }

		[Fact]
		public void Ctor_NullTitle_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>("title", () => new VersionNote(null, "Url"));
		}

		[Fact]
		public void Ctor_NullContent_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>("content", () => new VersionNote("Title", null));
		}

		[Fact]
		public void Ctor_EmptyTitle_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>("title", () => new VersionNote(string.Empty, "Url"));
		}

		[Fact]
		public void Ctor_EmptyContent_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>("content", () => new VersionNote("Title", string.Empty));
		}

		[Fact]
        public void Equals_ReturnsExpected()
        {
            var note1 = new VersionNote("Title", "Content");
            var note2 = new VersionNote("Title", "Content");

            var note3 = new VersionNote("ATitle", "Content");

            var note4 = new VersionNote("Title", "AContent");
            
            Assert.True(note1.Equals(note2));
            Assert.True(note1.GetHashCode().Equals(note2.GetHashCode()));

            Assert.True(note2.Equals(note1));
            Assert.True(note2.GetHashCode().Equals(note1.GetHashCode()));

            Assert.False(note1.Equals(note3));
            Assert.False(note1.Equals(note4));
            Assert.False(note1.Equals(null));
        }
    }
}
