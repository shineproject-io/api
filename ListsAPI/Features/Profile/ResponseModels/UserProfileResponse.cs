namespace ListsAPI.Features.Profile.ResponseModels
{
    public class UserProfileResponse
    {
        public int Id { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePicturePath { get; set; }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(GivenName))
                {
                    return EmailAddress;
                }

                return $"{GivenName} {FamilyName}";
            }
        }
    }
}