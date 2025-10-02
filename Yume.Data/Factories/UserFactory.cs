using Bogus;
using Yume.Data.Entities.User;
using Yume.Enums;

namespace Yume.Data.Factories
{
    public class UserFactory
    {
        private readonly Faker<User> _userFaker;

        public UserFactory()
        {
            _userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Email, f => f.Internet.Email().ToLower())
                //.RuleFor(u => u.EmailVerified, f => f.Random.Bool())
                .RuleFor(u => u.Username, f => f.Internet.UserName().ToLower())
                .RuleFor(u => u.Nickname, f => f.Random.Number(0, 10) > 8 ? f.Name.FirstName() : null)
                .RuleFor(u => u.Discriminator, f => f.Random.Number(1, 9999).ToString("D4"))
                //.RuleFor(u => u.About, f => null)
                //.RuleFor(u => u.PasswordHash, f => BCrypt.Net.BCrypt.HashPassword(f.Random.AlphaNumeric(f.Random.Number(8, 32))))
                .RuleFor(u => u.CreatedDate, f => f.Date.Past())
                .RuleFor(u => u.UpdatedDate, f => f.Date.Recent())
                .RuleFor(u => u.IsActive, f => true);
                //.RuleFor(u => u.MfaTotpEnabled, f => false) //f.Random.Bool())
                //.RuleFor(u => u.MfaBackupEnabled, f => false) //f.Random.Bool())
                //.RuleFor(u => u.Theme, f => f.PickRandom<UserThemeEnum>());

            // Configure relationships if needed
            ConfigureRelationships();
        }

        private void ConfigureRelationships()
        {
            //_userFaker.RuleFor(u => u.MfaTotp, f => new UserMfaTotp
            //{
            //    Id = Guid.NewGuid(),
            //    SecretHash = f.Random.AlphaNumeric(20),
            //    CreatedDate = f.Date.Past(),
            //});

            //_userFaker.RuleFor(u => u.MfaBackup, f => new List<UserMfaBackup>
            //{
            //    new UserMfaBackup
            //    {
            //        Id = Guid.NewGuid(),
            //        CodeHash = f.Random.AlphaNumeric(20),
            //        CreatedDate = f.Date.Past(),
            //    }
            //});

            //_userFaker.RuleFor(u => u.LoginHistory, f => new List<UserLoginHistory>
            //{
            //    new UserLoginHistory
            //    {
            //        Id = Guid.NewGuid(),
            //        Timestamp = f.Date.Past(),
            //        Success = f.Random.Bool(),
            //        Reason = f.Lorem.Sentence(),
            //        IpAddress = f.Internet.IPv4(),
            //        Agent = f.Internet.UserAgent(),
            //        Device = f.Random.Word(),
            //        Os = f.System.OperatingSystem,
            //    }
            //});
        }

        public User GenerateUser()
        {
            return _userFaker.Generate();
        }

        public List<User> GenerateUsers(int count)
        {
            return _userFaker.Generate(count);
        }
    }
}
