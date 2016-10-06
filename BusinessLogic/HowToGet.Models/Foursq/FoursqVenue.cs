namespace HowToGet.Models.Foursq
{
	public class FoursqVenue
    {
        /// <summary>
        /// A unique string identifier for this venue.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The best known name for this venue.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  An object containing none, some, or all of address (street address), crossStreet, city, state, postalCode, country, lat, lng, and distance. All fields are strings, except for lat, lng, and distance. Distance is measured in meters. 
        ///  Some venues have their locations intentionally hidden for privacy reasons (such as private residences). If this is the case, the parameter isFuzzed will be set to true, and the lat/lng parameters will have reduced precision. 
        /// </summary>
		public FoursqLocation Location { get; set; }
    }
}