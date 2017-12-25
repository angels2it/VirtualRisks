using System.Threading.Tasks;
using CastleGo.Shared.Games;

namespace CastleGo.GameAi
{
    public interface IAttackingGameAiService
    {
        Task Battalion(GameStateModel game);
    }
}
