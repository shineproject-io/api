using Microsoft.Extensions.Configuration;
using System;

namespace ListsAPI.Infrastructure
{
    public interface IConfigurationValueProvider
    {
        /// <summary>
        /// Retrieves value from configuration store matching the key provided
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if value cannot be cast to requested type</exception>
        T GetValue<T>(string key);
    }

    public class ConfigurationValueProvider : IConfigurationValueProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigurationValueProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }
    }
}