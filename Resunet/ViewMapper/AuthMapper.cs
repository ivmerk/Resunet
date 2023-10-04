using System;
using Resunet.ViewModels;
using Resunet.DAL.Models;

namespace Resunet.ViewMapper
{
	public class AuthMapper
	{
        public static UserModel MapRegistrationViewModelToUserModel(RegisterViewModel model)
		{
			return new UserModel()
			{
				Email = model.Email!,
				Password = model.Password!
			};
		}
	}
}

