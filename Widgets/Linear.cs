namespace HeraCrossController.Widgets;

public class Linear : GraphicsView
{
	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value),typeof(int),typeof(Linear),0);
	public enum DirectionEnum
	{
		Horizonal,
		Vertical
	}
	public DirectionEnum Direction
	{
		get => _direction;
		set
		{
			_direction = value;
			_drawable.Direction = value;
		}
	}
	public int Value
	{
		get => (int)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}
	public int MaxValue { get; set; }
	public int MinValue { get; set; }
	private Drawables.LinearDrawable _drawable = new();
	private DirectionEnum _direction;
    private PointF Center => new()
	{
		X = (float)Width / 2,
		Y = (float)Height / 2
	};
	private PointF startPoint;
    public Linear()
	{
		Drawable = _drawable;
		//PanGestureRecognizer pr = new();
		//pr.PanUpdated += OnPanUpdated;
		//TapGestureRecognizer tr = new();
  //      tr.Tapped += OnTapped;
		//GestureRecognizers.Add(pr);
		//GestureRecognizers.Add(tr);
		StartInteraction += (sender, e) =>
		{
			startPoint = e.Touches.FirstOrDefault();
			ChangeValue(_drawable.Touch = (PointF)(startPoint - Center));
			Invalidate();
		};
		DragInteraction += (sender, e) =>
		{
			ChangeValue(_drawable.Touch = (PointF)(e.Touches.FirstOrDefault() - Center));
			Invalidate();
		};
		EndInteraction += (sender, e) =>
		{
			ChangeValue(_drawable.Touch = PointF.Zero);
			Invalidate();
		};
	}

 //   private void OnTapped(object? sender, TappedEventArgs e)
 //   {
	//	var p = e.GetPosition(this);
 //       startPoint.X = (float)p.Value.X;
	//	startPoint.Y = (float)p.Value.Y;
 //       ChangeValue(_drawable.Touch = (PointF)(startPoint - Center));
	//	Invalidate();
	//}

 //   private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
 //   {
 //       switch (e.StatusType)
 //       {
 //           case GestureStatus.Started:
 //           case GestureStatus.Running:
	//			PointF aPoint = new()
 //               {
 //                   X = (float)e.TotalX + startPoint.X,
 //                   Y = (float)e.TotalY + startPoint.X,
 //               };
 //               _drawable.Touch = (PointF)(aPoint - Center);
 //               ChangeValue(aPoint);
 //               Invalidate();
 //               break;

 //           case GestureStatus.Completed:
 //           case GestureStatus.Canceled:
 //               // 只在松手时归零
 //               _drawable.Touch = PointF.Zero;
 //               ChangeValue(Center);
 //               Invalidate();
 //               break;
 //       }
 //   }

    private void ChangeValue(PointF point)
	{
		float p = Direction switch
		{
			DirectionEnum.Horizonal => float.Clamp(point.X,0,(float)Width)/(float)Width,
			DirectionEnum.Vertical => float.Clamp(point.Y,0,(float)Height)/(float)Height,
			_ => throw new NotImplementedException()
		};
		int span = MaxValue - MinValue;
		Value = (int)(p * span) + MinValue;
	}

}