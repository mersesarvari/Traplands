using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WPFGameTest.Logic;

namespace WPFGameTest.Renderer
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
            drawingContext.DrawText(new FormattedText("Player01", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 12, Brushes.Red),
                new Point(model.Player.Transform.Position.X + model.Player.Transform.Size.X / 2 - 20, model.Player.Transform.Position.Y - 15));
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
                    item.Fill,
                    null,
                    new Rect(item.Rect.X, item.Rect.Y, item.Rect.Width, item.Rect.Height));
            }

            foreach (var item in model.Lines)
            {
                drawingContext.DrawLine(new Pen(item.Stroke, item.StrokeThickness), new Point(item.X1, item.Y1), new Point(item.X2, item.Y2));
            }
        }
    }
}
