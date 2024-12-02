using ATM.Core.Domain.Users;
using System;

namespace ATM.Core.Domain
{
    public static class Seeds
    {
        public static class SUsers
        {
            public static class Users
            {
                public static User Administrator = new User
                {
                    Id = Guid.Parse("89F2B01E-D8A8-EC11-83C7-ECB1D797E5CD"),
                    Name = "Test",
                    Username = "Test@gmail.com",
                    Role = UserRole.Administrator,
                    IsActive= true,
                    IsDeleted=false,
                    CreatedOnUtc = DateTime.Now,
                    PasswordCipher = "Test@123",
                    Password = "3C7959E8355F19CB6C7A023E46099E5EA9EF23CC4C75675D153B366289FA1D1DF18134229825B75064C6A4E86D97E3FA6EBAAED2C1DA8C93500024C3C3F4FFD4",
                    CreatedById = Guid.Parse("89F2B01E-D8A8-EC11-83C7-ECB1D797E5CD"),
                };

                /// <summary>
                /// This user should be used when no one is logged in.
                /// </summary>
                public static User Guest = new User
                {
                    Id = Guid.Parse("E73BBD5B-3270-4DB7-B343-465F5E99D78D"),
                    Name = "xhesjana",
                    Username = "xhesjana.stambolliu@gmail.com",
                    Role = UserRole.User,
                    Password = "A",
                    CreatedById = Users.Administrator.Id,
                };
            }

        }
    }
}
