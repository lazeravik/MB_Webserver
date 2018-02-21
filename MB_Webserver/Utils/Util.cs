using System;
using System.Text;
using System.Web.Script.Serialization;

namespace MusicBeePlugin
{
    class Util
    {
        public static string FormattedMills(float mills)
        {
            var span = new TimeSpan(0, 0, Convert.ToInt32(mills / 1000)); //Or TimeSpan.FromSeconds(seconds); (see Jakob C´s answer)
            return string.Format("{0}:{1:00}",
                                        (int)span.TotalMinutes,
                                        span.Seconds);


        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string Serialize<T>(T data)
        {
            var serialized = string.Empty;

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serialized = serializer.Serialize(data);
            }
            catch (Exception)
            {
            }


            return serialized;
        }

		public static string Encode64(string source)
		{
			var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
			return encodedString;
		}

		public static string Decode64(string source)
		{
			var base64EncodedBytes = Convert.FromBase64String(source.Replace(' ', '+'));
			return Encoding.UTF8.GetString(base64EncodedBytes);
		}

	}
}
