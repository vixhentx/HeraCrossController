using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Widgets.Drawables
{
    internal class LinearDrawable : IDrawable
    {
        public Linear.DirectionEnum Direction { get; set; }
        //可正可负
        public PointF Touch { get; set; }
        public Color BoarderColor { get; set; } = Colors.Black;
        public Color TrackColor { get; set; } = Colors.Cyan;
        public Color CircleColor { get; set; } = Colors.Blue;
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            (float r,PointF tCenter) = Direction switch
            {
                Linear.DirectionEnum.Horizonal => (
                    dirtyRect.Height / 2,
                    new PointF()
                    {
                        X = float.Clamp(dirtyRect.Center.X + Touch.X,0,dirtyRect.Width),
                        Y = dirtyRect.Center.Y
                    }
                ),
                Linear.DirectionEnum.Vertical => (
                    dirtyRect.Width / 2,
                    new PointF()
                    {
                        X = dirtyRect.Center.X,
                        Y = float.Clamp(dirtyRect.Center.Y + Touch.Y,0,dirtyRect.Height)
                    }
                ),
                _ => throw new NotImplementedException(),
            };
            canvas.StrokeColor = BoarderColor;
            canvas.FillColor = TrackColor;
            canvas.FillRoundedRectangle(dirtyRect, r);  //外框

            canvas.StrokeColor = CircleColor;
            canvas.FillColor = CircleColor;
            canvas.FillCircle(tCenter, r);              //键帽

        }
    }
}
