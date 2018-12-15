namespace ListsAPI.Features.Profile.RequestModels
{
    public class ChangeUserProfileNameRequest
    {
        public string GivenName { get; set; }

        public string FamilyName { get; set; }
    }
}