using System.Collections.Generic;
using System.Windows.Media;
using Game.Helpers;

namespace Game.Models
{
    public abstract class GameObject
    {
        public string Tag { get; set; }
        public bool NeedToRemove { get; set; }
        public Brush Fill { get; set; }
        public Transform Transform { get; set; }
        public IntRect Hitbox { get; set; }

        public GameObject(Vector2 position, Vector2 size)
        {
            Tag = "Default";

            Fill = new SolidColorBrush(Colors.Thistle);

            Transform = new Transform();
            Transform.Position = new Vector2(position.X, position.Y);
            Transform.Size = new Vector2(size.X, size.Y); ;

            Hitbox = new IntRect(position.X, position.Y, size.X, size.Y);
        }

        public static List<GameObject> solids = new List<GameObject>();
        public static List<GameObject> interactables = new List<GameObject>();
        public static List<GameObject> players = new List<GameObject>();

        public static void SetSolids(List<GameObject> solidBodies)
        {
            solids = solidBodies;
        }

        public static void SetInteractables(List<GameObject> interactableBodies)
        {
            interactables = interactableBodies;
        }

        public static void SetPlayers(List<GameObject> playerList)
        {
            players = playerList;
        }

        public virtual void Start()
        {

        }

        public virtual void Update(float deltaTime)
        {
            Transform.ScaleTransform.CenterX = Transform.Position.X + Transform.Size.X * 0.5f;
            Transform.ScaleTransform.CenterY = Transform.Position.Y + Transform.Size.Y * 0.5f;
        }

        public void SetDefaultSprite(ImageSource imgSrc)
        {
            Fill = new ImageBrush(imgSrc);
        }
    }
}