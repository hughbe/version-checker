using System;
using System.Threading.Tasks;
using Xunit;

namespace VersionChecker.Tests
{
    public class VersionCheckerTests
    {
        public static string VersionsLocation { get; } = "https://raw.githubusercontent.com/hughbe/version-checker/master/resources/versions";

        [Fact]
        public void VersionChecker_Constructor_Test_1()
        {
            var currentVersion = new ApplicationVersion("1.1.0.1");

            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            Assert.Equal(versionChecker.VersionsLocation, VersionsLocation);
            Assert.Equal(versionChecker.CurrentVersion, currentVersion);
        }

        [Fact]
        public void VersionChecker_Constructor_Test_2()
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

            Assert.Equal(versionChecker.CurrentVersionName, "hello");

            Assert.Throws<ArgumentNullException>(() => versionChecker.CurrentVersionName = null);
            Assert.Throws<ArgumentException>(() => versionChecker.CurrentVersionName = "");

            versionChecker.ResetCurrentVersionName();
            Assert.Equal(versionChecker.CurrentVersionName, "currentversion");
        }

        [Fact]
        public async void VersionChecker_Get_Version_Test_1()
        {
            var currentVersion = new ApplicationVersion("1.1.0.0");
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);


            var version = await versionChecker.GetVersion("currentversion");
            Assert.Equal(version.Id, "1.1.0.0");
        }

        [Fact]
        public void VersionChecker_Get_Version_Test_2()
        {
            var currentVersion = new ApplicationVersion("1.1.0.1");
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            Assert.ThrowsAsync<ArgumentNullException>(() => versionChecker.GetVersion(null));
            Assert.ThrowsAsync<ArgumentException>(() => versionChecker.GetVersion(""));
        }

        [Fact]
        public async Task VersionChecker_Up_To_Date_Test_1()
        {
            var currentVersion = new ApplicationVersion("1.1.0.0");
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            var upToDate = await versionChecker.IsUpToDate();
            Assert.True(upToDate);
        }

        [Fact]
        public async Task VersionChecker_Up_To_Date_Test_2()
        {
            var currentVersion = new ApplicationVersion("1.0.0.0");
            var versionChecker = new ApplicationVersionChecker(VersionsLocation, currentVersion);

            var upToDate = await versionChecker.IsUpToDate();
            Assert.False(upToDate);
        }
    }
}
