using System;
using System.ComponentModel.DataAnnotations;

namespace Resunet.DAL.Models
{
	public class UserModel
	{
		[Key]
		public int? UserId { get; set; }
		public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Salt { get; set; } = null!;
        //       FirstName varchar(50),
        //LastName varchar(50),
        public int Status { get; set; } = 0;
    }
}

