using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HowToGet.BusinessLogic.Providers;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Repositories.Mongo;
using HtmlAgilityPack;
using NLog;

namespace CarrierImporter
{
	class Program
	{
		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}
		
		private static Logger _loggerImage;
		private static Logger CurrentImageLogger
		{
			get { return _loggerImage ?? (_loggerImage = LogManager.GetLogger("image")); }
		}

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Specify a file name");
				return;
			}

			var fileName = args[0];
			CarrierTypes carrierType; 
			if (!CarrierTypes.TryParse(args[1], out carrierType))
			{
				Console.WriteLine("Unknown carrier type");
				return;
			}

			var regexpTitle = new Regex(@"===[\{\{]*[\[\[]*[\w '\|]+[\}\}]*[\]\]]*===", RegexOptions.Compiled);
			var regexpWords = new Regex(@"[^=*\[\]\{\}]+", RegexOptions.Compiled);
			var regexpName = new Regex(@"\*\[\[.+\]\]", RegexOptions.Compiled);
			var regexpClarify = new Regex(@"\(.+\)", RegexOptions.Compiled);
			var regexpUrl = new Regex(@"[\w -()_:]*\|", RegexOptions.Compiled);
			var regexpFlag = new Regex(@"flag\|", RegexOptions.Compiled);

			var lines = File.ReadLines(fileName);
			var carrierProvider = new CarrierProvider();
			var carrierRepository = new CarrierRepository();
			var countries = new CountryRepository().GetAllCountries();

			string country = string.Empty;
			string countryId = string.Empty;
			foreach (var line in lines)
			{
				if (regexpTitle.IsMatch(line))
				{
					var word = regexpWords.Match(line).Value;
					country = regexpFlag.Replace(word, string.Empty).Trim();

					var countryObj = countries.FirstOrDefault(s => s.Name == country);
					if (countryObj == null)
					{
						CurrentLogger.Error("Country not found! {0}", country);
						break;
					}
					countryId = countryObj.Id;
					CurrentLogger.Info("Processing Country = {0}, CountryId = {1}", country, countryId);
					CurrentLogger.Info("=====================================");
					continue;
				}
				if (regexpName.IsMatch(line))
				{
					var word = regexpWords.Match(line).Value;
					var url = regexpUrl.Match(word).Value.TrimEnd('|');
					bool isDirectUrl = false;
					string name;
					if (string.IsNullOrEmpty(url))
					{
						url = word;
						name = regexpClarify.Replace(word, string.Empty).TrimEnd();
					}
					else
					{
						name = regexpUrl.Replace(word, string.Empty).Trim();
						if (url.StartsWith("http://"))
							isDirectUrl = true;
					}


					CurrentLogger.Info("Processing Carrier = {0}", name);

					var carrier = carrierProvider.FindOneByName(name, carrierType);
					if (carrier == null)
					{
						CurrentLogger.Info("Carrier {0} is not exists", name);
						
						string carrierImage;
						string carrierUrl;
						if(!isDirectUrl)
							LoadCarrierInfo(url, country, name, out carrierUrl, out carrierImage);
						else
						{
							carrierImage = null;
							carrierUrl = url;
						}

						carrier = new Carrier
									  {
										  CountryId = countryId,
										  Name = name,
										  LowercaseName = name.ToLowerInvariant(),
										  Type = carrierType,
										  Web = !string.IsNullOrEmpty(carrierUrl) ? carrierUrl : null,
										  Icon = !string.IsNullOrEmpty(carrierImage) ? carrierImage : null,
									  };
						carrierProvider.CreateCarrier(carrier);
						CurrentLogger.Info("Carrier {0} created with id = {1}", name, carrier.Id);
					}
					else
					{
						CurrentLogger.Info("Carrier {0} already exists with id = {1}", name, carrier.Id);
						if (carrier.CountryId != countryId)
						{
							CurrentLogger.Info("Carrier {0} have wrong country (id = {1}). Fix countryId to {2}", name, carrier.CountryId, countryId);
							carrier.CountryId = countryId;
							carrierRepository.UpdateCountry(carrier);
						}
						if (string.IsNullOrEmpty(carrier.Web) || string.IsNullOrEmpty(carrier.Icon))
						{
							CurrentLogger.Info("Carrier {0} doent have url and image. Check again", name);

							string carrierImage;
							string carrierUrl;
							if (!isDirectUrl)
								LoadCarrierInfo(url, country, name, out carrierUrl, out carrierImage);
							else
							{
								carrierImage = string.Empty;
								carrierUrl = url;
							}

							if (!string.IsNullOrEmpty(carrier.Web) && carrier.Web != carrierUrl)
							{
								carrier.Web = carrierUrl;
								carrierRepository.UpdateWeb(carrier);
								CurrentLogger.Info("Updating carrier url to {1} for carrier {0}", name, carrierUrl);
							}
							if (!string.IsNullOrEmpty(carrier.Icon) && carrier.Icon != carrierImage)
							{
								carrier.Icon = carrierImage;
								carrierRepository.UpdateIcon(carrier);
								CurrentLogger.Info("Updating carrier icon to {1} for carrier {0}", name, carrierImage);
							}
						}
					}
				}
			}
		}

		private static void LoadCarrierInfo(string word, string country, string name, out string carrierUrl, out string carrierImage)
		{
			var addr = string.Format("http://en.wikipedia.org/wiki/{0}", word.Replace(" ", "_"));
			var doc = new HtmlDocument();
			var client = new WebClient();
			client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1312.57 Safari/537.17");
			string content = string.Empty;
			try
			{
				content = client.DownloadString(addr);
			}
			catch (WebException ex)
			{
				if (ex.Message.Contains("404"))
					CurrentLogger.Error("Unable to download. Country = {0}, Name = {1}, WikiAddr = {2}, Error = {3}", country, name, addr, ex.Message);
			}

			carrierImage = string.Empty;
			carrierUrl = string.Empty;

			doc.LoadHtml(content);

			var contentNode = doc.GetElementbyId("mw-content-text");
			if (contentNode != null)
			{
				var nodes = contentNode.ChildNodes.Where(s => s.Name == "table");
				foreach (var childNode in nodes)
				{
					var attr = childNode.Attributes["class"];
					if (attr != null && attr.Value == "infobox vcard")
					{
						var links = childNode.SelectNodes("tr/td//a[@class=\"external text\"]") ??
						            childNode.SelectNodes("tr/td//a[@class=\"external free\"]");
						if (links != null)
						{
							carrierUrl = links.First().Attributes["href"].Value;
							CurrentLogger.Info("Carrier {0} have a website {1}", name, carrierUrl);
						}
						else
						{
							CurrentLogger.Warn("URL NOT FOUND. Country = {0}, Name = {1}, WikiAddr = {2}", country, name, addr);
						}
						var images = childNode.SelectNodes("tr/td//a[@class=\"image\"]/img");
						if (images != null)
						{
							var imageAttr = images.First().Attributes["src"];
							if (imageAttr != null)
							{
								var image = imageAttr.Value;
								CurrentLogger.Info("Carrier {0} have image {1}", name, image);
								if (image.StartsWith("//"))
									image = "http:" + image;

								var uri = new Uri(image);
								carrierImage = uri.Segments[uri.Segments.Count() - 1];
								using (var imageClient = new WebClient())
								{
									try
									{
										imageClient.DownloadFile(image, AppDomain.CurrentDomain.BaseDirectory + "\\images\\" + carrierImage);
									}
									catch (WebException ex)
									{
										CurrentLogger.Error("Unable to download image. Country = {0}, Name = {1}, ImageAddr = {2}, Error = {3}", country,
										                    name, carrierImage, ex.Message);
									}
								}
							}
						}
						else
						{
							CurrentImageLogger.Debug("IMAGE NOT FOUND. Country = {0}, Name = {1}, WikiAddr = {2}", country, name, addr);
						}
					}
				}
			}
		}
	}
}
