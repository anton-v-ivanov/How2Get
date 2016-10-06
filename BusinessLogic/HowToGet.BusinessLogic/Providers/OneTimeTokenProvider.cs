using System;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Security;
using HowToGet.Repository.Interfaces;
using HowToGet.Security.Providers;

namespace HowToGet.BusinessLogic.Providers
{
	public class OneTimeTokenProvider : IOneTimeTokenProvider
	{
		private readonly IOneTimeTokenRepository _repository;

		public OneTimeTokenProvider(IOneTimeTokenRepository repository)
		{
			_repository = repository;
		}

		public void MarkAsUsed(string token)
		{
			_repository.MarkAsUsed(token);
		}

		public string Generate(string userId)
		{
			var token = Guid.NewGuid().ToString("N");
			_repository.Save(new OneTimeToken(userId, token));
			return token;
		}

		public string Exchange(string token)
		{
			var oneTimeToken = _repository.Check(token);
			if (oneTimeToken != null)
			{
				var result = TokenProvider.GenerateAuthToken(oneTimeToken.UserId);
				_repository.MarkAsUsed(token);
				return result;
			}
			throw new ObjectNotFoundException(string.Format("Token {0} is not valid", token));
		}

	}
}