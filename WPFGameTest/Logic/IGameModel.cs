using System.Collections.Generic;
using System.Windows.Shapes;
using Game.Models;
using Game.Renderer;

namespace Game.Logic
{
    public interface IGameModel
    {
        void ProcessInput();
        void Update(float deltaTime);
    }

    public interface ISingleplayer : IGameModel
    {
        Player Player { get; set; }
        List<GameObject> Solids { get; set; }
        List<GameObject> Interactables { get; set; }
    }

    public interface IMultiplayer : IGameModel
    {
        List<Player> Players { get; set; }
        List<GameObject> Solids { get; set; }
        List<GameObject> Interactables { get; set; }
    }

    public interface ILevelEditor : IGameModel
    {
        LevelGrid Grid { get; set; }
        List<EditorRect> Rectangles { get; set; }
        List<Line> Lines { get; set; }
        EditorRect PreviewRect { get; set; }
        double Zoom { get; set; }
    }
}