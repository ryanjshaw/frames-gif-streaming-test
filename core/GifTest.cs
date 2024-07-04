using System.Drawing;
using KGySoft.Drawing.Imaging;
using KGySoft.Drawing.SkiaSharp;
using SkiaSharp;

namespace core;

public class GifTest
{
    private const int Width = 583;
    private const int Height = 583;
    
    public async Task Render (Stream asyncStream, CancellationToken cancellationToken)
    {
        using var syncStream = new SyncToAsyncStream(asyncStream);
        using var gifEncoder = new GifEncoder(syncStream, new Size(Width, Height));

        var counter = 0.0f;
        var font = new SKFont(SKTypeface.Default, 32);
        var paint = new SKPaint() { Color = SKColors.White };
        while (!cancellationToken.IsCancellationRequested)
        {
            using var skBitmap = new SKBitmap(Width, Height);
            using var skCanvas = new SKCanvas(skBitmap);
            skCanvas.Clear(SKColors.Black); // Fill the canvas with black
            skCanvas.DrawText(counter.ToString(), 50, 50, font, paint);
            using var bitmap = skBitmap.GetReadableBitmapData();
            using var bitmap8bpp = bitmap.Clone(KnownPixelFormat.Format8bppIndexed, OptimizedPaletteQuantizer.Wu());
            gifEncoder.AddImage(bitmap8bpp);

            counter += 0.10f;

            await Task.Delay(100);
        }
        
        gifEncoder.FinalizeEncoding();
    }
}