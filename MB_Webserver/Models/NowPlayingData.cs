namespace MusicBeePlugin.Models
{
    class NowPlayingData
    {
        public bool HasTrackChanged { get; set; }
        public string CurrentPlayerState { get; set; }
        public string CurrentTrackTitle { get; set; }
        public string CurrentTrackArtist { get; set; }
        public float CurrentTrackSize { get; set; }
        public float CurrentTrackPosition { get; set; }
        public float CurrentTrackCompleted { get; set; }
        public string ArtworkPath { get; set; }
        public string CurrentTrackAlbum { get; set; }
        public string CurrentTrackSizeReadable { get; set; }
        public string CurrentTrackPositionReadable { get; set; }
        public string CurrentTrackGenre { get; set; }
        public float CurrentVolume { get; set; }

		public ArtistData ArtistDataset { get; set; }

        public string[] NowPlayingList { get; set; }
        public string[] NowPlayingListAll { get; set; }

        public string NextQueueTrack { get; set; }

        public string callback_function { get; set; }

    }

    class NowPlayingList
    {
        public NowPlayingList(string list)
        {
            List = list;
        }

        public string List { get; set; }
    }
}
