using Arinco.BicepHub.App.Core.Utilities;

namespace Arinco.BicepHub.App.Core.Models
{
    public class User
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public CustomTokenCredential AzureAccessTokenCredential { get; set; }
    }
}
