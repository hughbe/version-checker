# Version Checker
A drop-in C# library that simplifies version checking for applications of all types.

![alt text](https://github.com/hughbe/version-checker/blob/master/resources/screenshots/1.png "Screenshot 1")

#Version Format
The format of versions is XML. An example is structured as follows (all but the `Id` element are optional):
```xml
<Version>
  <Id>1.0.0.0</Id>
  <Date>26/09/15</Date>
  <Notes>
    <Note>
      <Title>1st release note</Title>
      <Content>...</Content>
    </Note>
    <Note>
      <Title>2nd release note</Title>
      <Content>...</Content>
    </Note>
  </Notes>
  <Urls>
    <Url>
      <Title>Download Link</Title>
      <Content>http://a.download.link</Content>
    </Url>
  </Urls>
</Version>
```

#Example Use
1. Create a folder on a website of your choice and name it appropriately (e.g. "versions")
2. Add a file titled `latestversion.xml` with the format above
```csharp
var versionsLocation = "...";
var currentVersion = new ApplicationVersion("..."); //Version identifier (e.g. 1.1.0.0)
var versionChecker = new ApplicationVersionChecker(versionsLocation, currentVersion)
try 
{
    var upToDate = versionChecker.IsUpToDate();
    
    if (!versionChecker.UpToDate)
    {
        var latestVersion = versionChecker.LatestVersion;

        var versionId = latestVersion.Id;
        var releaseDate = latestVersion.Date;
        var releaseNotes = latestVersion.Notes;
        var urls = latestVersion.Urls;
        //...
     }
}
catch (Exception)
{
   //Could not get or parse the version xml file
}
```
