using System;
using OptimizingLastMile.Entites.Enums;

namespace OptimizingLastMile.Entites;

public class NotificationLog
{
    public Guid Id { get; set; }
    public NotificationTypeEnum NotificationType { get; set; }

    public Guid? OrderId { get; set; }

    public long? DriverId { get; set; }
    public long? CustomerId { get; set; }
    public long? ManagerId { get; set; }
}

