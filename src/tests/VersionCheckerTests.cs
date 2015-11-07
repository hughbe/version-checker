using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace VersionChecker.Tests
{
    public class VersionCheckerTests
    {
        public static string VersionsLocation { get; } = "https://raw.githubusercontent.com/hughbe/version-checker/master/resources/versions";
        public static ApplicationVersion OnlineCurrentVersion { get; } = new ApplicationVersion("1.1.0.0");

        public static ApplicationVersionChecker OnlineCurrentVersionChecker() =>
            new ApplicationVersionChecker(VersionsLocation, OnlineCurrentVersion);

        [Theory]
        [InlineData("1.1.0.1", "url")]
        [InlineData("2.3.4.5", "test")]
        public void Constructor_Test(string versionId, string versionsLocation)
        {
            var currentVersion = new ApplicationVersion(versionId);
            var versionChecker = new ApplicationVersionChecker(versionsLocation, currentVersion);

            Assert.Equal(versionChecker.VersionsLocation, versionsLocation);
            Assert.Equal(versionChecker.CurrentVersion, currentVersion);
        }

        [Fact]
        public void Invalid_Constructor_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new ApplicationVersionChecker(null, OnlineCurrentVersion));
            Assert.Throws<ArgumentException>(() => new ApplicationVersionChecker("", OnlineCurrentVersion));

            Assert.Throws<ArgumentNullException>(() => new ApplicationVersionChecker(VersionsLocation, null));
        }
        
        [Theory]
        [InlineData("current1")]
        [InlineData("current2")]
        public void Current_Version_Name_Test(string newCurrentName)
        {
            var versionChecker = OnlineCurrentVersionChecker();

            versionChecker.LatestVersionName = newCurrentName;
            Assert.True(versionChecker.LatestVersionName == newCurrentName);

            versionChecker.ResetLatestVersionName();
            Assert.Equal(versionChecker.LatestVersionName, "latestversion");
        }

        [Fact]
        public void Invalid_Current_Name_Test()
        {
            var versionChecker = OnlineCurrentVersionChecker();

            Assert.Throws<ArgumentNullException>(() => versionChecker.LatestVersionName = null);
            Assert.Throws<ArgumentException>(() => versionChecker.LatestVersionName = "");
        }

        [Fact]
        public void Get_Current_Version_Test()
        {
            var versionChecker = OnlineCurrentVersionChecker();
            
            var version = versionChecker.GetVersion("latestversion").Result;
            Assert.Equal(version, OnlineCurrentVersion);

            Assert.True(versionChecker.IsUpToDate().Result);
        }

        [Fact]
        public void Update_Latest_Version()
        {
            var versionChecker = OnlineCurrentVersionChecker();

            var latestVersion = versionChecker.GetLatestVersion().Result;
            Assert.Equal(latestVersion, OnlineCurrentVersion);
        }

        [Fact]
        public async Task Get_Invalid_Version_Test()
        {
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, OnlineCurrentVersion);

            await Assert.ThrowsAsync<ArgumentNullException>(() => versionChecker.GetVersion(null));
            await Assert.ThrowsAsync<ArgumentException>(() => versionChecker.GetVersion(""));
        }

        [Theory]
        [InlineData("1.1.0.0", true)]
        [InlineData("1.0.0.0", false)]
        public async Task Up_To_Date_Test(string input, bool expected)
        {
            var currentVersion = new ApplicationVersion(input);
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            var upToDate = await versionChecker.IsUpToDate();
            Assert.Equal(upToDate, expected);
        }

        [Fact]
        public async Task Invalid_Up_To_Date_Test()
        {
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, OnlineCurrentVersion);
            versionChecker.LatestVersionName = "invalid-data-torlalsdalladla";

            await Assert.ThrowsAsync<HttpRequestException>(async () => await versionChecker.GetLatestVersion());
        }
    }
}
