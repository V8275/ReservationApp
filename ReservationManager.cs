using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ReservationManager
{
    private const string DataFileName = "reservations.txt";
    public List<Reservation> Reservations { get; private set; }

    public ReservationManager()
    {
        Reservations = new List<Reservation>();
        LoadReservations();
    }

    public void AddReservation(Reservation reservation)
    {
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation));
        }
        
        if (IsTimeSlotOccupied(reservation.StartTime, reservation.EndTime))
        {
            throw new InvalidOperationException("Временной интервал уже занят другим резервом");
        }
        
        Reservations.Add(reservation);
        SaveReservations();
    }

    public void RemoveReservation(Reservation reservation)
    {
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation));
        }
        Reservations.Remove(reservation);
        SaveReservations();
    }

    public void UpdateReservationStatus(Reservation reservation, ReservationStatus newStatus)
    {
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation));
        }
        reservation.UpdateStatus(newStatus);
        SaveReservations();
    }

    private bool IsTimeSlotOccupied(DateTime startTime, DateTime endTime)
    {
        return Reservations.Any(r => 
            r.Status == ReservationStatus.Активно &&
            startTime < r.EndTime && 
            endTime > r.StartTime);
    }

    private void SaveReservations()
    {
        try
        {
            var lines = Reservations.Select(r => 
                $"{r.CustomerName}|{r.StartTime:yyyy-MM-dd HH:mm}|{r.EndTime:yyyy-MM-dd HH:mm}|{(int)r.Status}");
            File.WriteAllLines(DataFileName, lines);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при сохранении данных: {ex.Message}");
        }
    }

    private void LoadReservations()
    {
        try
        {
            if (File.Exists(DataFileName))
            {
                var lines = File.ReadAllLines(DataFileName);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 4)
                    {
                        if (DateTime.TryParse(parts[1], out DateTime startTime) && 
                            DateTime.TryParse(parts[2], out DateTime endTime) && 
                            Enum.TryParse(parts[3], out ReservationStatus status))
                        {
                            var reservation = new Reservation(parts[0], startTime, endTime);
                            reservation.Status = status;
                            Reservations.Add(reservation);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при загрузке данных: {ex.Message}");
        }
    }
}