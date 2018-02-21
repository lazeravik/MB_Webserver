using System.Collections.Generic;

namespace MusicBeePlugin.Models
{
	class TrackData
	{
		public string Album { get; internal set; }
		public string AlbumArtist { get; internal set; }
		public string Artist { get; internal set; }
		public string TrackNo { get; internal set; }
		public string Rating { get; internal set; }
		public string FilePath { get; internal set; }
		public string TrackTitle { get; set; }
		public string Loved { get; set; }
	}

	class AlbumData
	{
		public string Album { get; internal set; }
		public string AlbumArtist { get; internal set; }
		public List<TrackData> TrackList { get; set; }

	}

	class AlbumList
	{
		public List<AlbumData> AlbumLists { get; set; }
		public string callback_function { get; set; }

	}
}
