using Microsoft.Extensions.Configuration;
using System.IO;

namespace TadosCatFeeding.CRUDoperations
{
    public abstract class CRUD<T>
    {
        protected readonly string connectionString; 

        protected CRUD()
        {
            connectionString = GetConnection().GetSection("ConnectionStrings").GetSection("CatFeedingDB").Value;
        }

        protected IConfigurationRoot GetConnection()
            => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

        public abstract (T objectReturned, bool success, string report) Read(int id);
        public abstract (bool success, string report) Update(int id, T info);
        public abstract (bool success, string report) Delete(int id);
        public abstract (bool success, string report) Create(T info);
    }
}
