using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace SPCASW.Web.Security
{
   public class SecurityModule : IHttpModule
   {

      private static readonly Regex _allowableRootFileRegEx = new Regex( @"/(google[A-Z0-9]+\.html|robots.txt|favicon.ico)$", RegexOptions.IgnoreCase | RegexOptions.Compiled );

      public void Dispose() { }

      public void Init( HttpApplication context )
      {
         context.AuthorizeRequest += ( s, e ) =>
         {
            var app = s as HttpApplication;

            try
            {
               if( app.Context.Trace.IsEnabled )
               {
                  app.Context.Trace.Write( "BEGIN  SecurityModule.AuthorizeRequest" );
               }

               if( true ) // TODO: for development, remove on publish
               {
                  FormsAuthentication.SetAuthCookie( "spcasw", true );
                  return;
               }

               var requiresRedirect = true;

               var request = app.Request;

               var pathAndQuery = request.Url.PathAndQuery;

               app.Context.Trace.Write( "PATH = " + pathAndQuery );

               string[] whiteListPaths = { 
                                         "/Account",
                                         "/Content",
                                         "/Service"
                                     };

               if( pathAndQuery == "/" || whiteListPaths.Any( x => pathAndQuery.StartsWith( x, StringComparison.OrdinalIgnoreCase ) ) )
               {
                  // These files are ok for anonymous users to access
                  requiresRedirect = false;
               }

               var path = request.Url.GetLeftPart( UriPartial.Path );

               if( _allowableRootFileRegEx.IsMatch( path ) )
               {
                  requiresRedirect = false;
               }

               if( app.Context.User.Identity.IsAuthenticated )
               {
                  requiresRedirect = false;
               }

               if( !requiresRedirect )
                  return;

               var redirectUrl = String.Format( "{0}?returnUrl={1}", FormsAuthentication.LoginUrl, request.Url.PathAndQuery );

               if( app.Context.Trace.IsEnabled )
               {
                  app.Context.Trace.Warn( String.Format( "Redirecting user to {0}", redirectUrl ) );
               }

               // The use is not authorized to access this resource
               app.Response.Redirect( redirectUrl );
            }
            finally
            {
               if( app.Context.Trace.IsEnabled )
               {
                  app.Context.Trace.Write( "END SecurityModule.AuthorizeRequest" );
               }
            }
         };
      }
   }
}