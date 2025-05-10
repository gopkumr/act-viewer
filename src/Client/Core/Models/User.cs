using Arinco.BicepHub.App.Core.Utilities;

namespace Arinco.BicepHub.App.Core.Models
{
    public class User
    {
        public required string Name { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public CustomTokenCredential? AzureAccessTokenCredential { get; set; }
    }
}
