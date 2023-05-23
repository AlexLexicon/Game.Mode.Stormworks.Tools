using Game.Mode.Stormworks.Tools.Swtpkg.Application.Models;

namespace Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Extensions;
public static class PixelFormatExtensions
{
    public static PixelFormat ToAbstractions(this System.Drawing.Imaging.PixelFormat pixelFormat)
    {
        return pixelFormat switch
        {
            System.Drawing.Imaging.PixelFormat.Alpha => PixelFormat.Alpha,
            System.Drawing.Imaging.PixelFormat.Canonical => PixelFormat.Canonical,
            System.Drawing.Imaging.PixelFormat.DontCare => PixelFormat.DontCare,
            System.Drawing.Imaging.PixelFormat.Extended => PixelFormat.Extended,
            System.Drawing.Imaging.PixelFormat.Format16bppArgb1555 => PixelFormat.Format16bppArgb1555,
            System.Drawing.Imaging.PixelFormat.Format16bppGrayScale => PixelFormat.Format16bppGrayScale,
            System.Drawing.Imaging.PixelFormat.Format16bppRgb555 => PixelFormat.Format16bppRgb555,
            System.Drawing.Imaging.PixelFormat.Format16bppRgb565 => PixelFormat.Format16bppRgb565,
            System.Drawing.Imaging.PixelFormat.Format1bppIndexed => PixelFormat.Format1bppIndexed,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb => PixelFormat.Format24bppRgb,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb => PixelFormat.Format32bppArgb,
            System.Drawing.Imaging.PixelFormat.Format32bppPArgb => PixelFormat.Format32bppPArgb,
            System.Drawing.Imaging.PixelFormat.Format32bppRgb => PixelFormat.Format32bppRgb,
            System.Drawing.Imaging.PixelFormat.Format48bppRgb => PixelFormat.Format48bppRgb,
            System.Drawing.Imaging.PixelFormat.Format4bppIndexed => PixelFormat.Format4bppIndexed,
            System.Drawing.Imaging.PixelFormat.Format64bppArgb => PixelFormat.Format64bppArgb,
            System.Drawing.Imaging.PixelFormat.Format64bppPArgb => PixelFormat.Format64bppPArgb,
            System.Drawing.Imaging.PixelFormat.Format8bppIndexed => PixelFormat.Format8bppIndexed,
            System.Drawing.Imaging.PixelFormat.Gdi => PixelFormat.Gdi,
            System.Drawing.Imaging.PixelFormat.Indexed => PixelFormat.Indexed,
            System.Drawing.Imaging.PixelFormat.Max => PixelFormat.Max,
            System.Drawing.Imaging.PixelFormat.PAlpha => PixelFormat.PAlpha,
            //this has the same underlying index as 'System.Drawing.Imaging.PixelFormat.DontCare'
            //System.Drawing.Imaging.PixelFormat.Undefined => PixelFormat.Undefined,
            _ => throw new NotImplementedException($"The '{typeof(System.Drawing.Imaging.PixelFormat).FullName}' of '{pixelFormat}' is not implemented by the '{typeof(PixelFormat).FullName}'.")
        };
    }

    public static System.Drawing.Imaging.PixelFormat ToDrawing(this PixelFormat pixelFormat)
    {
        return pixelFormat switch
        {
            PixelFormat.Alpha => System.Drawing.Imaging.PixelFormat.Alpha,
            PixelFormat.Canonical => System.Drawing.Imaging.PixelFormat.Canonical,
            PixelFormat.DontCare => System.Drawing.Imaging.PixelFormat.DontCare,
            PixelFormat.Extended => System.Drawing.Imaging.PixelFormat.Extended,
            PixelFormat.Format16bppArgb1555 => System.Drawing.Imaging.PixelFormat.Format16bppArgb1555,
            PixelFormat.Format16bppGrayScale => System.Drawing.Imaging.PixelFormat.Format16bppGrayScale,
            PixelFormat.Format16bppRgb555 => System.Drawing.Imaging.PixelFormat.Format16bppRgb555,
            PixelFormat.Format16bppRgb565 => System.Drawing.Imaging.PixelFormat.Format16bppRgb565,
            PixelFormat.Format1bppIndexed => System.Drawing.Imaging.PixelFormat.Format1bppIndexed,
            PixelFormat.Format24bppRgb => System.Drawing.Imaging.PixelFormat.Format24bppRgb,
            PixelFormat.Format32bppArgb => System.Drawing.Imaging.PixelFormat.Format32bppArgb,
            PixelFormat.Format32bppPArgb => System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
            PixelFormat.Format32bppRgb => System.Drawing.Imaging.PixelFormat.Format32bppRgb,
            PixelFormat.Format48bppRgb => System.Drawing.Imaging.PixelFormat.Format48bppRgb,
            PixelFormat.Format4bppIndexed => System.Drawing.Imaging.PixelFormat.Format4bppIndexed,
            PixelFormat.Format64bppArgb => System.Drawing.Imaging.PixelFormat.Format64bppArgb,
            PixelFormat.Format64bppPArgb => System.Drawing.Imaging.PixelFormat.Format64bppPArgb,
            PixelFormat.Format8bppIndexed => System.Drawing.Imaging.PixelFormat.Format8bppIndexed,
            PixelFormat.Gdi => System.Drawing.Imaging.PixelFormat.Gdi,
            PixelFormat.Indexed => System.Drawing.Imaging.PixelFormat.Indexed,
            PixelFormat.Max => System.Drawing.Imaging.PixelFormat.Max,
            PixelFormat.PAlpha => System.Drawing.Imaging.PixelFormat.PAlpha,                            
            //this has the same underlying index as 'System.Drawing.Imaging.PixelFormat.DontCare'
            //System.Drawing.Imaging.PixelFormat.Undefined => PixelFormat.Undefined,
            _ => throw new NotImplementedException($"The '{typeof(PixelFormat).FullName}' of '{pixelFormat}' is not implemented by the '{typeof(System.Drawing.Imaging.PixelFormat).FullName}'.")
        };
    }
}
