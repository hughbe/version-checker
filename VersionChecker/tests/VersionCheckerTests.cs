using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace VersionChecker.Tests
{
    public class VersionCheckerTests
    {
        public static string VersionsLocation { get; } = "https://raw.githubusercontent.com/hughbe/version-checker/master/resources/versions";

        [Fact]
        public void VersionChecker_Constructor_Test()
        {
            var currentVersion = new ApplicationVersion("1.1.0.1");

            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            Assert.Equal(versionChecker.VersionsLocation, "https://raw.githubusercontent.com/hughbe/version-checker/master/resources/versions");
            Assert.Equal(versionChecker.CurrentVersion, currentVersion);
        }

        [Fact]
        public void VersionChecker_Invalid_Constructor_Test()
        {
            var currentVersion = new ApplicationVersion("1.1.0.1");

            Assert.Throws<ArgumentNullException>(() => new ApplicationVersionChecker(null, currentVersion));
            Assert.Throws<ArgumentException>(() => new ApplicationVersionChecker("", currentVersion));

            Assert.Throws<ArgumentNullException>(() => new ApplicationVersionChecker(VersionsLocation, null));
        }

        [Fact]
        public void VersionChecker_Current_Version_Name_Test()
        {
            var currentVersion = new ApplicationVersion("1.1.0.1");
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);
            versionChecker.CurrentVersionName = "hello";
            Assert.True(versionChecker.CurrentVersionName == "hello");

            Assert.Throws<ArgumentNullException>(() => versionChecker.CurrentVersionName = null);
            Assert.Throws<ArgumentException>(() => versionChecker.CurrentVersionName = "");

            versionChecker.ResetCurrentVersionName();
            Assert.Equal(versionChecker.CurrentVersionName, "currentversion");
        }

        [Fact]
        public async void VersionChecker_Get_Current_Version_Test()
        {
            var currentVersion = new ApplicationVersion("1.1.0.0");
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);


            var version = await versionChecker.GetVersion("currentversion");
            Assert.Equal(version.Id, "1.1.0.0");
        }

        [Fact]
        public void VersionChecker_Get_Invalid_Version_Test()
        {
            var currentVersion = new ApplicationVersion("1.1.0.1");
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            Assert.ThrowsAsync<ArgumentNullException>(() => versionChecker.GetVersion(null));
            Assert.ThrowsAsync<ArgumentException>(() => versionChecker.GetVersion(""));
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
            var currentVersion = new ApplicationVersion("1.1.0.0");
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);
            versionChecker.CurrentVersionName = "invalid-data-torlalsdalladla";

            await Assert.ThrowsAsync<HttpRequestException>(async () => await versionChecker.UpdateLatestVersion());
        }

        [Fact]
        public void Utilities_Check_Parameter_Test()
        {
            Assert.Throws<ArgumentNullException>(() => Utilities.CheckParameter(null, "paramName"));
            Assert.Throws<ArgumentNullException>(() => Utilities.CheckStringParam(null, "description", "paramName"));
            Assert.Throws<ArgumentException>(() => Utilities.CheckStringParam("", "description", "paramName"));

            Assert.Null(Record.Exception(() => Utilities.CheckParameter("aString", "paramName")));
            Assert.Null(Record.Exception(() => Utilities.CheckStringParam("aString", "description", "paramName")));
        }

    }
}
