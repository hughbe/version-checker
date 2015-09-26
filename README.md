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
		<Note>1st release note</Note>
		<Note>2nd release note</Note>
		<Note>3rd release note</Note>
		<Note>4th release note</Note>
	</Notes>
	<Url>http://a.download.link/</Url>
</Version>
```

#Example Use
1. Create a folder on a website of your choice and name it appropriately (e.g. "versions")
2. Add a file titled 
```csharp
var versionsFolderUrl = "...";
var versionChecker = new VersionChecker(versionsFolderUrl);
if (!versionChecker.UpToDate)
{
	var latestVersion = versionChecker.LatestVersion;

	var versionId = latestVersion.Id;
	var releaseDate = latestVersion.Date;
	var releaseNotes = latestVersion.Notes;
	var installationUrl = latestVersion.Url;
	//Handle not up to date
}
```