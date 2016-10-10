using System;
using Xunit;

namespace VersionChecker.Tests
{
	public class VersionUrlTests
	{
		[Fact]
		public void Ctor()
		{
			var url = new VersionUrl("Title", "Url");

			Assert.Equal(url.Title, "Title");
			Assert.Equal(url.Url, "Url");
		}

		[Fact]
		public void Ctor_NullTitle_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>("title", () => new VersionUrl(null, "Url"));
		}

		[Fact]
		public void Ctor_NullUrl_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>("url", () => new VersionUrl("Title", null));
		}

		[Fact]
		public void Ctor_EmptyTitle_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>("title", () => new VersionUrl(string.Empty, "Url"));
		}

		[Fact]
		public void Ctor_EmptyUrl_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>("url", () => new VersionUrl("Title", string.Empty));
		}

		[Fact]
		public void Equals_ReturnsExpected()
		{
			var url1 = new VersionUrl("Title", "Url");
			var url2 = new VersionUrl("Title", "Url");

			var url3 = new VersionUrl("ATitle", "Url");

			var url4 = new VersionUrl("Title", "AUrl");

			Assert.True(url1.Equals(url2));
			Assert.True(url1.GetHashCode().Equals(url2.GetHashCode()));

			Assert.True(url2.Equals(url1));
			Assert.True(url2.GetHashCode().Equals(url1.GetHashCode()));

			Assert.False(url1.Equals(url3));
			Assert.False(url1.Equals(url4));
			Assert.False(url1.Equals(null));
		}
	}
}
