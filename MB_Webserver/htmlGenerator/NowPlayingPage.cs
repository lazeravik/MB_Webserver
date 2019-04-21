using System;
using System.Text;
using MusicBeePlugin.IhtmlGenerator;

namespace MusicBeePlugin.htmlGenerator
{
	class NowPlayingPage : IHtmlGenerator
	{
		public string GetGeneratedResponse()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(
				$@"        
		<div class='song-info' id='song-info'>
			<div class='overlay'>
                <div class='artwork'>
                    <img id = 'current-track-artwork' class='current-track-artwork' src='/img/artwork-default.png'>
                </div>
                <div class='trackinfo'>
                    <h1 id = 'title-big' class='track-title'>Track Title</h1>
                    <p><span id = 'artist-big' class='artist'>Artist name</span><span id = 'album-big' class='album'>Album name</span></p>
					<p class='artistinfo' id='artistInfo'></p>
                </div>
            </div>
        </div>"
				);

			return sb.ToString();
		}
	}
}
