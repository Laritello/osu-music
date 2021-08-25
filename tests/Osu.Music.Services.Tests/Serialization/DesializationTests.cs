using NUnit.Framework;
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

        }

        [Test]
        public void DeserializeTest()
        {
            var beatmap = BeatmapConverter.Deserialize("Resources\\YouSayRun.osu");

            Assert.AreEqual(beatmap, beatmap, "Not equalt");
        }
    }
}
