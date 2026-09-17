namespace SpaFlow.Web.Models;

public enum AppointmentStatus
{
    Pending = 1,
    Confirmed = 2,
    CheckedIn = 3,
    InService = 4,
    Completed = 5,
    Cancelled = 6,
    NoShow = 7
}

public enum AppointmentSource
{
    Reception = 1,
    Phone = 2,
    WhatsApp = 3,
    Web = 4,
    SocialMedia = 5,
    WalkIn = 6
}

public enum PaymentStatus
{
    Pending = 1,
    Partial = 2,
    Paid = 3,
    Refunded = 4,
    Cancelled = 5
}

public enum PaymentMethod
{
    Cash = 1,
    Card = 2,
    Qr = 3,
    Transfer = 4,
    GiftCard = 5,
    Other = 6
}

public enum InventoryMovementType
{
    Purchase = 1,
    Sale = 2,
    InternalUse = 3,
    AdjustmentIn = 4,
    AdjustmentOut = 5,
    Return = 6
}
