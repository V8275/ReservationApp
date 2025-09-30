using System;

public class Reservation
{
    public string CustomerName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; }

    public Reservation(string customerName, DateTime startTime, DateTime endTime)
    {
        CustomerName = customerName;
        StartTime = startTime;
        EndTime = endTime;
        Status = ReservationStatus.Active;
    }

    public void UpdateStatus(ReservationStatus newStatus)
    {
        Status = newStatus;
    }

    public override string ToString()
    {
        return $"{CustomerName} - {StartTime:yyyy-MM-dd HH:mm} до {EndTime:yyyy-MM-dd HH:mm} - {Status}";
    }
}