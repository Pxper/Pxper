// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pxper
{
  public class PsdParser
  {
    private short channels;
    private int height;
    private int width;
    private short bitsPerPixel;
    private short colourMode;

    public PsdParser()
    {
      this.channels = 0;
      this.height = 0;
      this.width = 0;
      this.bitsPerPixel = 0;
      this.colourMode = 0;
    }

    public List<OverlayBase> GetGuides(string pathAndFilename)
    {
      FileStream stream = new FileStream(pathAndFilename, FileMode.Open, FileAccess.Read, FileShare.Read);

      if (!stream.SafeFileHandle.Equals(0))
      {
        try
        {
          this.ReadHeader(stream);
        }

        catch (Exception)
        {
          return null;
        }

        try
        {
          this.SkipColorMode(stream);
        }

        catch (Exception)
        {
          return null;
        }

        try
        {
          return this.ReadImageResource(stream);
        }

        catch (Exception)
        {
          return null;
        }
      }

      return null;
    }

    protected bool ReadHeader(FileStream stream)
    {
      BinaryReader binaryReader = new BinaryReader(stream);

      try
      {
        binaryReader.BaseStream.Position = 0;

        byte[] signature = binaryReader.ReadBytes(4);
        byte[] version = binaryReader.ReadBytes(2);
        byte[] reserved = binaryReader.ReadBytes(6);
        byte[] channels = binaryReader.ReadBytes(2);
        byte[] rows = binaryReader.ReadBytes(4);
        byte[] columns = binaryReader.ReadBytes(4);
        byte[] depth = binaryReader.ReadBytes(2);
        byte[] mode = binaryReader.ReadBytes(2);

        if (Encoding.ASCII.GetString(signature).Equals("8BPS") && version[1] == 0x01)
        {
          if (this.SwapBytes(channels, 2))
            this.channels = BitConverter.ToInt16(channels, 0);

          if (this.SwapBytes(rows, 4))
            this.height = BitConverter.ToInt32(rows, 0);

          if (this.SwapBytes(columns, 4))
            this.width = BitConverter.ToInt32(columns, 0);

          if (this.SwapBytes(depth, 2))
            this.bitsPerPixel = BitConverter.ToInt16(depth, 0);

          if (this.SwapBytes(mode, 2))
            this.colourMode = BitConverter.ToInt16(mode, 0);

          if (this.channels != -1 && this.height != -1 && this.width != -1 && this.bitsPerPixel != -1 && this.colourMode != -1)
            return true;
        }

        return false;
      }

      catch (Exception ex)
      {
        return false;
      }
    }

    protected bool SkipColorMode(FileStream stream)
    {
      BinaryReader binaryReader = new BinaryReader(stream);

      try
      {
        binaryReader.BaseStream.Position = 26;

        int length = 0;
        byte[] bytes = binaryReader.ReadBytes(4);

        if (this.SwapBytes(bytes, 4))
          length = BitConverter.ToInt32(bytes, 0);

        if (length > 0)
          binaryReader.ReadBytes(length);

        return true;
      }

      catch (Exception ex)
      {
        return false;
      }
    }

    protected List<OverlayBase> ReadImageResource(FileStream stream)
    {
      List<OverlayBase> guides = new List<OverlayBase>();
      BinaryReader binaryReader = new BinaryReader(stream);

      try
      {
        int length = 0;
        byte[] bytes = binaryReader.ReadBytes(4);

        if (this.SwapBytes(bytes, 4))
          length = BitConverter.ToInt32(bytes, 0);

        long streamLength = stream.Length;
        int bytesTotal = length;
        int bytesRed = 0;

        while (stream.Position < streamLength && bytesRed < bytesTotal)
        {
          byte[] osType = binaryReader.ReadBytes(4);

          bytesRed += 4;

          if (Encoding.ASCII.GetString(osType).Equals("8BIM"))
          {
            int imageResourceId = -1;

            bytes = binaryReader.ReadBytes(2);
            bytesRed += 2;

            if (this.SwapBytes(bytes, 2))
              imageResourceId = BitConverter.ToInt16(bytes, 0);

            byte @byte = binaryReader.ReadByte();

            bytesRed += 1;

            int nameSize = (int)@byte;

            if (nameSize > 0)
            {
              if (nameSize % 2 != 0)
              {
                @byte = binaryReader.ReadByte();
                bytesRed += 1;
              }

              bytesRed += nameSize;
            }

            binaryReader.ReadByte();
            bytesRed += 1;

            int size = 0;

            bytes = binaryReader.ReadBytes(4);

            if (this.SwapBytes(bytes, 4))
              size = BitConverter.ToInt32(bytes, 0);

            bytesRed += 4;

            if ((size % 2) != 0)
              size++;

            if (size > 0)
            {
              if (imageResourceId == 1032)
              {
                binaryReader.ReadBytes(4);
                bytesRed += 4;

                binaryReader.ReadBytes(8);
                bytesRed += 8;

                int count = 0;

                bytes = binaryReader.ReadBytes(4);

                if (this.SwapBytes(bytes, 4))
                  count = BitConverter.ToInt32(bytes, 0);

                bytesRed += 4;

                if (count > 0)
                {
                  for (int i = 0; i != count; i++)
                  {
                    int position = -1;

                    bytes = binaryReader.ReadBytes(4);

                    if (this.SwapBytes(bytes, 4))
                      position = BitConverter.ToInt32(bytes, 0);

                    bytesRed += 4;

                    byte direction = binaryReader.ReadByte();

                    bytesRed += 1;

                    Guide guide = new Guide();

                    guide.Position = position / 32;
                    guide.Orienation = direction == 0 ? Orienation.Vertical : Orienation.Horizontal;
                    guides.Add(guide);
                  }
                }
              }

              else
              {
                for (int n = 0; n < size; ++n)
                {
                  byte c = binaryReader.ReadByte();
                  bytesRed += 1;
                }
              }
            }
          }
        }

        return guides;
      }

      catch (Exception ex)
      {
        return null;
      }
    }

    private bool SwapBytes(byte[] bytes, int length)
    {
      try
      {
        for (long i = 0; i < length / 2; i++)
        {
          byte b = bytes[i];

          bytes[i] = bytes[length - i - 1];
          bytes[length - i - 1] = b;
        }

        return true;
      }

      catch (Exception ex)
      {
        return false;
      }
    }
  }
}