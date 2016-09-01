// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pxper
{
  public static class BitmapExtensions
  {
    public static Bitmap SetOpacity(this Bitmap image, float opacity)
    {
      try
      {
        Bitmap bitmap = new Bitmap(image.Width, image.Height);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
          ColorMatrix colorMatrix = new ColorMatrix();

          colorMatrix.Matrix33 = opacity;

          ImageAttributes imageAttributes = new ImageAttributes();

          imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
          g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
        }

        return bitmap;
      }

      catch (Exception ex)
      {
        return null;
      }
    }

    public static Bitmap SetRed(this Bitmap image)
    {
      try
      {
        Bitmap bitmap = new Bitmap(image.Width, image.Height);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
          float[][] colorMatrixElements = { 
            new float[] {1, 0, 0, 0, 0},
            new float[] {0, 0.5f, 0, 0, 0},
            new float[] {0, 0, 0.5f, 0, 0},
            new float[] {0, 0, 0, 0.5f, 0},
            new float[] {0, 0, 0, 0, 1}
          };

          ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
          ImageAttributes imageAttributes = new ImageAttributes();

          imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
          g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
        }

        return bitmap;
      }

      catch (Exception ex)
      {
        return null;
      }
    }
  }
}