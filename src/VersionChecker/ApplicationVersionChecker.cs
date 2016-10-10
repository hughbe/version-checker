using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace VersionChecker
{
    public class ApplicationVersionChecker
    {
        public ApplicationVersionChecker(string versionsLocation, ApplicationVersion currentVersion)
        {
            if (versionsLocation == null) { throw new ArgumentNullException(nameof(versionsLocation)); }
            if (versionsLocation.Length == 0) { throw new ArgumentException("The version location is empty", nameof(versionsLocation)); }

            if (currentVersion == null) { throw new ArgumentNullException(nameof(currentVersion)); }
            
            VersionsLocation = versionsLocation;
            CurrentVersion = currentVersion;
        }

        public ApplicationVersion CurrentVersion { get; }
        public ApplicationVersion LatestVersion { get; private set; }

        public string VersionsLocation { get; }
        
        private string _latestVersionName = "latestversion";
        public string LatestVersionName
        {
            get { return _latestVersionName; }
            set
            {
                Utilities.CheckStringParam(value, SR.LatestVersionNameEmpty, nameof(value));

                _latestVersionName = value;
            }
        }

        public void ResetLatestVersionName()
        {
            LatestVersionName = "latestversion";
        }
        
        public async Task<ApplicationVersion> GetLatestVersion()
        {
            return await GetVersion(LatestVersionName);
        }
        
        public async Task<ApplicationVersion> GetVersion(string versionId)
        {
            if (versionId == null) { throw new ArgumentNullException(nameof(versionId)); }
            if (versionId.Length == 0) { throw new ArgumentException("Version Id cannot be empty", nameof(versionId)); }

            var path = Path.Combine(VersionsLocation, versionId + ".xml");

            var xml = await new HttpClient().GetStringAsync(path);
            return ApplicationVersion.FromXml(xml);
        }

        public async Task<bool> IsUpToDate()
        {
            LatestVersion = await GetLatestVersion();
            return LatestVersion.Equals(CurrentVersion);
        }
    }
}
