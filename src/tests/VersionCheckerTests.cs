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
        public void VersionChecker_Constructor_Test(string versionId, string versionsLocation)
        {
            var currentVersion = new ApplicationVersion(versionId);
            var versionChecker = new ApplicationVersionChecker(versionsLocation, currentVersion);

            Assert.Equal(versionChecker.VersionsLocation, versionsLocation);
            Assert.Equal(versionChecker.CurrentVersion, currentVersion);
        }

        [Fact]
        public void VersionChecker_Invalid_Constructor_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new ApplicationVersionChecker(null, OnlineCurrentVersion));
            Assert.Throws<ArgumentException>(() => new ApplicationVersionChecker("", OnlineCurrentVersion));

            Assert.Throws<ArgumentNullException>(() => new ApplicationVersionChecker(VersionsLocation, null));
        }
        
        [Theory]
        [InlineData("current1")]
        [InlineData("current2")]
        public void VersionChecker_Current_Version_Name_Test(string newCurrentName)
        {
            var versionChecker = OnlineCurrentVersionChecker();

            versionChecker.CurrentVersionName = newCurrentName;
            Assert.True(versionChecker.CurrentVersionName == newCurrentName);

            versionChecker.ResetCurrentVersionName();
            Assert.Equal(versionChecker.CurrentVersionName, "currentversion");
        }

        [Fact]
        public void VersionChecker_Invalid_Current_Name_Test()
        {
            var versionChecker = OnlineCurrentVersionChecker();

            Assert.Throws<ArgumentNullException>(() => versionChecker.CurrentVersionName = null);
            Assert.Throws<ArgumentException>(() => versionChecker.CurrentVersionName = "");
        }

        [Fact]
        public async Task VersionChecker_Get_Current_Version_Test()
        {
            var versionChecker = OnlineCurrentVersionChecker();
            
            var version = await versionChecker.GetVersion("currentversion");
            Assert.Equal(version, OnlineCurrentVersion);

            Assert.True(versionChecker.IsUpToDate().Result);
        }

        [Fact]
        public async Task VersionChecker_Update_Latest_Version()
        {
            var versionChecker = OnlineCurrentVersionChecker();

            await versionChecker.UpdateLatestVersion();

            var version = versionChecker.LatestVersion;
            Assert.Equal(version, OnlineCurrentVersion);
            
            Assert.True(versionChecker.IsUpToDate().Result);
        }

        [Fact]
        public async Task VersionChecker_Get_Invalid_Version_Test()
        {
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, OnlineCurrentVersion);

            await Assert.ThrowsAsync<ArgumentNullException>(() => versionChecker.GetVersion(null));
            await Assert.ThrowsAsync<ArgumentException>(() => versionChecker.GetVersion(""));
        }

        [Theory]
        [InlineData("1.1.0.0", true)]
        [InlineData("1.0.0.0", false)]
        public async Task VersionChecker_Up_To_Date_Test(string input, bool expected)
        {
            var currentVersion = new ApplicationVersion(input);
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            var upToDate = await versionChecker.IsUpToDate();
            Assert.Equal(upToDate, expected);
        }

        [Fact]
        public async Task VersionChecker_Invalid_Up_To_Date_Test()
        {
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, OnlineCurrentVersion);
            versionChecker.CurrentVersionName = "invalid-data-torlalsdalladla";

            await Assert.ThrowsAsync<HttpRequestException>(async () => await versionChecker.UpdateLatestVersion());
        }
    }
}
