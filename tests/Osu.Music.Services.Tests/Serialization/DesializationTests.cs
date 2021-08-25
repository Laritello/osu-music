using NUnit.Framework;
using Osu.Music.Services.IO;
using Osu.Music.Services.Serialization;
using System.IO;

namespace Osu.Music.Services.Tests.Serialization
{
    [TestFixture]
    public class DesializationTests
    {
        [SetUp]
        public void SetUp()
        {
            // Set up logic
        }

        [Test]
        public void DeserializeTest()
        {
            var beatmap = BeatmapConverter.Deserialize("Resources\\YouSayRun.osu");
            Assert.AreEqual(beatmap, beatmap, "Not equalt");
        }
    }
}
