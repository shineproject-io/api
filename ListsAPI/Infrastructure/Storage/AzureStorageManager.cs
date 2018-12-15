using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;
using System.Threading.Tasks;

namespace ListsAPI.Infrastructure.Storage
{
    public interface IAzureStorageManager
    {
        Task<string> StoreFile(string containerName, string fileName, IFormFile uploadFile);

        Task DeleteFile(string containerName, string fileName);
    }

    public class AzureStorageManager : IAzureStorageManager
    {
        private readonly IConfigurationValueProvider _configurationValueProvider;

        public AzureStorageManager(IConfigurationValueProvider configurationValueProvider)
        {
            _configurationValueProvider = configurationValueProvider;
        }

        private CloudStorageAccount ConnectStorageAccount()
        {
            CloudStorageAccount storageAccount = null;

            string storageConnectionString = _configurationValueProvider.GetValue<string>("StorageConnectionString");

            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                return storageAccount;
            }
            else
            {
                throw new WebException("Unable to connect to storage account");
            }
        }

        private CloudBlobContainer FetchBlobContainer(CloudStorageAccount storageAccount, string containerName)
        {
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            return cloudBlobClient.GetContainerReference(containerName);
        }

        public async Task<string> StoreFile(string containerName, string fileName, IFormFile uploadFile)
        {
            var storageAccount = ConnectStorageAccount();
            var cloudBlobContainer = FetchBlobContainer(storageAccount, containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            await cloudBlobContainer.SetPermissionsAsync(permissions);

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

            using (var fileStream = uploadFile.OpenReadStream())
            {
                await cloudBlockBlob.UploadFromStreamAsync(fileStream);
            }

            return cloudBlobContainer.Uri.AbsoluteUri + "/" + fileName;
        }

        public async Task DeleteFile(string containerName, string fileName)
        {
            var storageAccount = ConnectStorageAccount();
            var cloudBlobContainer = FetchBlobContainer(storageAccount, containerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

            if (cloudBlobContainer != null)
            {
                await cloudBlockBlob.DeleteIfExistsAsync();
            }
        }
    }
}