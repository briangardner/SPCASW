using System;

namespace SPCASW.Common
{
   [Serializable]
   public class JsonSuccess
   {
      public bool Success { get; set; }
      public string Message { get; set; }
   }

   [Serializable]
   public class JsonSuccess<T> : JsonSuccess
   {
      public T Data { get; set; }
   }
}