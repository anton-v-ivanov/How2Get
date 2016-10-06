using HowToGet.Models.Dictionaries;

namespace HowToGet.Web.Models
{
    public class ExternalLoginModel
    {
		public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }
        
		public string LastName { get; set; }
		
		public string Gender { get; set; }

	    public string Picture { get; set; }

		public ExternalAuthServices AuthService { get; set; }

	    public string City { get; set; }

	    public string Country { get; set; }

	    public string Referrer { get; set; }

		//public string InviteCode { get; set; }
		
		public string AccessToken { get; set; }
    }
}