using WPFGameTest.Logic;

namespace WPFGameTest.Renderer
{
    public interface IRenderer
    {
        IGameModel Model { get; set; }

        void SetupModel(IGameModel model);
    }
}