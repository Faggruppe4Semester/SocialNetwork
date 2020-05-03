using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Areas.Database
{
    public class SocialNetworkDatabaseSettings : ISocialNetworkDatabaseSettings
    {
        public string UserCollectionName { get; set; }
        public string PostCollectionName { get; set; }
        public string CircleCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISocialNetworkDatabaseSettings
    {
        string PostCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string CircleCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
