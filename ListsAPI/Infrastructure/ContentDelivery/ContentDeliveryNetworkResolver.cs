namespace ListsAPI.Infrastructure.ContentDelivery
{
    public interface IContentDeliveryNetworkResolver
    {
        string Resolve(string absoluteUri);
    }

    public class ContentDeliveryNetworkResolver : IContentDeliveryNetworkResolver
    {
        private readonly IConfigurationValueProvider _configurationValueProvider;

        public ContentDeliveryNetworkResolver(IConfigurationValueProvider configurationValueProvider)
        {
            _configurationValueProvider = configurationValueProvider;
        }

        public string Resolve(string absoluteUri)
        {
            var contentDeliveryBaseUrl = _configurationValueProvider.GetValue<string>("CDNBaseUrl");

            if (string.IsNullOrWhiteSpace(contentDeliveryBaseUrl))
            {
                return absoluteUri;
            }

            var storageBaseUrl = _configurationValueProvider.GetValue<string>("StorageBaseUrl");

            return absoluteUri.Replace(storageBaseUrl, contentDeliveryBaseUrl);
        }
    }
}