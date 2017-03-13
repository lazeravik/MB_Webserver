using System;
using System.Collections.Generic;
using System.Globalization;
using WebServer;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    class GenerateResponse
    {
        public static MusicBeeApiInterface MbApi { get; set; }
        static NowPlayingData data = null;
        private static WSServer _wsref;

        public static WSServer WsRef
        {
            get { return _wsref; }
            set { _wsref = value; }
        }


        public static void Init()
        {

        }

        /// <summary>
        /// Calls MusicBee api function depending on the response
        /// </summary>
        /// <param name="response"></param>
        public static void HandleResponse(string response)
        {
            if (response is null)
            {
                return;
            }

            switch (response)
            {
                case "toggleplay":
                    MbApi.Player_PlayPause();
                    break;

                case "playprev":
                    MbApi.Player_PlayPreviousTrack();
                    break;

                case "playnext":
                    MbApi.Player_PlayNextTrack();
                    break;


                case "getnowplaylist":
                    GetNowPlaylist();
                    break;


                default:
                    break;
            }

            //if the response contains "/" backslash then it contain multiple values
            if (response.Contains("/"))
            {
                //Create an array of responses
                string[] responseArray = response.Split('/');
                if (responseArray.Length == 2)
                {
                    //Array of values that are passed with the request
                    var value = responseArray[1];
                    switch (responseArray[0])
                    {
                        case "settrackpos":
                            SetPlayerTrackPos(float.Parse(value, CultureInfo.InvariantCulture.NumberFormat));
                            break;

                        case "setvol":
                            SetVolume(float.Parse(value, CultureInfo.InvariantCulture.NumberFormat));
                            break;

                        case "playtrack":
                            PlayPlaylistTrack(value);
                            break;

                        default:
                            break;
                    }
                }
            }
        }



        /// <summary>
        /// Generates serialized JSON object with Now playing data to send to the browser
        /// </summary>
        /// <param name="isForcedOnce">Should we send the artwork data</param>
        /// <returns>Serialized json data</returns>
        public static string GetNowPlayingData(bool isForcedOnce)
        {
            float currentPos = MbApi.Player_GetPosition();
            float totalTime = MbApi.NowPlaying_GetDuration();
            float completed = currentPos / totalTime * 100;

            if (data == null)
                data = new NowPlayingData();

            string oldHash = Util.CreateMD5(data.CurrentTrackTitle + data.CurrentTrackArtist).ToLower();
            string curHash = Util.CreateMD5(MbApi.NowPlaying_GetFileTag(MetaDataType.TrackTitle) +
                    MbApi.NowPlaying_GetFileTag(MetaDataType.Artist)).ToLower();
            bool TrackChanged = (curHash != oldHash);

            data.HasTrackChanged = (TrackChanged || isForcedOnce);
            data.CurrentTrackTitle = MbApi.NowPlaying_GetFileTag(MetaDataType.TrackTitle);
            data.CurrentTrackArtist = MbApi.NowPlaying_GetFileTag(MetaDataType.Artist);
            data.CurrentPlayerState = MbApi.Player_GetPlayState().ToString();
            data.CurrentTrackSize = totalTime;
            data.CurrentTrackPosition = currentPos;
            data.CurrentTrackCompleted = completed;
            data.CurrentTrackSizeReadable = Util.FormattedMills(totalTime);
            data.CurrentTrackPositionReadable = Util.FormattedMills(currentPos);
            data.CurrentTrackAlbum = MbApi.NowPlaying_GetFileTag(MetaDataType.Album);
            data.CurrentTrackGenre = MbApi.NowPlaying_GetFileTag(MetaDataType.Genres);
            data.TrackArtwork = (TrackChanged || isForcedOnce) ? GetCurrentTrackArtwork() : string.Empty;
            data.CurrentVolume = MbApi.Player_GetVolume();

            var allfile = MbApi.NowPlayingList_QueryGetNextFile();
            data.NextQueueTrack = allfile;


            data.callback_function = "refreshPlayerControl";

            return Util.Serialize(data);
        }

        private static void SetPlayerTrackPos(float pos)
        {
            int position = Convert.ToInt32(pos / 100 * MbApi.NowPlaying_GetDuration());
            MbApi.Player_SetPosition(position);
        }




        /// <summary>
        /// Returns base64 image format for browser for rendering
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentTrackArtwork()
        {
            return "data:image/png;base64," + MbApi.NowPlaying_GetArtwork();
        }

        public static void GetNowPlaylist()
        {
            PlaylistData data = new PlaylistData();
            List<string[]> list = new List<string[]>();
            string url = null;

            MetaDataType[] fields = {
                    MetaDataType.TrackTitle,
                    MetaDataType.Album,
                    MetaDataType.Artist,
                    MetaDataType.Year,
                    MetaDataType.Rating,
                    MetaDataType.RatingLove,
                    MetaDataType.Genre
                };
            string[] playlist = null;
            int i = 0;
            while (true)
            {
                MbApi.NowPlayingList_GetFileTags(i, fields, ref playlist);
                url = MbApi.NowPlayingList_GetFileProperty(i, FilePropertyType.Url);

                if (playlist is null)
                {
                    break;
                }

                Array.Resize(ref playlist, playlist.Length + 1);
                playlist[7] = url;

                list.Add(playlist);
                playlist = null;
                i++;
            }


            data.Playlist = list;
            data.callback_function = "SetPlaylistData";

            WsRef.SendMessage(Util.Serialize(data));
        }


        private static void PlayPlaylistTrack(string url)
        {
            MbApi.NowPlayingList_PlayNow(url);
        }

        /// <summary>
        /// Sets the MusicBee's volume with the value
        /// </summary>
        /// <param name="value"></param>
        private static void SetVolume(float value)
        {
            //MusicBee uses 0 to 1 volume range so clamp the values
            value = Mathf.Range(value, 0, 1);
            MbApi.Player_SetVolume(value);
        }
    }
}
