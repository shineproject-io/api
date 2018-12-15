namespace ListsAPI.Features.Profile.RequestModels
{
    public class CreateUserProfileRequest
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}