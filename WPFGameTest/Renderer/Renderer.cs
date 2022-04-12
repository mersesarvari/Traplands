using System.Windows;
using System.Windows.Media;

namespace WPFGameTest.Renderer
{
    public class Renderer : FrameworkElement
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(new ImageBrush(Resource.GetImage("Player")), null, new Rect(100,100,100,100));
            
            //drawingContext.DrawRectangle(new SolidColorBrush(Colors.Red), null, new Rect(100,100,100,100));
        }
    }
}
