using System.Collections.Generic;

namespace MusicBeePlugin.Models
{
    class PlaylistData
    {
        public string callback_function { get; set; }
        public List<string[]> Playlist { get; internal set; }
    }
}
