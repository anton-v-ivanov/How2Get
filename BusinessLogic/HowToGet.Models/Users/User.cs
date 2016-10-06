using System;
using System.Collections.Generic;
using System.Web.Security;
using HowToGet.Models.Dictionaries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Users
{
	[Serializable]
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("uname")]
		public string Username { get; set; }

		[BsonElement("lname")]
		public string LowercaseUsername { get; set; }

		[BsonElement("email")]
		public string Email { get; set; }

		[BsonElement("lemail")]
		public string LowercaseEmail { get; set; }

		[BsonElement("cmnt")]
		[BsonIgnoreIfNull]
		public string Comment { get; set; }

		[BsonElement("pass")]
		public string Password { get; set; }

		[BsonElement("fmt")]
		public MembershipPasswordFormat PasswordFormat { get; set; }
		
		[BsonElement("salt")]
		public string PasswordSalt { get; set; }

		[BsonElement("qstion")]
		[BsonIgnoreIfNull]
		public string PasswordQuestion { get; set; }

		[BsonElement("ans")]
		[BsonIgnoreIfNull]
		public string PasswordAnswer { get; set; }

		[BsonElement("apprvd")]
		public bool IsApproved { get; set; }

		[BsonElement("actdate")]
		public DateTime LastActivityDate { get; set; }

		[BsonElement("logindate")]
		public DateTime LastLoginDate { get; set; }

		[BsonElement("passdate")]
		public DateTime LastPasswordChangedDate { get; set; }

		[BsonElement("create")]
		public DateTime CreateDate { get; set; }

		[BsonElement("lockd")]
		public bool IsLockedOut { get; set; }

		[BsonElement("lockdate")]
		[BsonIgnoreIfNull]
		[BsonIgnoreIfDefault]
		public DateTime LastLockedOutDate { get; set; }

		[BsonElement("passcount")]
		[BsonIgnoreIfNull]
		[BsonIgnoreIfDefault]
		public int FailedPasswordAttemptCount { get; set; }

		[BsonElement("passwindow")]
		[BsonIgnoreIfNull]
		[BsonIgnoreIfDefault]
		public DateTime FailedPasswordAttemptWindowStart { get; set; }

		[BsonElement("anscount")]
		[BsonIgnoreIfNull]
		[BsonIgnoreIfDefault]
		public int FailedPasswordAnswerAttemptCount { get; set; }

		[BsonElement("answindow")]
		[BsonIgnoreIfNull]
		[BsonIgnoreIfDefault]
		public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

		[BsonElement("roles")]
		[BsonIgnoreIfNull]
		public List<string> Roles { get; set; }

		[BsonElement("pct")]
		[BsonIgnoreIfNull]
		public string Picture { get; set; }

		[BsonElement("gndr")]
		public GenderTypes Gender { get; set; }

		[BsonElement("hcntry")]
		[BsonIgnoreIfNull]
		[BsonRepresentation(BsonType.ObjectId)]
		public string HomeCountryId { get; set; }

		[BsonElement("hcity")]
		[BsonIgnoreIfNull]
		[BsonRepresentation(BsonType.ObjectId)]
		public string HomeCityId { get; set; }

		[BsonIgnore]
		public string HomeCountry { get; set; }

		[BsonIgnore]
		public string HomeCity { get; set; }


		public User()
		{
			Roles = new List<string>();
		}

		public override string ToString()
		{
			return Username + " <" + Email + ">";
		}

	}
}
