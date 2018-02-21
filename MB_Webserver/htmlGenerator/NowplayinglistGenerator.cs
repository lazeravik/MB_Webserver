using MusicBeePlugin.IhtmlGenerator;
using MusicBeePlugin.Models;
using System.Text;

namespace MusicBeePlugin.htmlGenerator
{
	class NowplayinglistGenerator : IHtmlGenerator
	{
		AlbumList List;
		public NowplayinglistGenerator(AlbumList data)
		{
			List = data;
		}

		public string GetGeneratedResponse()
		{
			if(List.AlbumLists.Count <= 0) { return string.Empty; }

			StringBuilder sb = new StringBuilder();


			foreach (var album in List.AlbumLists)
			{
				foreach (var track in album.TrackList)
				{
					var loveState = (track.Loved == "L") ? "loved" : "notloved";
					var loveHtml = (track.Loved == "L") ? "<i class='ficon icon-heart'></i>" :
														  "<i class='ficon icon-heart-empty'></i>";

					var obfuscatedFilePath = Util.Encode64(track.FilePath);
					sb.Append(
						$@"<li class='playlist-contextmnu ' data-fileurl='file/{obfuscatedFilePath}' onclick='playTrack(this)'>
							<img class='b-lazy' src='data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==' 
							data-src='mimg/{obfuscatedFilePath}.jpg'>
							<span class='title'>{track.TrackTitle}</span>
							<span class='artist'>{track.Artist}</span>
							<span class='album'>{track.Album}</span>
							<span class='love {loveState}'>{loveHtml}</span>
						</li>"
						);
				}
			}

			return sb.ToString();
		}
	}
}
