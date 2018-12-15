namespace ListsAPI.Features.Profile.RequestModels
{
    public class ResetPasswordRequest
    {
        public string EmailAddress { get; set; }

        public string Token { get; set; }

        public string Password { get; set; }
    }
}