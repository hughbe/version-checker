using System;
using System.Xml;
using VersionChecker;
using Xunit;

namespace VersionCheckerTests
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
            var longDescription = "ShortDescription";

            var date = DateTime.Now;

            var notes = new VersionNotesCollection();
            notes.Add(new VersionNote("Test", "Hi"));

            var urls = new VersionUrlCollection();
            urls.Add(new VersionUrl("Test", "Hi"));

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
            Assert.Throws<ArgumentNullException>(() => new ApplicationVersion(null));
            Assert.Throws<ArgumentException>(() => new ApplicationVersion(""));
        }

        [Fact]
        public void Version_Equality_Test()
        {
            var version1 = new ApplicationVersion("1.1.0.1");
            var version2 = new ApplicationVersion("1.1.0.1");
            var version3 = new ApplicationVersion("1.3.0.1");

            Assert.True(version1.Equals(version2));
            Assert.False(version1.Equals(version3));
            Assert.False(version2.Equals(version3));
        }

        [Fact]
        public void Version_Id_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            var outputXml = version.ToXml();

            Assert.Equal(inputXml, outputXml);
        }

        [Fact]
        public void Version_Id_Serialization_Error_Test_1()
        {
            Assert.Throws<ArgumentNullException>(() => ApplicationVersion.FromXml(null));
            Assert.Throws<ArgumentException>(() => ApplicationVersion.FromXml(""));

            Assert.Throws<XmlException>(() => ApplicationVersion.FromXml("aasdsdasads"));
        }

        [Fact]
        public void Version_Short_Description_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <ShortDescription>Hi</ShortDescription>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            var outputXml = version.ToXml();

            Assert.Equal(inputXml, outputXml);
        }

        [Fact]
        public void Version_Long_Description_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <LongDescription>Hi</LongDescription>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            var outputXml = version.ToXml();

            Assert.Equal(inputXml, outputXml);
        }

        [Fact]
        public void Version_Date_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Date>9/12/2009 9:45:30</Date>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            var outputXml = version.ToXml();

            Assert.Equal(inputXml, outputXml);
        }

        [Fact]
        public void Version_Date_Serialization_Test_1()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Date>09/12/2009</Date>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(version.Date, new DateTime(2009, 12, 09));
        }

        [Fact]
        public void Version_Date_Serialization_Test_2()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Date>09/12/2009 09:30:45</Date>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(version.Date, new DateTime(2009, 12, 09, 09, 30, 45));
        }

        [Fact]
        public void Version_Notes_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Notes>
    <Note>
      <Title>Test</Title>
      <Content>Test</Content>
    </Note>
  </Notes>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            var outputXml = version.ToXml();

            Assert.Equal(inputXml, outputXml);
        }

        [Fact]
        public void Version_Notes_Serialization_Test_1()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Notes>
    <Note><Title>1</Title><Content>1</Content></Note>
    <Note><Title>2</Title><Content>2</Content></Note>
    <Note><Title>3</Title><Content>3</Content></Note>
    <Note><Title>4</Title><Content>4</Content></Note>
    <Note><Title>5</Title><Content>5</Content></Note>
  </Notes>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(version.Notes.Count, 5);
        }

        [Fact]
        public void Version_Notes_Serialization_Test_2()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Notes>
  </Notes>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(version.Notes.Count, 0);
        }

        [Fact]
        public void Version_Notes_Serialization_Test_3()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(version.Notes.Count, 0);
        }

        [Fact]
        public void Version_Notes_Serialization_Error_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Notes>
    <Note><Title>1</Title><Error2>1</Error2></Note>
    <Note><Title>2</Title><Error2>2</Error2></Note>
    <Note><Error1>3</Error1><Content>3</Content></Note>
    <Note><Error1>4</Error1><Content>4</Content></Note>
    <Note><Error1>3</Error1><Error2>5</Error2></Note>
    <Note><Error1>4</Error1><Error2>6</Error2></Note>
  </Notes>
</Version>";
            Assert.Throws<ArgumentException>(() => ApplicationVersion.FromXml(inputXml));
        }

        [Fact]
        public void Version_Urls_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Urls>
    <Url>
      <Title>Test</Title>
      <Url>Test</Url>
    </Url>
  </Urls>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            var outputXml = version.ToXml();

            Assert.Equal(inputXml, outputXml);
        }

        [Fact]
        public void Version_Urls_Serialization_Test_1()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Urls>
    <Url><Title>1</Title><Url>1</Url></Url>
    <Url><Title>2</Title><Url>2</Url></Url>
    <Url><Title>3</Title><Url>3</Url></Url>
    <Url><Title>4</Title><Url>4</Url></Url>
    <Url><Title>5</Title><Url>5</Url></Url>
  </Urls>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);

            Assert.Equal(version.Urls.Count, 5);
        }

        [Fact]
        public void Version_Urls_Serialization_Test_2()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Urls>
  </Urls>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(version.Urls.Count, 0);
        }

        [Fact]
        public void Version_Urls_Serialization_Test_3()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            Assert.Equal(version.Urls.Count, 0);
        }

        [Fact]
        public void Version_Urls_Serialization_Error_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Urls>
    <Url><Title>1</Title><Error2>1</Error2></Url>
    <Url><Title>2</Title><Error2>2</Error2></Url>
    <Url><Error1>3</Error1><Url>3</Url></Url>
    <Url><Error1>4</Error1><Url>4</Url></Url>
    <Url><Error1>3</Error1><Error2>5</Error2></Url>
    <Url><Error1>4</Error1><Error2>6</Error2></Url>
  </Urls>
</Version>";
            Assert.Throws<ArgumentException>(() => ApplicationVersion.FromXml(inputXml));
        }

        [Fact]
        public void Version_Notes_Urls_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Notes>
    <Note>
      <Title>Test</Title>
      <Content>Test</Content>
    </Note>
  </Notes>
  <Urls>
    <Url>
      <Title>Test</Title>
      <Url>Test</Url>
    </Url>
  </Urls>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            var outputXml = version.ToXml();

            Assert.Equal(inputXml, outputXml);
        }

        [Fact]
        public void Version_Copyright_Serialization_Deserialization_Test()
        {
            var inputXml =
@"<Version>
  <Id>1.1.0.0</Id>
  <Copyright>Copyright</Copyright>
</Version>";
            var version = ApplicationVersion.FromXml(inputXml);
            var outputXml = version.ToXml();

            Assert.Equal(inputXml, outputXml);
        }
    }
}
