// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pxper
{
  public class ObjectUtils
  {
    public static object Clone(object obj)
    {
      MemoryStream ms = new MemoryStream();
      BinaryFormatter bf = new BinaryFormatter();

      try
      {
        bf.Serialize(ms, obj);
        ms.Seek(0, SeekOrigin.Begin);
        obj = bf.Deserialize(ms);
      }

      catch (Exception ex)
      {
        return null;
      }

      finally
      {
        ms.Close();
      }

      return obj;
    }

    public static byte[] ToArray(object obj)
    {
      MemoryStream ms = new MemoryStream();
      BinaryFormatter bf = new BinaryFormatter();

      try
      {
        bf.Serialize(ms, obj);
      }

      catch (Exception ex)
      {
        return null;
      }

      return ms.ToArray();
    }

    public static object FromArray(byte[] buffer)
    {
      MemoryStream ms = new MemoryStream();
      BinaryFormatter bf = new BinaryFormatter();
      object obj = null;

      try
      {
        foreach (byte b in buffer)
          ms.WriteByte(b);

        ms.Seek(0, SeekOrigin.Begin);
        obj = bf.Deserialize(ms);
      }

      catch (Exception ex)
      {
        return null;
      }

      return obj;
    }

    public static void Save(object obj, string file)
    {
      Exception error;

      ObjectUtils.Save(obj, file, out error);
    }

    public static void Save(object obj, string file, out Exception error)
    {
      FileStream fs = null;
      BinaryFormatter bf = new BinaryFormatter();

      error = null;

      try
      {
        fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None);
        bf.Serialize(fs, obj);
        fs.Flush();
      }

      catch (Exception ex)
      {
        error = ex;
      }

      finally
      {
        if (fs != null)
          fs.Close();
      }
    }

    public static object Load(string file)
    {
      Exception error;

      return ObjectUtils.Load(file, out error);
    }

    public static object Load(string file, out Exception error)
    {
      FileStream fs = null;
      BinaryFormatter bf = new BinaryFormatter();
      object obj = null;

      error = null;

      try
      {
        fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        obj = bf.Deserialize(fs);
      }

      catch (Exception ex)
      {
        error = ex;
      }

      finally
      {
        if (fs != null)
          fs.Close();
      }

      return error == null ? obj : null;
    }
  }
}