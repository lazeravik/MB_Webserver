using MusicBeePlugin.htmlGenerator;
using Nancy;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
	public class MainModule : NancyModule
    {
        public static MusicBeeApiInterface MbApi { get; set; }

        public MainModule()
        {
            //Define the root view
            Get["/"] = p => View[$"/index.html"];

            //Define the Javscript, CSS and image directory
            Get[@"/js/{name}"] = x =>
            {
                return Response.AsFile(string.Format("./js/{0}",(string)x.name));
            };

            Get[@"/css/{name}"] = x =>
            {
                return Response.AsFile(string.Format("./css/{0}", (string)x.name));
            };

            Get[@"/img/{name}"] = x =>
            {
                return Response.AsImage(string.Format("./img/{0}", (string)x.name));
            };

			Get[@"/currentArtwork/{hash}.jpg"] = x =>
			{
				return Response.AsStream(() => GenerateResponse.GetCurrentArtwork(), "image/*");
			};

			Get[@"/blur/{hash}.jpg"] = x =>
			{
				return Response.AsStream(() => GenerateResponse.GetCurrentArtworkBlur(), "image/*");
			};

			Get[@"/mimg/{path*}"] = x =>
			{
				return Response.AsStream(() => GenerateResponse.GetArtwork(
					Util.Decode64(((string)x.path).Replace(".jpg", "")), 40, 40), "image/*");
			};

			Get[@"/cimg/{path*}"] = x =>
			{
				return Response.AsStream(() => GenerateResponse.GetArtwork(
					Util.Decode64(((string)x.path).Replace(".jpg", "")), 120, 120), "image/*");
			};

			Get[@"/queryfiles/{query}"] = q =>
			{
				return Response.AsJson(GenerateResponse.QueryFiles(), HttpStatusCode.OK);
			};

			Get[@"/play/file/{track}"] = q =>
			{
				var trackPath = Util.Decode64(q.track);
				GenerateResponse.PlayTrack(trackPath);
				return Response.AsText((string)trackPath);
			};

			Get[@"/html/nowplayinglist"] = q =>
			{
				return Response.AsText(new NowplayinglistGenerator(
					GenerateResponse.GetNowPlaylist()).GetGeneratedResponse(), "text/html");
			};

			Get[@"/html/getlibrary"] = q =>
			{
				return Response.AsText(new LibraryGenerator(
					GenerateResponse.QueryFiles()).GetGeneratedResponse(), "text/html");
			};


			Get[@"/html/getNowPlaying"] = q =>
			{
				return Response.AsText(new NowPlayingPage().GetGeneratedResponse(), "text/html");
			};

		}
	}
}
