using System.Threading.Tasks;

namespace CastleGo.GameAi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameAiFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        Task AnayticForGame(string id);
    }
}
