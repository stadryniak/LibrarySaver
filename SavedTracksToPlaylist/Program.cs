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
        static void Main(string[] args)
        {
            //start menu
            Console.WriteLine("Save Saved Tracks to Playlist");
            Console.WriteLine("Type number of selected option and press enter");
            Console.WriteLine("Select what do you want to do:");
            Console.WriteLine("[1] Save all tracks from library to playlists");
            Console.WriteLine("[0] Close");
            try
            {
                Controller.Select = Int16.Parse(Console.ReadLine()); // user select to int
            }
            catch (FormatException e)
            {
                Console.WriteLine("It's not a number!");
                Console.WriteLine($"\nError message: \n{e}");
                Console.WriteLine("\nPress any key to continue. . .");
                Console.ReadKey();
                System.Environment.Exit(0);
            }

            if (Controller.Select == 0 || Controller.Select != 1)
            {
                System.Environment.Exit(0);
            } 

            ImplicitGrantAuth auth = new ImplicitGrantAuth(Modules.AuthorizationCredits.authcode, "http://localhost:4002", "http://localhost:4002", Scope.UserLibraryRead | Scope.UserReadPrivate | Scope.PlaylistModifyPublic);
            auth.AuthReceived += AuthOnAuthReceived;
            auth.Start();
            auth.OpenBrowser();
            Console.ReadKey();
            auth.Stop(0);

        }

        public static void AuthOnAuthReceived(object sender, Token payload)
        {
            SpotifyWebAPI _spotify = new SpotifyWebAPI
            {
                TokenType = payload.TokenType,
                AccessToken = payload.AccessToken
            };
            Controller controller = new Controller();
            controller.apiController(_spotify);
        }
    }
}
