using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestReservationApp
{
    [TestClass]
    public class ReservationTests
    {
        [TestMethod]
        public void Reservation_Constructor_InitializesPropertiesCorrectly()
        {
            string customerName = "Иван Иванов";
            DateTime startTime = new DateTime(2024, 1, 15, 10, 0, 0);
            DateTime endTime = new DateTime(2024, 1, 15, 12, 0, 0);

            var reservation = new Reservation(customerName, startTime, endTime);

            Assert.AreEqual(customerName, reservation.CustomerName);
            Assert.AreEqual(startTime, reservation.StartTime);
            Assert.AreEqual(endTime, reservation.EndTime);
            Assert.AreEqual(ReservationStatus.Active, reservation.Status);
        }

        [TestMethod]
        public void Reservation_UpdateStatus_ChangesStatusCorrectly()
        {
            var reservation = new Reservation("Тест Клиент", DateTime.Now, DateTime.Now.AddHours(2));

            reservation.UpdateStatus(ReservationStatus.Completed);

            Assert.AreEqual(ReservationStatus.Completed, reservation.Status);
        }

        [TestMethod]
        public void Reservation_ToString_ReturnsCorrectFormat()
        {
            var reservation = new Reservation("Тест",
                new DateTime(2024, 1, 15, 10, 0, 0),
                new DateTime(2024, 1, 15, 12, 0, 0));

            string result = reservation.ToString();

            StringAssert.Contains(result, "Тест");
            StringAssert.Contains(result, "2024-01-15 10:00");
            StringAssert.Contains(result, "2024-01-15 12:00");
            StringAssert.Contains(result, "Активно");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Reservation_Constructor_InvalidTime_ThrowsException()
        {
            DateTime startTime = new DateTime(2024, 1, 15, 12, 0, 0);
            DateTime endTime = new DateTime(2024, 1, 15, 10, 0, 0);

            var reservation = new Reservation("Тест", startTime, endTime);
        }
    }
}
