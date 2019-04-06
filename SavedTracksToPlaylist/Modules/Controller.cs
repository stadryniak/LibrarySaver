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
            int numberOfTracks = 0;
            List<string> tracksURI = new List<string>();
            while (true)
            {
                Paging<SavedTrack> savedTracks = _spotify.GetSavedTracks(50, offset);
                savedTracks.Items.ForEach(track =>
                {


                    tracksURI.Add(track.Track.Uri);
                    
                    numberOfTracks++;
                });
                offset += 50;
                if (!savedTracks.HasNextPage())
                {
                    break;
                }
            }

            Console.WriteLine($"Number of tracks: {tracksURI.Count}");


            // gets user id
            string profileID="";
            PrivateProfile user = _spotify.GetPrivateProfile();
            if (user.HasError())
            {
                Console.WriteLine("\n\nProfileID not read properly. Cannot create playlist.\n\n");
            }
            profileID = user.Id.ToString();
            
            //creats new empty playlist
            FullPlaylist playlist = _spotify.CreatePlaylist(profileID, "This is my new Playlist");
            if (playlist.HasError())
            {
                Console.WriteLine("/n/nError while creating playlist.\n\n");
            }

            //gets playlist id
            string playlistID = playlist.Id.ToString();

            //renames playlist

            ErrorResponse response1 = _spotify.UpdatePlaylist(playlistID, "Library to Playlist", true);
            if (!response1.HasError())
                Console.WriteLine("Successfully set playlist name to \"Library to Playlist\"");


            //inserts playlist tracks to playlist
            int position = 0;
            int howMany = tracksURI.Count;
            int iterationCounter = 1;
            if (howMany > 100)
            {
                howMany = 100;
            }
            while (true)
            {
                if (howMany * iterationCounter > tracksURI.Count)
                {
                    howMany = tracksURI.Count - 100 * iterationCounter + 100;
                }
                if (position >= tracksURI.Count)
                {
                    break;
                }
                ErrorResponse response2 = _spotify.AddPlaylistTracks(profileID, playlistID, tracksURI.GetRange(position, howMany), position);
                //Console.WriteLine($"Position = {position}, howMany = {howMany}");
                if (response2.HasError())
                {
                    Console.WriteLine("Error while processing. Breaking.");
                    Console.WriteLine(response2.Error.Message.ToString());
                    break;
                }
                position += howMany;
                iterationCounter++;
           }
            Console.WriteLine("Task ended. Press any key to continue. . .");
        }
    }
}
