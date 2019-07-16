using System;
using SpotifyAPI.Web; //Base Namespace
using SpotifyAPI.Web.Models; //Models for the JSON-responses

namespace SavedTracksToPlaylist.Modules
{
    internal class LibraryToPlaylist
    {
        public void AllLibraryToPlaylist( SpotifyWebAPI spotify )
        {
            //saves all track ids to list

            var tracksUri = HelperFunctions.GetSavedTracksUris( spotify );

            Console.WriteLine( $"Number of tracks: {tracksUri.Count}" );


            // gets user id
            PrivateProfile user = spotify.GetPrivateProfile();
            if ( user.HasError() )
            {
                Console.WriteLine( "\n\nProfileID not read properly. Cannot create playlist.\n\n" );
            }
            var userId = user.Id;

            //creates new empty playlist
            FullPlaylist playlist = spotify.CreatePlaylist( userId, "Library to Playlist" );

            //error notification
            Console.WriteLine( playlist.HasError()
                ? "/n/nError while creating playlist.\n\n"
                : "Playlist created and named \"Library to Playlist\"" );


            //inserts playlist tracks to playlist
            HelperFunctions.InsertTracks( spotify, tracksUri, playlist.Id );

            Console.WriteLine( "\nWarning: there may be less songs added to playlist than are present in library, because they might be not available" );
            Console.WriteLine( "\nTask ended. Press any key to exit. . ." );
        }
    }
}
