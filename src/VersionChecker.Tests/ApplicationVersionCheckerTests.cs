using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace VersionChecker.Tests
{
    public class ApplicationVersionCheckerTests
    {
        public static string VersionsLocation { get; } = "https://raw.githubusercontent.com/hughbe/version-checker/master/resources/versions";
        public static ApplicationVersion OnlineCurrentVersion { get; } = new ApplicationVersion("1.1.0.0");

        public static ApplicationVersionChecker OnlineCurrentVersionChecker() => new ApplicationVersionChecker(VersionsLocation, OnlineCurrentVersion);

        [Theory]
        [InlineData("1.1.0.1", "url")]
        [InlineData("2.3.4.5", "test")]
        public void Ctor(string versionId, string versionsLocation)
        {
            var currentVersion = new ApplicationVersion(versionId);
            var versionChecker = new ApplicationVersionChecker(versionsLocation, currentVersion);

            Assert.Equal(versionsLocation, versionChecker.VersionsLocation);
            Assert.Equal(currentVersion, versionChecker.CurrentVersion);
        }

        [Fact]
        public void Ctor_NullVersionsLocation_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("versionsLocation", () => new ApplicationVersionChecker(null, OnlineCurrentVersion));
		}

		[Fact]
		public void Ctor_NullCurrentVersionThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>("currentVersion", () => new ApplicationVersionChecker(VersionsLocation, null));
		}

		[Fact]
		public void Ctor_EmptyVersionsLocation_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>("versionsLocation", () => new ApplicationVersionChecker("", OnlineCurrentVersion));
		}
        
        [Theory]
        [InlineData("current1")]
        [InlineData("current2")]
        public void LatestVersionName_Set_Get_ReturnsExpected(string value)
        {
            ApplicationVersionChecker versionChecker = OnlineCurrentVersionChecker();

            versionChecker.LatestVersionName = value;
            Assert.Equal(value, versionChecker.LatestVersionName);

            versionChecker.ResetLatestVersionName();
            Assert.Equal("latestversion", versionChecker.LatestVersionName);
        }

		[Fact]
		public void LatestVersionName_SetNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>("value", () => OnlineCurrentVersionChecker().LatestVersionName = null);
		}

		[Fact]
		public void LatestVersionName_SetEmpty_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>("value", () => OnlineCurrentVersionChecker().LatestVersionName = "");
        }

        [Fact]
        public async Task GetVersion_ReturnsExpected()
        {
			ApplicationVersionChecker versionChecker = OnlineCurrentVersionChecker();
            
            ApplicationVersion version = await versionChecker.GetVersion("latestversion");
            Assert.Equal(OnlineCurrentVersion, version);

            Assert.True(versionChecker.IsUpToDate().Result);
        }

        [Fact]
        public async Task GetLatestVersion_ReturnsExpected()
        {
			ApplicationVersionChecker versionChecker = OnlineCurrentVersionChecker();

            ApplicationVersion latestVersion = await versionChecker.GetLatestVersion();
            Assert.Equal(OnlineCurrentVersion, latestVersion);
        }

		[Fact]
		public async Task GetVersion_NullVersionId_ThrowsArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>("versionId", () => OnlineCurrentVersionChecker().GetVersion(null));
		}

		[Fact]
		public async Task GetVersion_EmptyVersionId_ThrowsArgumentException()
		{
			await Assert.ThrowsAsync<ArgumentException>("versionId", () => OnlineCurrentVersionChecker().GetVersion(""));
        }

        [Theory]
        [InlineData("1.1.0.0", true)]
        [InlineData("1.0.0.0", false)]
        public async Task IsUpToDate_ReturnsExpected(string input, bool expected)
        {
            var currentVersion = new ApplicationVersion(input);
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            var upToDate = await versionChecker.IsUpToDate();
            Assert.Equal(upToDate, expected);
        }

        [Fact]
        public async Task GetLatestVersion_NoSuchVersion_ThrowsHttpRequestException()
        {
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, OnlineCurrentVersion);
            versionChecker.LatestVersionName = "invalid-data-torlalsdalladla";

            await Assert.ThrowsAsync<HttpRequestException>(async () => await versionChecker.GetLatestVersion());
        }
    }
}
