using Nancy;
using System;
using System.IO;
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

			Get[@"/currentArtwork.jpg"] = x =>
			{
				return Response.AsStream(() => GenerateResponse.GetCurrentArtwork(), "image/*");
			};

			Get[@"/mimg/{path*}"] = x =>
			{
				return Response.AsStream(() => GenerateResponse.GetArtwork((string)x.path), "image/*");
			};

		}
    }
}
