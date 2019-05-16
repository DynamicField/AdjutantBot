using AdjutantApi.Data.Models;

namespace AdjutantApi.ViewModels
{
    public class KeyConsumptionUpdate
    {
        public string KeyValue { get; set; }
        public KeyConsumptionState NewState { get; set; }
    }
}