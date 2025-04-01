using ACRViewer.BlazorServer.Configuration;
using ACRViewer.BlazorServer.Managers;

namespace ACRViewer.BlazorServer.Models
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
