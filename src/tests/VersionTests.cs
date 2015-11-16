using System;
using System.Collections.ObjectModel;
using System.Xml;
using Xunit;

namespace VersionChecker.Tests
{
    public class VersionTests
    {
        [Fact]
        public void Version_Constructor_Test_1()
        {
            var version = new ApplicationVersion("1.1.0.1");
            Assert.Equal(version.Id, "1.1.0.1");

            Assert.Equal(version.ShortDescription, null);
            Assert.Equal(version.LongDescription, null);

            Assert.Equal(version.Date, DateTime.MinValue);

            Assert.Equal(version.Notes, null);
            Assert.Equal(version.Urls, null);

            Assert.Equal(version.Copyright, null);
        }

        [Fact]
        public void Version_Constructor_Test_2()
        {
            var versionId = "1.1.0.1";

            var shortDescription = "ShortDescription";
            var longDescription = "LongDescription";

            var date = DateTime.Now;

            var notes = new Collection<VersionNote>
            {
                new VersionNote("Test", "Hi")
            };

            var urls = new Collection<VersionUrl>
            {
                new VersionUrl("Google", "http://google.com")
            };

            var copyright = "(C) Hugh Bellamy 2015";

            var version = new ApplicationVersion(versionId,  shortDescription, longDescription, date, notes, urls, copyright);
            Assert.Equal(version.Id, versionId);

            Assert.Equal(version.ShortDescription, shortDescription);
            Assert.Equal(version.LongDescription, longDescription);

            Assert.Equal(version.Date, date);

            Assert.Equal(version.Notes, notes);
            Assert.Equal(version.Notes.Count, notes.Count);

            Assert.Equal(version.Urls, urls);
            Assert.Equal(version.Urls.Count, urls.Count);

            Assert.Equal(version.Copyright, copyright);
        }

        [Fact]
        public void Version_Constructor_Test_3()
        {
            Assert.Throws<ArgumentNullException>("id", () => new ApplicationVersion(null));
            Assert.Throws<ArgumentException>("id", () => new ApplicationVersion(""));
        }

        [Theory]
        [InlineData("1.1", "1.1", true)]
        [InlineData("3.2.1.0", "3.2.1.0", true)]
        [InlineData("1.1", "1.0", false)]
        [InlineData("1.0", "1.1", false)]
        [InlineData("2.0", "1.1", false)]
        [InlineData("2.0", "2.1", false)]
        public void Version_Equality_Test(string versionId1, string versionId2, bool expected)
        {
            var version1 = new ApplicationVersion(versionId1);
            var version2 = new ApplicationVersion(versionId2);

            if (expected)
            {
                Assert.True(version1.Equals(version2));
            }
            else 
            {
                Assert.False(version1.Equals(version2));
            }
        }

        [Fact]
        public void Version_Null_Equality_Test()
        {
            var version = new ApplicationVersion("1.0");
            Assert.False(version.Equals(null));
        }

        [Theory]
        [InlineData("1.1.0.0")]
        [InlineData("2.0")]
        public void Version_Id_Serialization_Deserialization_Test(string input)
        {
            var inputXml =
@"<Version>
  <Id>" + input + @"</Id>
</Version>";
            var inputVersion = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(inputVersion.Id, input);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.Id, outputVersion.Id);
        }

        [Fact]
        public void Version_Id_Serialization_Error_Test()
        {
            Assert.Throws<ArgumentNullException>("xml", () => ApplicationVersion.FromXml(null));

            Assert.Throws<XmlException>(() => ApplicationVersion.FromXml(""));
            Assert.Throws<XmlException>(() => ApplicationVersion.FromXml("aasdsdasads"));
        }

        [Theory]
        [InlineData("Short Description 1")]
        [InlineData("2nd Short Description")]
        public void Version_Short_Description_Serialization_Deserialization_Test(string input)
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <ShortDescription>" + input + @"</ShortDescription>
</Version>";
            var inputVersion = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(inputVersion.ShortDescription, input);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.ShortDescription, outputVersion.ShortDescription);
        }

        [Theory]
        [InlineData("Long Description 1")]
        [InlineData("2nd Long Description")]
        public void Version_Long_Description_Serialization_Deserialization_Test(string input)
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <LongDescription>" + input + @"</LongDescription>
</Version>";
            var inputVersion = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(inputVersion.LongDescription, input);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.LongDescription, outputVersion.LongDescription);
        }

        [Theory]
        [InlineData("9/12/2009 9:45:30")]
        [InlineData("8/11/2012 10:15:50")]
        [InlineData("9/12/2009")]
        [InlineData("8/11/2012")]
        public void Version_Date_Serialization_Deserialization_Test(string input)
        {
            var date = Convert.ToDateTime(input);

            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Date>" + input + @"</Date>
</Version>";
            var inputVersion = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(inputVersion.Date, date);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.Date, outputVersion.Date);
        }

        [Theory]
        [InlineData("input title", "input content", 5)]
        [InlineData("some title", "some content", 3)]
        public void Version_Notes_Serialization_Deserialization_Test(string input1, string input2, int count)
        {
            var inputXml =
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

            var inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(inputVersion.Notes.Count, count);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);
            
            Assert.Equal(inputVersion.Notes.Count, outputVersion.Notes.Count);
            Assert.Equal(inputVersion.Notes, outputVersion.Notes);
        }

        [Fact]
        public void Version_Empty_Notes_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Notes>
  </Notes>
