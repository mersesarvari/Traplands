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

    public interface IGameplayBase : IGameModel
    {
        float LevelTimer { get; }
        bool Paused { get; set; }
        bool GameOver { get; set; }
        bool Transitioning { get; set; }
        double TransitionAlpha { get; }
        Player Player { get; set; }
        List<GameObject> Solids { get; set; }
        List<GameObject> Interactables { get; set; }
    }

    public interface ISingleplayer : IGameplayBase
    {
        void SetLevel(string levelName);
        void SaveLevel();
    }

    public interface IMultiplayer : IGameplayBase
    {
        List<User> Players { get; set; }
    }

    public interface ILevelEditor : IGameModel
    {
        LevelGrid Grid { get; set; }
        List<EditorElement> Rectangles { get; set; }
        List<Line> Lines { get; set; }
        EditorElement PreviewRect { get; set; }
        CannonRect SelectedCannon { get; set; }
        void FlipCannon(CannonRect cannon);
        void Init(FrameworkElement renderTarget, ScrollViewer camera);
        void LoadElements(IList<EditorElement> elements);
        void SelectElement(EditorElement element);
        void SaveLevel(string levelName);
    }
}