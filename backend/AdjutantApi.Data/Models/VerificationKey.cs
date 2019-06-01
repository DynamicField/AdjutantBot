using System.ComponentModel.DataAnnotations;

namespace AdjutantApi.Data.Models
{
    public class VerificationKey
    {
        public int Id { get; set; }
        [Required]
        public string KeyValue { get; set; }
        public KeyConsumptionState ConsumptionState { get; set; }
        public string DisplayName { get; set; }
    }
}

