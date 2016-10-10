using System;
using System.Collections.ObjectModel;
using System.Xml;
using Xunit;

namespace VersionChecker.Tests
{
    public class ApplicationVersionTests
    {
        [Fact]
        public void Ctor_String()
        {
            var version = new ApplicationVersion("1.1.0.1");
            Assert.Equal("1.1.0.1", version.Id);

            Assert.Null(version.ShortDescription);
            Assert.Null(version.LongDescription);

            Assert.Equal(DateTime.MinValue, version.Date);

            Assert.Null(version.Notes);
            Assert.Null(version.Urls);

            Assert.Null(version.Copyright);
        }

        [Fact]
        public void Ctor_String_String_String_DateTime_IEnumerable_IEnumerable_String()
        {
            string versionId = "1.1.0.1";

            string shortDescription = "ShortDescription";
            string longDescription = "LongDescription";

            DateTime date = DateTime.Now;

            var notes = new Collection<VersionNote>
            {
                new VersionNote("Test", "Hi")
            };

            var urls = new Collection<VersionUrl>
            {
                new VersionUrl("Google", "http://google.com")
            };

            string copyright = "(C) Hugh Bellamy 2015";

            var version = new ApplicationVersion(versionId,  shortDescription, longDescription, date, notes, urls, copyright);
            Assert.Equal(versionId, version.Id);

            Assert.Equal(shortDescription, version.ShortDescription);
            Assert.Equal(longDescription, version.LongDescription);

            Assert.Equal(date, version.Date);

            Assert.Equal(notes, version.Notes);
            Assert.Equal(urls, version.Urls);

            Assert.Equal(copyright, version.Copyright);
        }

		[Fact]
		public void Ctor_NullId_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>("id", () => new ApplicationVersion(null));
		}

		[Fact]
		public void Ctor_EmptyIf_ThrowsArgumentException()
		{
            Assert.Throws<ArgumentException>("id", () => new ApplicationVersion(""));
        }

        [Theory]
        [InlineData("1.1", "1.1", true)]
        [InlineData("3.2.1.0", "3.2.1.0", true)]
        [InlineData("1.1", "1.0", false)]
        [InlineData("1.0", "1.1", false)]
        [InlineData("2.0", "1.1", false)]
        [InlineData("2.0", "2.1", false)]
        public void Equals_ReturnsExpected(string versionId1, string versionId2, bool expected)
        {
            var version1 = new ApplicationVersion(versionId1);
            var version2 = new ApplicationVersion(versionId2);

			Assert.Equal(expected, version1.Equals(version2));
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            var version = new ApplicationVersion("1.0");
            Assert.False(version.Equals(null));
        }

        [Theory]
        [InlineData("1.1.0.0")]
        [InlineData("2.0")]
        public void Serialize_Deserialize_ReturnsSameId(string input)
        {
            string inputXml =
@"<Version>
  <Id>" + input + @"</Id>
</Version>";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(input, inputVersion.Id);

            string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.Id, outputVersion.Id);
        }

        [Fact]
        public void Serialize_Invalid_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>("xml", () => ApplicationVersion.FromXml(null));

            Assert.Throws<XmlException>(() => ApplicationVersion.FromXml(""));
            Assert.Throws<XmlException>(() => ApplicationVersion.FromXml("aasdsdasads"));
        }

        [Theory]
        [InlineData("Short Description 1")]
        [InlineData("2nd Short Description")]
        public void Serialize_Deserialize_ReturnsSameShortDescription(string input)
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <ShortDescription>" + input + @"</ShortDescription>
</Version>";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(input, inputVersion.ShortDescription);

            string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.ShortDescription, outputVersion.ShortDescription);
        }

        [Theory]
        [InlineData("Long Description 1")]
        [InlineData("2nd Long Description")]
        public void Serialize_Deserialize_ReturnsSameLongDescription(string input)
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <LongDescription>" + input + @"</LongDescription>
</Version>";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(input, inputVersion.LongDescription);

            string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Equal(inputVersion.LongDescription, outputVersion.LongDescription);
        }

        [Theory]
        [InlineData("9/12/2009 9:45:30")]
        [InlineData("8/11/2012 10:15:50")]
        [InlineData("9/12/2009")]
        [InlineData("8/11/2012")]
        public void Serialize_Deserialize_ReturnsSameDate(string input)
        {
            DateTime date = Convert.ToDateTime(input);
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Date>" + input + @"</Date>
</Version>";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(date, inputVersion.Date);

            string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Equal(inputVersion.Date, outputVersion.Date);
        }

        [Theory]
        [InlineData("input title", "input content", 5)]
        [InlineData("some title", "some content", 3)]
        public void Serialize_Deserialize_ReturnsSameNonEmptyNotes(string input1, string input2, int count)
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Notes>";
            for (int i = 0; i < count; i++)
            {
                inputXml += "    <Note><Title> " +  input1 + "</Title><Content>" + input2 + "</Content></Note>";
            }            
            inputXml +=  @"
  </Notes>
</Version>";

            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(inputVersion.Notes.Count, count);

            string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Equal(inputVersion.Notes, outputVersion.Notes);
        }

        [Fact]
        public void Serialize_Deserialize_ReturnsSameEmptyNotes()
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Notes>
  </Notes>
</Version>";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Empty(inputVersion.Notes);

            string outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Equal(inputVersion.Notes, outputVersion.Notes);
        }

        [Fact]
        public void Serialize_Deserialize_NoNotes()
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
</Version>";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
			Assert.Null(inputVersion.Notes);

			string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Null(outputVersion.Notes);
        }

        [Theory]
        [InlineData("input title", "input url", 5)]
        [InlineData("some title", "some url", 3)]
        public void Serialize_Deserialize_ReturnsSameNonEmptyUrls(string input1, string input2, int count)
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Urls>";
            for (int i = 0; i < count; i++)
            {
                inputXml += "    <Url><Title> " + input1 + "</Title><Url>" + input2 + "</Url></Url>";
            }
            inputXml += @"
    </Urls>
</Version>";

            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(count, inputVersion.Urls.Count);

            string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Equal(inputVersion.Urls, outputVersion.Urls);
        }

        [Fact]
        public void Serialize_Deserialize_ReturnsSameEmptyUrls()
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Urls>
  </Urls>
</Version>";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Empty(inputVersion.Urls);

            string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Equal(inputVersion.Urls, outputVersion.Urls);
        }

        [Fact]
        public void Serialize_Deserialize_NoUrls()
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
</Version>";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
			Assert.Null(inputVersion.Notes);

			string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Null(outputVersion.Notes);
        }
        
        [Theory]
        [InlineData("Some Copyright")]
        [InlineData("")]
        public void Serialize_Deserialize_ReturnsSameCopyrigth(string input)
        {
            string inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Copyright>" + input + @"</Copyright>
</Version> ";
            ApplicationVersion inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(input, inputVersion.Copyright);

            string outputXml = inputVersion.ToXml();
            ApplicationVersion outputVersion = ApplicationVersion.FromXml(outputXml);
            Assert.Equal(inputVersion.Copyright, outputVersion.Copyright);
        }
    }
}
