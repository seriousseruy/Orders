using System.Collections.Generic;

namespace Orders.ViewModels
{
    public class UserProfileViewModel
    {
        public string FirstName { get; }

        public string LastName { get; }

        public string AvatarUri { get; }

        public string Email { get; }

        public IEnumerable<string> Roles { get; }

        public IEnumerable<string> Orders { get; }

        public UserProfileViewModel(string firstName, string lastName, string avatarUri, string email,
            IEnumerable<string> roles, IEnumerable<string> orders)
        {
            FirstName = firstName;
            LastName = lastName;
            AvatarUri = avatarUri;
            Email = email;
            Roles = roles;
            Orders = orders;
        }
    }
}