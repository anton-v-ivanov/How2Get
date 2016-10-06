using System.Text;

namespace HowToGet.Web.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }

	    public string Email { get; set; }

        public string Password { get; set; }

	    public string Referrer { get; set; }

		//public string InviteCode { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("UserName: {0}", UserName));
			sb.AppendLine(string.Format("Email: {0}", Email));
			sb.AppendLine(string.Format("Referrer: {0}", Referrer));
			//sb.AppendLine(string.Format("InviteCode: {0}", InviteCode));
			return sb.ToString();
		}
    }
}