using System.Collections.Generic;

namespace MusicBeePlugin
{
    class PlaylistData
    {
        public string callback_function { get; set; }
        public List<string[]> Playlist { get; internal set; }
    }
}
