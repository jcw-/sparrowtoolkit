namespace Sparrow.Chart
{
    #region Enums

    /// <summary>
    /// Set the Overlay mode for Axis and Series
    /// </summary>
    public enum OverlayMode
    {
        /// <summary>
        /// The axis first
        /// </summary>
        AxisFirst,
        /// <summary>
        /// The series first
        /// </summary>
        SeriesFirst
    }

    /// <summary>
    /// Set Theme for SparrowChart
    /// </summary>
    public enum Theme
    {
        /// <summary>
        /// The arctic
        /// </summary>
        Arctic,
        /// <summary>
        /// The autmn
        /// </summary>
        Autmn,
        /// <summary>
        /// The cold
        /// </summary>
        Cold,
        /// <summary>
        /// The flower
        /// </summary>
        Flower,
        /// <summary>
        /// The forest
        /// </summary>
        Forest,
        /// <summary>
        /// The grayscale
        /// </summary>
        Grayscale,
        /// <summary>
        /// The ground
        /// </summary>
        Ground,
        /// <summary>
        /// The lialac
        /// </summary>
        Lialac,
        /// <summary>
        /// The natural
        /// </summary>
        Natural,
        /// <summary>
        /// The pastel
        /// </summary>
        Pastel,
        /// <summary>
        /// The rainbow
        /// </summary>
        Rainbow,
        /// <summary>
        /// The spring
        /// </summary>
        Spring,
        /// <summary>
        /// The summer
        /// </summary>
        Summer,
        /// <summary>
        /// The warm
        /// </summary>
        Warm,
        /// <summary>
        /// The metro
        /// </summary>
        Metro,
        /// <summary>
        /// The custom
        /// </summary>
        Custom
    }

    /// <summary>
    /// Set the axis tick position
    /// </summary>
    public enum TickPosition
    {
        /// <summary>
        /// The inside
        /// </summary>
        Inside,
        /// <summary>
        /// The cross
        /// </summary>
        Cross,
        /// <summary>
        /// The outside
        /// </summary>
        Outside
    }

    /// <summary>
    /// Set the XAxis Position for Sparrow Chart
    /// </summary>
    public enum XAxisPosition
    {
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom,
        /// <summary>
        /// The top
        /// </summary>
        Top
    }

    /// <summary>
    /// Set the YAxis Position for Sparrow Chart
    /// </summary>
    public enum YAxisPosition
    {
        /// <summary>
        /// The left
        /// </summary>
        Left,
        /// <summary>
        /// The right
        /// </summary>
        Right
    }

    /// <summary>
    /// Set the Axis Position for Sparrow Chart
    /// </summary>
    internal enum AxisPosition
    {
        /// <summary>
        /// The left
        /// </summary>
        Left,
        /// <summary>
        /// The top
        /// </summary>
        Top,
        /// <summary>
        /// The right
        /// </summary>
        Right,
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Set the Legend position for Sparrow Chart
    /// </summary>
    public enum LegendPosition
    {
        /// <summary>
        /// The inside
        /// </summary>
        Inside,
        /// <summary>
        /// The outside
        /// </summary>
        Outside
    }

    /// <summary>
    /// Set ValueType of XAxis
    /// </summary>
    public enum XType
    {
        /// <summary>
        /// The double
        /// </summary>
        Double, // For use Double type Axis
        /// <summary>
        /// The date time
        /// </summary>
        DateTime, // For use DateTime type Axis
        /// <summary>
        /// The category
        /// </summary>
        Category // For use Category type axis

    }

    internal enum ActualType
    {
        /// <summary>
        /// The double
        /// </summary>
        Double, // For use Double type Axis
        /// <summary>
        /// The date time
        /// </summary>
        DateTime, // For use DateTime type Axis
        /// <summary>
        /// The category
        /// </summary>
        Category // For use Category type axis
    }

    /// <summary>
    /// Set ValueType of YAxis
    /// </summary>
    public enum YType
    {
        /// <summary>
        /// The double
        /// </summary>
        Double, // For use Double type Axis
        /// <summary>
        /// The date time
        /// </summary>
        DateTime, // For use DateTime type Axis
    }

    /// <summary>
    /// Set SmoothingMode of GDI and BitmapGraphics
    /// </summary>
    public enum SmoothingMode
    {
        /// <summary>
        /// The anti alias
        /// </summary>
        AntiAlias,
        /// <summary>
        /// The default
        /// </summary>
        Default,
        /// <summary>
        /// The high quality
        /// </summary>
        HighQuality,
        /// <summary>
        /// The high speed
        /// </summary>
        HighSpeed,
        /// <summary>
        /// The invalid
        /// </summary>
        Invalid,
        /// <summary>
        /// The none
        /// </summary>
        None
    }

    /// <summary>
    /// Set CompositingQuality of GDI and BitmapGraphics
    /// </summary>
    public enum CompositingQuality
    {
        /// <summary>
        /// The assume linear
        /// </summary>
        AssumeLinear,
        /// <summary>
        /// The default
        /// </summary>
        Default,
        /// <summary>
        /// The gamma corrected
        /// </summary>
        GammaCorrected,
        /// <summary>
        /// The high quality
        /// </summary>
        HighQuality,
        /// <summary>
        /// The high speed
        /// </summary>
        HighSpeed,
        /// <summary>
        /// The invalid
        /// </summary>
        Invalid
    }

    /// <summary>
    /// Set RenderingMode of SparrowChart(GDIRendering-Good Performance,DefaultRendering-Medium Performance,Default-Good Quality of Rendering)
    /// </summary>
    public enum RenderingMode
    {
        /// <summary>
        /// The default
        /// </summary>
        Default,
#if DIRECTX2D
        DirectX2D,
#endif
#if WPF
        GDIRendering,  
        WritableBitmap
#endif
    }

    /// <summary>
    /// Set CompositingMode of GDI and BitmapGraphics
    /// </summary>
    public enum CompositingMode
    {
        /// <summary>
        /// The source copy
        /// </summary>
        SourceCopy,
        /// <summary>
        /// The source over
        /// </summary>
        SourceOver
    }
   


#if !WPF

    // Summary:
    //     Specifies values that control the behavior of a control positioned inside
    //     another control.
    public enum Dock
    {
        // Summary:
        //     Specifies that the control should be positioned on the left of the control.
        /// <summary>
        /// The left
        /// </summary>
        Left = 0,
        //
        // Summary:
        //     Specifies that the control should be positioned on top of the control.
        /// <summary>
        /// The top
        /// </summary>
        Top = 1,
        //
        // Summary:
        //     Specifies that the control should be positioned on the right of the control.
        /// <summary>
        /// The right
        /// </summary>
        Right = 2,
        //
        // Summary:
        //     Specifies that the control should be positioned at the bottom of the control.
        /// <summary>
        /// The bottom
        /// </summary>
        Bottom = 3,
    }
#endif

#endregion
}
