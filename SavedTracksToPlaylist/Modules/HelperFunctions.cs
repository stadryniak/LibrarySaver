using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SavedTracksToPlaylist.Modules
{
    class PlaylistStatus
    {
        public string PlaylistId { get; set; }
        public bool IsPresent { get; set; }
    }

    //contains utility functions used by other classes
    internal static class HelperFunctions
    {
        public static PlaylistStatus PlaylistPresenceCheck( SpotifyWebAPI spotify ) //returns ID of playlist named "Library to Playlist"
        {
            Console.WriteLine( "Checking playlist \"Library to Playlist\" presence..." );
            var playlistStatus = new PlaylistStatus();
            var userId = spotify.GetPrivateProfile().Id;
            var offset = 0;
            while ( true )
            {
               var userPlaylists = spotify.GetUserPlaylists( userId, 50, offset );
                userPlaylists.Items.ForEach( playlist =>
                 {
                     if ( playlist.Name == "Library to Playlist" && playlist.Owner.Id == userId )
                     {
                         playlistStatus.PlaylistId = playlist.Id;
                         playlistStatus.IsPresent = true;
                     }
                 } );
                if ( !userPlaylists.HasNextPage() )
                {
                    break;
                }
            }
            return playlistStatus;

        }

        public static List<string> GetSavedTracksUris( SpotifyWebAPI spotify )
        {
            Console.WriteLine( "Generating list of saved tracks uris..." );
            var offset = 0;
            var tracksUri = new List<string>();
            while ( true )
            {
                Paging<SavedTrack> savedTracks = spotify.GetSavedTracks( 50, offset );
                savedTracks.Items.ForEach( track =>
                 {
                     tracksUri.Add( track.Track.Uri );
                 } );
                offset += 50;
                if ( !savedTracks.HasNextPage() )
                {
                    break;
                }

            }
            Console.WriteLine( "List generated successfully." );
            return tracksUri;
        }

        private static List<string> TracksUriList( SpotifyWebAPI spotify, string playlistId )
        {
            Console.WriteLine( "Generating list of playlist tracks uris..." );
            var offset = 0;
            var tracksUri = new List<string>();
            while ( true )
            {
                var playlistTracks = spotify.GetPlaylistTracks( playlistId, "next,items(track(uri))", 100, offset );
                if ( !playlistTracks.HasError() )
                {
                    playlistTracks.Items.ForEach( track =>
                     {
                         tracksUri.Add( track.Track.Uri );
                     } );
                    offset += 100;
                    if ( !playlistTracks.HasNextPage() )
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine( playlistTracks.Error.Status );
                    Console.WriteLine( playlistTracks.Error.Message );
                    break;
                }
            }
            Console.WriteLine( "Tracks uris generated successfully." );
            return tracksUri;
        }

        public static List<string> FindFirstCommonTrackUri( SpotifyWebAPI spotify, List<string> libraryUriList, string playlistId )
        {
            var playlistUriList = TracksUriList( spotify, playlistId );
            Console.WriteLine( "Calculating differences..." );
            var diffList = libraryUriList.Except( playlistUriList );
            var enumerableDiffList = diffList.ToList();

            Console.WriteLine( $"{enumerableDiffList.Count} tracks will be added." );

            return enumerableDiffList.ToList();
        }

        public static void InsertTracks( SpotifyWebAPI spotify, List<string> tracksUri, string playlistId )
        {
            Console.WriteLine( "Adding tracks to playlist..." );
            var position = 0;
            var howMany = tracksUri.Count;
            var iterationCounter = 1;
            if ( howMany > 100 )
            {
                howMany = 100;
            }
            while ( true )
            {
                if ( howMany * iterationCounter > tracksUri.Count )
                {
                    howMany = tracksUri.Count - 100 * iterationCounter + 100;
                }
                if ( position >= tracksUri.Count )
                {
                    break;
                }
                var addTracks = spotify.AddPlaylistTracks( playlistId, tracksUri.GetRange( position, howMany ), position );
                if ( addTracks.HasError() )
                {
                    Console.WriteLine( "Error while processing. Breaking." );
                    Console.WriteLine( addTracks.Error.Message );
                    break;
                }
                position += howMany;
                iterationCounter++;
            }
            Console.WriteLine( "Tracks added successfully." );
        }

    }
}
