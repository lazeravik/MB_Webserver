using MusicBeePlugin.IhtmlGenerator;
using MusicBeePlugin.Models;
using System.Linq;
using System.Text;

namespace MusicBeePlugin.htmlGenerator
{
	class LibraryGenerator : IHtmlGenerator
	{
		AlbumList Lists;
		public LibraryGenerator(AlbumList list)
		{
			Lists = list;
		}

		public string GetGeneratedResponse()
		{
			if(Lists.AlbumLists.Count <= 0) { return string.Empty; }

			int x;
			//var newAlbumList = Lists.AlbumLists.GroupBy(c => 
			//(c.Album[0]==' ' || int.TryParse(c.Album[0].ToString(), out x))?: c.Album);
			var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");

			var newAlbumList = from album in Lists.AlbumLists
							   group album by new
							   {
								   FirstLetter = int.TryParse(album.Album[0].ToString(), out x) || 
								   album.Album[0]==' ' || !regexItem.IsMatch(album.Album[0].ToString())
							   } into newAlbumGrp 
							   group newAlbumGrp.Key by newAlbumGrp.Key.FirstLetter into anotherGrp
							   orderby anotherGrp.Key
							   select anotherGrp;


			StringBuilder sb = new StringBuilder();

			sb.Append("<ul class='albumcover-view'>");
			foreach (var album in Lists.AlbumLists)
			{
				var imagePath = Util.Encode64(album.TrackList[0].FilePath);
				sb.Append(
					$@"<li>
						<div class='cover'>
							<img class='b-lazy' src='data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==' 
							data-src='cimg/{imagePath}.jpg'>
							<p class='album-name'>{album.Album}</p>
							<p class='artist-name'>{album.AlbumArtist}</p>
						</div>
						<ul class='track-list '>
					"
					);
				foreach (var track in album.TrackList)
				{
					sb.Append(
						$@"<li><button data-trackno='{track.TrackNo}'>
								<p class='trackname'>{track.TrackTitle}</p>
								<p class='track-artist'>{track.Artist}</p>
						</button></li>"
						);
				}
				sb.Append("</ul></li>");
			}
			sb.Append("</ul>");

			return sb.ToString();
		}
	}
}
