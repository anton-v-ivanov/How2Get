namespace HowToGet.Models.Foursq
{
	public class FoursqCheckin
	{
		/// <summary>
		/// A unique identifier for this checkin.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// One of checkin, shout, or venueless.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// optional If the user is not clear from context, then a compact user is present.
		/// </summary>
		public FoursqUser User { get; set; }

		/// <summary>
		/// optional If the venue is not clear from context, and this checkin was at a venue, then a compact venue is present.
		/// </summary>
		public FoursqVenue Venue { get; set; }

		/// <summary>
		/// optionalIf the type of this checkin is shout or venueless, this field may be present and may contains a lat, lng pair and/or a name, providing unstructured information about the user's current location.
		/// </summary>
		public FoursqLocation Location { get; set; }
	}
}