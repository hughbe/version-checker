using System;

namespace VersionChecker
{
    internal static class SR
    {
        public static string VersionUrlEmpty => "The version url is empty";
        public static string VersionUrlTitleEmpty => "The version url title is empty";

        public static string VersionNoteTitleEmpty => "The version note title is empty";
        public static string VersionNoteContentEmpty => "The version note content is empty";

        public static string LatestVersionNameEmpty => "The latest version name cannot be empty";
    }

    internal static class Utilities
    {
		public static void CheckParameter(object parameter, string parameterName)
        {
			if (parameter == null) { throw new ArgumentNullException(parameterName); }
        }

        public static void CheckStringParam(string parameter, string description, string parameterName)
        {
            CheckParameter(parameter, parameterName);
            if (parameter.Length == 0) { throw new ArgumentException(description, parameterName); }
        }
    }
}
