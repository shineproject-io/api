namespace ListsAPI.Infrastructure.Authorisers.Models
{
    public class AuthorisationResponse<T> where T : class
    {
        public bool AuthorisationResult { get; set; }
        public T ResponseObject { get; set; }
    }
}