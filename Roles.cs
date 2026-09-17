namespace SpaFlow.Web.Security;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Receptionist = "Receptionist";
    public const string Therapist = "Therapist";
    public const string Cashier = "Cashier";

    public static readonly string[] All =
    [
        SuperAdmin, Admin, Manager, Receptionist, Therapist, Cashier
    ];
}
