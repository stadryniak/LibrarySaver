using System;
using SpotifyAPI.Web; //Base Namespace
using SpotifyAPI.Web.Models; //Models for the JSON-responses

namespace SavedTracksToPlaylist.Modules
{
    internal static class LibraryToPlaylist
    {
        public static void AllLibraryToPlaylist(SpotifyWebAPI spotify)
        {
            //saves all track ids to list

            var tracksUri = HelperFunctions.GetSavedTracksUris(spotify);

            Console.WriteLine($"Number of tracks: {tracksUri.Count}");


            // gets user id
            PrivateProfile user = spotify.GetPrivateProfile();
            if (user.HasError())
            {
                Console.WriteLine("\n\nProfileID not read properly. Cannot create playlist.\n\n");
            }
            string userId = user.Id;

            //creates new empty playlist
            FullPlaylist playlist = spotify.CreatePlaylist(userId, "Library to Playlist");
            if (playlist.HasError())
            {
                Console.WriteLine("/n/nError while creating playlist.\n\n");
            }
            else
            {
                Console.WriteLine("Playlist created and named \"Library to Playlist\"");
            }


            //inserts playlist tracks to playlist
            HelperFunctions.InsertTracks(spotify, tracksUri, playlist.Id);

            Console.WriteLine("\nWarning: there may be less songs added to playlist than are present in library, because they might be not available");
            Console.WriteLine("\nTask ended. Press any key to exit. . .");
        }
    }
}
