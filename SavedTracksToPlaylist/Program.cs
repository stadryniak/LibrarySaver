using System;
using SpotifyAPI.Web; //Base Namespace
using SpotifyAPI.Web.Auth; //All Authentication-related classes
using SpotifyAPI.Web.Enums; //Enums
using SpotifyAPI.Web.Models; //Models for the JSON-responses
using SavedTracksToPlaylist.Modules;


namespace SavedTracksToPlaylist
{
    class Program
    {

        static void Main()
        {
            //start menu
            Console.WriteLine( "Save Saved Tracks to Playlist" );
            Console.WriteLine( "Type number of selected option and press enter" );
            Console.WriteLine( "Select what do you want to do:" );
            Console.WriteLine( "[1] Save all tracks from library to playlist or update existing" );
            Console.WriteLine( "[2] Tracks features" );
            Console.WriteLine( "[0] Close" );

            //gets user input
            if ( short.TryParse( Console.ReadLine(), out short select ) )
            {
                Controller.Select = select;
            }
            else
            {
                Console.WriteLine( "It's not a number!" );
                Console.WriteLine( "\nPress any key to continue. . ." );
                Console.ReadKey();
                Environment.Exit( 0 );
            }

            if ( Controller.Select != 1 && Controller.Select != 2 )
            {
                Environment.Exit( 0 );
            }

            ImplicitGrantAuth auth = new ImplicitGrantAuth( AuthorizationCredits.Authcode, "http://localhost:4002", "http://localhost:4002", Scope.UserLibraryRead | Scope.UserReadPrivate | Scope.PlaylistModifyPublic );
            auth.AuthReceived += AuthOnAuthReceived;
            auth.Start();
            auth.OpenBrowser();
            Controller.WaitEvent.WaitOne();
            Console.WriteLine( "Closing connection..." );
            auth.Stop();
            Console.WriteLine( "Connection closed." );
        }

        private static void AuthOnAuthReceived( object sender, Token payload )
        {

            SpotifyWebAPI spotify = new SpotifyWebAPI
            {
                TokenType = payload.TokenType,
                AccessToken = payload.AccessToken
            };
            Controller controller = new Controller();
            controller.ApiController( spotify );
        }
    }
}