</Version>";

            var inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(inputVersion.Notes.Count, 0);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.Notes, outputVersion.Notes);
            Assert.Equal(inputVersion.Notes.Count, outputVersion.Notes.Count);
        }

        [Fact]
        public void Version_No_Notes_Serialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
</Version>";

            var inputVersion = ApplicationVersion.FromXml(inputXml);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Null(inputVersion.Notes);
            Assert.Null(outputVersion.Notes);
        }

        [Theory]
        [InlineData("input title", "input url", 5)]
        [InlineData("some title", "some url", 3)]
        public void Version_Urls_Serialization_Deserialization_Test(string input1, string input2, int count)
        {
            var inputXml =
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

            var inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(inputVersion.Urls.Count, count);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);
            
            Assert.Equal(inputVersion.Urls.Count, outputVersion.Urls.Count);
            Assert.Equal(inputVersion.Urls, outputVersion.Urls);
        }

        [Fact]
        public void Version_Empty_Urls_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Urls>
  </Urls>
</Version>";

            var inputVersion = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(inputVersion.Urls.Count, 0);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.Urls, outputVersion.Urls);
            Assert.Equal(inputVersion.Urls.Count, outputVersion.Urls.Count);
        }

        [Fact]
        public void Version_No_Urls_Serialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
</Version>";

            var inputVersion = ApplicationVersion.FromXml(inputXml);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Null(inputVersion.Notes);
            Assert.Null(outputVersion.Notes);
        }
        
        [Theory]
        [InlineData("Some Copyright")]
        [InlineData("")]
        public void Version_Copyright_Serialization_Deserialization_Test(string input)
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Copyright>" + input + @"</Copyright>
</Version> ";
            var inputVersion = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(inputVersion.Copyright, input);

            var outputXml = inputVersion.ToXml();
            var outputVersion = ApplicationVersion.FromXml(outputXml);

            Assert.Equal(inputVersion.Copyright, outputVersion.Copyright);
        }

        [Fact]
        public void Version_Url_Constructor_Test()
        {
            var url = new VersionUrl("Title", "Url");

            Assert.Equal(url.Title, "Title");
            Assert.Equal(url.Url, "Url");
        }

        [Fact]
        public void Version_Url_Invalid_Constructor_Test()
        {
            Assert.Throws<ArgumentNullException>("title", () => new VersionUrl(null, "Url")); //Title is null
            Assert.Throws<ArgumentNullException>("url", () => new VersionUrl("Title", null)); //Url is null

            Assert.Throws<ArgumentException>("title", () => new VersionUrl("", "Url")); //Title is empty
            Assert.Throws<ArgumentException>("url", () => new VersionUrl("Title", "")); //Url is empty
        }

        [Fact]
        public void Version_Url_Equality_Test()
        {
            var url1 = new VersionUrl("Title", "Url");
            var url2 = new VersionUrl("Title", "Url");
            
            var url3 = new VersionUrl("ATitle", "Url");

            var url4 = new VersionUrl("Title", "AUrl");
            
            Assert.True(url1.Equals(url2));
            Assert.True(url1.GetHashCode().Equals(url2.GetHashCode()));

            Assert.True(url2.Equals(url1));
            Assert.True(url2.GetHashCode().Equals(url1.GetHashCode()));

            Assert.False(url1.Equals(url3));
            Assert.False(url1.Equals(url4));
            Assert.False(url1.Equals(null));
        }

        [Fact]
        public void Version_Note_Constructor_Test()
        {
            var note = new VersionNote("Title", "Content");

            Assert.Equal(note.Title, "Title");
            Assert.Equal(note.Content, "Content");
        }

        [Fact]
        public void Version_Note_Invalid_Constructor_Test()
        {
            Assert.Throws<ArgumentNullException>("title", () => new VersionNote(null, "Content")); //Title is null
            Assert.Throws<ArgumentNullException>("content", () => new VersionNote("Title", null)); //Content is null

            Assert.Throws<ArgumentException>("title", () => new VersionNote("", "Content")); //Title is empty
            Assert.Throws<ArgumentException>("content", () => new VersionNote("Title", "")); //Content is empty
        }

        [Fact]
        public void Version_Note_Equality_Test()
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
