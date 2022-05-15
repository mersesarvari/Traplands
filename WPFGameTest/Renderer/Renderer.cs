using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Game.Logic;
using WPFGameTest.Logic;

namespace Game.Renderer
{
    public abstract class RendererBase : FrameworkElement, IRenderer
    {
        public IGameModel Model { get; set; }

        public void SetupModel(IGameModel model)
        {
            Model = model;
        }
    }

    public class SinglePlayerRenderer : RendererBase
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            ISingleplayer model = Model as ISingleplayer;

            foreach (var item in model.Solids)
            {
                drawingContext.DrawRectangle(
                    item.Fill,
                    null,
                    new Rect(item.Transform.Position.X, item.Transform.Position.Y, item.Transform.Size.X, item.Transform.Size.Y));
            }

            foreach (var item in model.Interactables)
            {
                drawingContext.DrawRectangle(
                    item.Fill,
                    null,
                    new Rect(item.Transform.Position.X, item.Transform.Position.Y, item.Transform.Size.X, item.Transform.Size.Y));
            }

            drawingContext.PushTransform(model.Player.Transform.ScaleTransform);
            drawingContext.DrawRectangle(
                model.Player.Fill,
                null,
                new Rect(model.Player.Transform.Position.X, model.Player.Transform.Position.Y, model.Player.Transform.Size.X, model.Player.Transform.Size.Y));
            drawingContext.Pop();

            // Testing text rendering for online games
            //drawingContext.DrawText(new FormattedText("Player01", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 12, Brushes.Red),
            //    new Point(model.Player.Transform.Position.X + model.Player.Transform.Size.X / 2 - 20, model.Player.Transform.Position.Y - 15));
        }
    }

    public class LevelEditorRenderer : RendererBase
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            ILevelEditor model = Model as ILevelEditor;

            foreach (var item in model.Rectangles)
            {
                drawingContext.DrawRectangle(
                    item.Rectangle.Fill,
                    null,
                    new Rect(item.Position.X, item.Position.Y, item.Rectangle.Width, item.Rectangle.Height));

                if (item is WaypointRect)
                {
                    drawingContext.DrawText(new FormattedText((item as WaypointRect).IdLabel.Content.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 24, Brushes.Black),
                        new Point(item.Position.X, item.Position.Y));
                }
            }

            foreach (var item in model.Lines)
            {
                drawingContext.DrawLine(new Pen(item.Stroke, item.StrokeThickness), new Point(item.X1, item.Y1), new Point(item.X2, item.Y2));
            }
        }
    }

    public class MultiplayerRenderer : RendererBase
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            IMultiplayer model = Model as IMultiplayer;

            foreach (var item in model.Solids)
            {
                drawingContext.DrawRectangle(
                    item.Fill,
                    null,
                    new Rect(item.Transform.Position.X, item.Transform.Position.Y, item.Transform.Size.X, item.Transform.Size.Y));
            }

            foreach (var item in model.Interactables)
            {
                drawingContext.DrawRectangle(
                    item.Fill,
                    null,
                    new Rect(item.Transform.Position.X, item.Transform.Position.Y, item.Transform.Size.X, item.Transform.Size.Y));
            }

            foreach (var item in model.Players)
            {
                var scale = new ScaleTransform();
                scale.ScaleX = item.RenderData.ScaleX;
                scale.CenterX = item.RenderData.CenterX;
                scale.CenterY = item.RenderData.CenterY;

                SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.Color));

                var fill = Resource.GetSpriteSheetImage(item.RenderData.FileName, item.RenderData.ImageIndex);

                drawingContext.PushTransform(scale);
                drawingContext.DrawRectangle(
                    fill,
                    null,
                    new Rect(item.RenderData.Position.X, item.RenderData.Position.Y, item.RenderData.Size.X, item.RenderData.Size.Y));
                drawingContext.Pop();

                drawingContext.DrawText(new FormattedText(item.Username, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 12, color),
                new Point(item.RenderData.Position.X + item.RenderData.Size.X / 2 - 20, item.RenderData.Position.Y - 15));
            }

            SolidColorBrush playerColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.Player.PlayerColor));
            //Rectangle rect = new Rectangle();
            //rect.Width = model.Player.Transform.Size.X;
            //rect.Height = model.Player.Transform.Size.Y;
            //rect.Fill = new SolidColorBrush(Colors.Red);
            //rect.Opacity = 0.2;
            //rect.OpacityMask = model.Player.Fill;

            drawingContext.PushTransform(model.Player.Transform.ScaleTransform);
            drawingContext.DrawRectangle(
                model.Player.Fill,
                null,
                new Rect(model.Player.Transform.Position.X, model.Player.Transform.Position.Y, model.Player.Transform.Size.X, model.Player.Transform.Size.Y));
            drawingContext.Pop();

            drawingContext.DrawText(new FormattedText(model.Player.PlayerName, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 12, playerColor),
                new Point(model.Player.Transform.Position.X + model.Player.Transform.Size.X / 2 - 20, model.Player.Transform.Position.Y - 15));
        }
    }
}
