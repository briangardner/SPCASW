using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SPCASW.Common.Utilities
{
   public class DynamicConverter<TSource, TDest>
           where TDest : new()
   {
      private Func<TSource, TDest> _converter;

      private void CreateConverterIfNeeded()
      {
         if( _converter == null )
         {
            var source = Expression.Parameter( typeof( TSource ), "source" );
            var dest = Expression.Variable( typeof( TDest ), "dest" );

            var assignments = from srcProp in typeof( TSource ).GetProperties(
                                  BindingFlags.Public | BindingFlags.Instance )
                              where srcProp.CanRead
                              let destProp = typeof( TDest ).GetProperty(
                                  srcProp.Name,
                                  BindingFlags.Public | BindingFlags.Instance )
                              where ( destProp != null ) && ( destProp.CanWrite )
                              select Expression.Assign(
                                  Expression.Property( dest, destProp ),
                                  Expression.Property( source, srcProp ) );

            // put together the body:
            var body = new List<Expression>
               {
                  Expression.Assign(dest,
                                    Expression.New(typeof(TDest)))
               };
            body.AddRange( assignments );
            body.Add( dest );

            var expr =
                Expression.Lambda<Func<TSource, TDest>>(
                    Expression.Block(
                    new[] { dest }, // expression parameters
                    body.ToArray() // body
                    ),
                    source  // lambda expression
                );

            var func = expr.Compile();
            _converter = func;
         }
      }

      public TDest ConvertFrom( TSource source )
      {
         CreateConverterIfNeeded();
         return _converter( source );
      }
   }
}