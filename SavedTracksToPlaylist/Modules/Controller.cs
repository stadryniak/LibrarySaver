using System;
using System.Collections.Generic;
using System.Text;
using SpotifyAPI.Web; //Base Namespace
using SpotifyAPI.Web.Auth; //All Authentication-related classes
using SpotifyAPI.Web.Enums; //Enums
using SpotifyAPI.Web.Models; //Models for the JSON-responses

namespace SavedTracksToPlaylist.Modules
{
    class Controller
    {
        public static int Select { get; set; }

        public void apiController(SpotifyWebAPI _spotify)
        {
            switch (Select)
            {
                case 1:
                    AllLibraryToPlaylist(_spotify);
                    break;
                default: break;
            }
        }

        private void AllLibraryToPlaylist(SpotifyWebAPI _spotify)
        {
            //saves all track ids to list
            int offset = 0;
            int i = 1;
            List<string> tracksIDs = new List<string>();
            while (true)
            {
                Paging<SavedTrack> savedTracks = _spotify.GetSavedTracks(50, offset);
                savedTracks.Items.ForEach(track =>
                {
                    tracksIDs.Add(track.Track.Id);
                    Console.WriteLine(track.Track.Name);
                    Console.WriteLine(i);
                    i++;
                });
                offset += 50;
                if (!savedTracks.HasNextPage())
                {
                    break;
                }
            }
            //prints all ids of tracks
            foreach (string id in tracksIDs)
            {
                    Console.WriteLine(id);
            }
            Console.WriteLine(offset);
        }

    }
}
