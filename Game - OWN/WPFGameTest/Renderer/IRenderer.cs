using Game.Logic;

namespace Game.Renderer
{
    public interface IRenderer
    {
        IGameModel Model { get; set; }

        void SetupModel(IGameModel model);
    }
}