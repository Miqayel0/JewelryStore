namespace App.Domain.Enums;

public enum OrderStatus : byte
{
    Pending = 1,
    Started,
    Ready,
    Completed,
}
