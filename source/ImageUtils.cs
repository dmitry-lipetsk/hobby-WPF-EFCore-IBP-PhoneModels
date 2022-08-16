////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace MVVM{
////////////////////////////////////////////////////////////////////////////////
//class ImageUtils

static class ImageUtils
{
 public static ImageSource? BinaryToImageSource(byte[] binaryData)
 {
  if(Object.ReferenceEquals(binaryData,null))
   return null;

  var imageSource_obj
   =(new ImageSourceConverter()).ConvertFrom(binaryData);

  Debug.Assert(!Object.ReferenceEquals(imageSource_obj,null));

  Debug.Assert(imageSource_obj is ImageSource);

  var imageSource
   =(ImageSource)imageSource_obj;

  return imageSource;
 }//BinaryToImageSource

 //-----------------------------------------------------------------------
 public static BitmapSource? BinaryToBitmapSource(byte[] binaryData)
 {
  if(Object.ReferenceEquals(binaryData,null))
   return null;

  var imageSource_obj
   =BinaryToImageSource(binaryData);

  Debug.Assert(!Object.ReferenceEquals(imageSource_obj,null));

  //Note: It works but looks very suspicios

  Debug.Assert(imageSource_obj is BitmapSource);

  var bitmapSource
   =(BitmapSource)imageSource_obj;

  return bitmapSource;
 }//BinaryToBitmapSource

 //-----------------------------------------------------------------------
 public static byte[]? BitmapSourceToBinary(BitmapSource bitmapSource)
 {
  if(Object.ReferenceEquals(bitmapSource,null))
   return null;

  JpegBitmapEncoder encoder=new JpegBitmapEncoder();

  encoder.QualityLevel=100;

  using MemoryStream stream=new MemoryStream();

  encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

  encoder.Save(stream);

  var binaryData=stream.ToArray();

  //expected
  Debug.Assert(!Object.ReferenceEquals(binaryData,null));

  stream.Close();

  return binaryData;
 }//BitmapSourceToBinary
};//class ImageUtils

////////////////////////////////////////////////////////////////////////////////
}//namespace MVVM