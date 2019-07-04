using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;

namespace SavedTracksToPlaylist.Modules
{
    static class PlaylistUpdater
    {
        internal static void PlaylistUpdate(SpotifyWebAPI spotify, string playlistId)
        {
            var tracksUri = HelperFunctions.FindFirstCommonTrackUri(spotify, HelperFunctions.GetSavedTracksUris(spotify), playlistId);

            if (tracksUri.Count != 0)
            {
                HelperFunctions.InsertTracks(spotify, tracksUri, playlistId);
            }
            else
            {
                Console.WriteLine("Nothing to add.");
            }
        }
    }
}
