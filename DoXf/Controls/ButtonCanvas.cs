using System;
using System.Windows.Input;
using DoXf.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DoXf.Controls
{
    public enum DirectionType
    {
        Left = 0,
        Right = 1,
    }
    public enum StyleMode
    {
        Dark,
        Light
    }
    public class ButtonCanvas : SKCanvasView
    {
        public ButtonCanvas()
        {
        }

        #region ----- BindableProperty -----
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        public static readonly BindableProperty CommandParameterProperty =
                    BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonCanvas));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public static readonly BindableProperty CommandProperty =
                    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ButtonCanvas));

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
        public static readonly BindableProperty BorderColorProperty =
                    BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ButtonCanvas));

        public StyleMode StyleMode
        {
            get => (StyleMode)GetValue(StyleModeProperty);
            set => SetValue(StyleModeProperty, value);
        }
        public static readonly BindableProperty StyleModeProperty =
                    BindableProperty.Create(nameof(StyleMode), typeof(StyleMode), typeof(ButtonCanvas), StyleMode.Light, propertyChanged: StyleModePropertyChanged);

        private static void StyleModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ButtonCanvas)bindable).InvalidateSurface();
        }

        public Color BgInsideColor
        {
            get => (Color)GetValue(BgInsideColorProperty);
            set => SetValue(BgInsideColorProperty, value);
        }
        public static readonly BindableProperty BgInsideColorProperty =
                    BindableProperty.Create(nameof(BgInsideColor), typeof(Color), typeof(ButtonCanvas));

        public float BorderWidth
        {
            get => (float)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        public static readonly BindableProperty BorderWidthProperty =
                    BindableProperty.Create(nameof(BorderWidth), typeof(float), typeof(ButtonCanvas), 0f);

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty =
                    BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(ButtonCanvas), 0f);

        public DirectionType Direction
        {
            get => (DirectionType)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }
        public static readonly BindableProperty DirectionProperty =
                    BindableProperty.Create(nameof(Direction), typeof(DirectionType), typeof(ButtonCanvas), DirectionType.Left);
        #endregion

        protected override void OnTouch(SKTouchEventArgs e)
        {
            base.OnTouch(e);
            var bounds = new Rectangle(0, 0, Bounds.Width.ToPixels(), Bounds.Height.ToPixels());
            var boundsS = new Rectangle
            (
                Direction == DirectionType.Left ? 0 : Bounds.Width.ToPixels() - 20.ToPixels(),
                Bounds.Height.ToPixels() * .2,
                20.ToPixels(),
                Bounds.Height.ToPixels() * .3
            );
            if (!bounds.Contains(e.Location.ToFormsPoint()) || boundsS.Contains(e.Location.ToFormsPoint()))
            {
                Opacity = 1;
                return;
            }

            switch (e.ActionType)
            {
                case SKTouchAction.Released:
                    if (Command?.CanExecute(CommandParameter) ?? false)
                    {
                        Command.Execute(CommandParameter);
                        Opacity = 1;
                    }
                    break;
                case SKTouchAction.Pressed:
                    Opacity = .5;
                    break;
                case SKTouchAction.Entered:
                case SKTouchAction.Moved:
                case SKTouchAction.Cancelled:
                case SKTouchAction.Exited:
                case SKTouchAction.WheelChanged:
                    break;
            }
            // we have handled these events
            e.Handled = true;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            SKImageInfo info = e.Info;
            SKCanvas canvas = e.Surface.Canvas;

            #region ---- Draw background ----
            // Create the path
            SKPath path = new SKPath();
            SKPath pathBg = new SKPath();
            SKPath pathS = new SKPath();

            var strokeWidth = BorderWidth.ToPixels();
            var strokeMargin = strokeWidth / 2;

            float heightPart1 = (float)(info.Height * .2);
            float heightPart2 = (float)(info.Height * .3);
            float widthS = 20.ToPixels();
            float radius = CornerRadius.ToPixels();

            // 0-----------------------------------1
            // a--b----------------------------b'--a'
            // -------------------------------------
            // d--c----------------------------c'--d'
            // -------------------------------------
            // 3-----------------------------------2
            float[,] pointsCommon =
            {
                { strokeMargin, strokeMargin }, // 0
                { info.Width - strokeMargin, strokeMargin }, // 1
                { info.Width - strokeMargin, info.Height - strokeMargin }, // 2
                { strokeMargin, info.Height - strokeMargin }, // 3
            };
            float[,] pointsLeft =
            {
                { strokeMargin, heightPart1 }, // a
                { pointsCommon[0,0], pointsCommon[0,1] }, // 0
                { pointsCommon[1,0], pointsCommon[1,1] }, // 1
                { pointsCommon[2,0], pointsCommon[2,1] }, // 2
                { pointsCommon[3,0], pointsCommon[3,1] }, // 3
                { strokeMargin, heightPart1 + heightPart2 }, // d
                { widthS,  heightPart1}, // b
                { widthS, heightPart1 + heightPart2 } // c
            };
            float[,] pointsRight =
            {
                { info.Width - strokeMargin, heightPart1 }, // a'
                { pointsCommon[1,0], pointsCommon[1,1] }, // 1
                { pointsCommon[0,0], pointsCommon[0,1] }, // 0
                { pointsCommon[3,0], pointsCommon[3,1] }, // 3
                { pointsCommon[2,0], pointsCommon[2,1] }, // 2
                { info.Width - strokeMargin, heightPart1 + heightPart2 }, // d'
                { info.Width - widthS,  heightPart1}, // b'
                { info.Width - widthS, heightPart1 + heightPart2 } // c'
            };
            float[][,] points = { pointsLeft, pointsRight };

            int type = (int)Direction;
            // path border
            path.MoveTo(points[type][0, 0], points[type][0, 1] + strokeMargin);
            path.ArcTo(points[type][1, 0], points[type][1, 1], points[type][2, 0], points[type][2, 1], radius);
            path.ArcTo(points[type][2, 0], points[type][2, 1], points[type][3, 0], points[type][3, 1], radius);
            path.ArcTo(points[type][3, 0], points[type][3, 1], points[type][4, 0], points[type][4, 1], radius);
            path.ArcTo(points[type][4, 0], points[type][4, 1], points[type][5, 0], points[type][5, 1], radius);
            path.LineTo(points[type][5, 0], points[type][5, 1] - strokeMargin);

            // path background
            pathBg.MoveTo(points[type][0, 0], points[type][0, 1]);
            pathBg.ArcTo(points[type][1, 0], points[type][1, 1], points[type][2, 0], points[type][2, 1], radius);
            pathBg.ArcTo(points[type][2, 0], points[type][2, 1], points[type][3, 0], points[type][3, 1], radius);
            pathBg.ArcTo(points[type][3, 0], points[type][3, 1], points[type][4, 0], points[type][4, 1], radius);
            pathBg.ArcTo(points[type][4, 0], points[type][4, 1], points[type][5, 0], points[type][5, 1], radius);
            pathBg.LineTo(points[type][5, 0], points[type][5, 1]);

            pathBg.ArcTo(points[type][7, 0], points[type][7, 1], points[type][6, 0], points[type][6, 1], radius);
            pathBg.ArcTo(points[type][6, 0], points[type][6, 1], points[type][0, 0], points[type][0, 1], radius);
            pathBg.LineTo(points[type][0, 0], points[type][0, 1]);
            pathBg.Close();

            // draw S
            pathS.MoveTo(points[type][0, 0] - (type == 0 ? -strokeMargin : strokeMargin), points[type][0, 1]);
            pathS.ArcTo(points[type][6, 0], points[type][6, 1], points[type][7, 0], points[type][7, 1], radius);
            pathS.ArcTo(points[type][7, 0], points[type][7, 1], points[type][5, 0], points[type][5, 1], radius);
            pathS.LineTo(points[type][5, 0] - (type == 0 ? -strokeMargin : strokeMargin), points[type][5, 1]);

            SKPaint borderPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                //StrokeJoin = SKStrokeJoin.Round,
                Color = BorderColor.ToSKColor(),
                StrokeWidth = BorderWidth.ToPixels(),
                IsAntialias = true,
            };
            SKPaint borderSPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                //StrokeJoin = SKStrokeJoin.Round,
                Color = StyleMode == StyleMode.Light ? BorderColor.ToSKColor() : BgInsideColor.ToSKColor(),
                StrokeWidth = BorderWidth.ToPixels(),
                IsAntialias = true,
            };
            SKPaint fillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = BgInsideColor.ToSKColor(),
            };

            canvas.DrawPath(pathBg, fillPaint);
            canvas.DrawPath(path, borderPaint);
            canvas.DrawPath(pathS, borderSPaint);
            #endregion
        }
    }
}
