using System;
using System.Threading;
using SpotifyAPI.Web; //Base Namespace

namespace SavedTracksToPlaylist.Modules
{
    internal class Controller
    {
        public static readonly AutoResetEvent WaitEvent = new AutoResetEvent(false);
        public static int Select { get; set; }
        public void ApiController(SpotifyWebAPI spotify)
        {
            var playlistStatus = HelperFunctions.PlaylistPresenceCheck(spotify);

            switch (Select)
            {
                case 1:
                {
                    if ( !playlistStatus.IsPresent )
                    {
                        Console.WriteLine("Creating Playlist");
                        LibraryToPlaylist.AllLibraryToPlaylist(spotify);
                    }
                    else
                    {
                        Console.WriteLine($"Existing playlist detected. ID of existing playlist is: {playlistStatus.PlaylistId}. Type y to confirm update");
                        var confirm = Console.ReadLine();
                        if (confirm == "y" || confirm == "Y")
                        {
                            Console.WriteLine("Update confirmed. Proceeding...");
                            PlaylistUpdater.PlaylistUpdate(spotify, playlistStatus.PlaylistId);
                        }
                    }
                    break;
                }

                case 2:
                {
                    Analyse.GetSavedTrackProperties(spotify);
                    break;
                }

            }

            WaitEvent.Set();

        }
    }
}
