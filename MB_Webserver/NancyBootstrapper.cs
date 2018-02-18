using Nancy;
using Nancy.Conventions;
using System;
using System.IO;
using Nancy.Responses;
using System.Resources;
using Response.Properties;
using System.Threading.Tasks;

namespace MusicBeePlugin
{
	class NancyBootstrapper : DefaultNancyBootstrapper
	{
		protected override IRootPathProvider RootPathProvider
		{
			get { return new CRootPathProvider(); }
		}

		protected override void ConfigureConventions(NancyConventions nancyConventions)
		{
			base.ConfigureConventions(nancyConventions);
			//nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("static","F:/Onedrive/Music"));
		}
	}

	public class CRootPathProvider : IRootPathProvider
	{
		public string GetRootPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MusicBee\\Plugins\\web");
		}
	}

	public static class FormatterExtensions
	{
		public static Nancy.Response AsStream(this IResponseFormatter formatter, Func<Stream> readStream, string contentType)
		{
			return  new StreamResponse(readStream, contentType);
		}
	}

	public class StreamResponse : Nancy.Response
	{
		public StreamResponse(Func<Stream> readStream, string contentType)
		{
			Contents = stream =>
			{
				using (var read = readStream())
				{
					try
					{
						read.CopyTo(stream);
					}
					catch (Exception)
					{

					}
				}
			};
			ContentType = contentType;
			StatusCode = HttpStatusCode.OK;
		}
	}

}
