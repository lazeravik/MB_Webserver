using Nancy;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class MainModule : NancyModule
    {
        private const string WEB_DIR = "Plugins/web";
        public static MusicBeeApiInterface MbApi { get; set; }

        public MainModule()
        {
            //Define the root view
            Get["/"] = p => View[$"{WEB_DIR}/index.html"];

            //Define the Javscript, CSS and image directory
            Get[@"/js/{name}"] = x =>
            {
                return Response.AsFile(string.Format("{0}/js/{1}", WEB_DIR, (string)x.name));
            };

            Get[@"/css/{name}"] = x =>
            {
                return Response.AsFile(string.Format("{0}/css/{1}", WEB_DIR, (string)x.name));
            };

            Get[@"/img/{name}"] = x =>
            {
                return Response.AsImage(string.Format("{0}/img/{1}", WEB_DIR, (string)x.name));
            };

            Get[@"/music/{name}"] = x =>
            {
                return Response.AsFile(string.Format("{0}/{1}", "../../../Onedrive/Music/DOES", (string)x.name));
            };

            //Get["/action/{name}/{val}"] = action =>
            //{
            //    string val = action.val.Value;
            //    string data = null;
            //    GenerateResponse.mbApi = MbApi;

            //    switch (action.name.Value)
            //    {
            //        case "get_nowplaying_data":
            //            data = GenerateResponse.GetNowPlayingData(val);
            //            break;

            //        case "toggleplay":
            //            MbApi.Player_PlayPause();
            //            break;

            //        case "play_next":
            //            MbApi.Player_PlayNextTrack();
            //            break;

            //        case "play_prev":
            //            MbApi.Player_PlayPreviousTrack();
            //            break;

            //        case "set_track_pos":
            //                GenerateResponse.SetPlayerTrackPos(
            //                    float.Parse(val, CultureInfo.InvariantCulture.NumberFormat));
            //            break;

            //        default:
            //            break;
            //    }
            //    if (data != null)
            //    {
            //        return new Response()
            //        {
            //            StatusCode = HttpStatusCode.OK,
            //            ContentType = "application/json",
            //            Contents = (stream) =>
            //            {
            //                byte[] dataArray = Encoding.UTF8.GetBytes(data);
            //                stream.Write(dataArray, 0, dataArray.Length);
            //            }
            //        };
            //    } else
            //    {
            //        return new Response()
            //        {
            //            StatusCode = HttpStatusCode.OK,
            //            ContentType = "application/json",
            //            Contents = (stream) => {
            //                byte[] dataArray = Encoding.UTF8.GetBytes("{}");
            //                stream.Write(dataArray, 0, dataArray.Length);
            //            }
            //        };
            //    }
            //};
        }
    }
}
