using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Game.Models;
using Game.Renderer;
using WPFGameTest.Logic;

namespace Game.Logic
{
    public interface IGameModel
    {
        void ProcessInput();
        void Update(float deltaTime);
    }

    public interface ISingleplayer : IGameModel
    {
        float LevelTimer { get; }
        bool Paused { get; set; }
        bool GameOver { get; set; }
        bool Transitioning { get; set; }
        double TransitionAlpha { get; }
        Player Player { get; set; }
        List<GameObject> Solids { get; set; }
        List<GameObject> Interactables { get; set; }
        void SetLevel(string levelName);
        void SaveLevel();
    }

    public interface IMultiplayer : IGameModel
    {
        Player Player { get; set; }
        List<User> Players { get; set; }
        List<GameObject> Solids { get; set; }
        List<GameObject> Interactables { get; set; }
    }

    public interface ILevelEditor : IGameModel
    {
        LevelGrid Grid { get; set; }
        List<EditorElement> Rectangles { get; set; }
        List<Line> Lines { get; set; }
        EditorElement PreviewRect { get; set; }
        void Init(FrameworkElement renderTarget, ScrollViewer camera);
        void LoadElements(IList<EditorElement> elements);
        void SelectElement(EditorElement element);
        void SaveLevel(string levelName);
    }
}