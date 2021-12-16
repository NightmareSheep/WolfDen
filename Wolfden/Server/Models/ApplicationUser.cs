using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wolfden.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Guest { get; set; }

        /// <summary>
        /// Guests have a guid behind their name, this removes the guid and replaces it with a guest tag.
        /// </summary>
        public string DisplayName { get { return !Guest ? UserName : "Guest " + UserName[0..^36]; } }
        public DateTimeOffset LastLoginDate { get; set; }
    }
}
