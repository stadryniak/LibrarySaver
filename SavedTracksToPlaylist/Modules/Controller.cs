using System;
using System.Collections.Generic;
using SpotifyAPI.Web; //Base Namespace
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
                    LibraryToPlaylist.AllLibraryToPlaylist(_spotify);
                    break;
                default:
                    break;
            }
        }

        
    }
}
