using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Users
{
	public class UserRole
	{
		public UserRole()
		{
		}

		public UserRole(string role)
		{
			RoleName = role;
		}

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		
		[BsonElement("role")]
		public string RoleName { get; set; }
	}
}