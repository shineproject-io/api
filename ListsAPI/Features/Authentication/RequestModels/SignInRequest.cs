namespace ListsAPI.Features.Authentication.RequestModels
{
    public class SignInRequest
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}