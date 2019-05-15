using System.ComponentModel.DataAnnotations;

namespace AdjutantApi.Data.Models
{
    public class DiscordAccount
    {
        public int Id { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "The Verification key can only consist of numbers!")]
        public string AccountId { get; set; }
        public VerificationKey BoundKey { get; set; }
    }
}