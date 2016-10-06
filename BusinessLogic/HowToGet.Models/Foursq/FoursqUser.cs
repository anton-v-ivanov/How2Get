namespace HowToGet.Models.Foursq
{
    public class FoursqUser
    {
        /// <summary>
        /// A unique identifier for this user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A user's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// A user's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// male or female
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// URL of a profile picture for this user.
        /// </summary>
        public string Photo { get; set; }

		/// <summary>
        /// User's home city
        /// </summary>
        public string HomeCity { get; set; }

        /// <summary>
        /// One of brand, celebrity, or user. Users can establish following relationships with celebrities.
        /// </summary>
        public string Type { get; set; }
    }
}