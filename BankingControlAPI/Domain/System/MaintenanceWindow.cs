using BankingControlAPI.Domain.Enums;

namespace BankingControlAPI.Domain.System
{
    public class MaintenanceWindow(MaintenanceStatus _statusFunc)
    {
        public MaintenanceStatus Status { get; set; } = _statusFunc;

        public int RetryAfterInSeconds { get; set; } = 3600;
    }
}
