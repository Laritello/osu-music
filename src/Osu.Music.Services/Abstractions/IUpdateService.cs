using System.Threading.Tasks;

namespace Osu.Music.Services.Abstractions
{
	public interface IUpdateService
	{
		public Task<bool> HasUpdate();
		public Task Update();
	}
}
