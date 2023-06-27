using System.Text.Json.Serialization;

namespace PontoCalculator.Models
{
    public class User
    {

        public String Email { get; set; }
        [JsonIgnore] public String Password { get; set; }
        public int Id { get; set; }
        public String Name { get; set; }
        public bool Active { get; set; }
        public bool Staff { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
    }
}
