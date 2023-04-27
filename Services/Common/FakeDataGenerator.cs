using Bogus;
using Models.DbModels;

namespace Services.Common
{
    public static class FakeDataGenerator
    {
        public static Faker<User> CreateFakeUser()
        {
            var generator = new Faker<User>("ru")
                .StrictMode(true)
                .RuleFor(x => x.Id, y => y.Random.Uuid())
                .RuleFor(x => x.Name, y => y.Name.FirstName());
            return generator;
        }
    }
}
