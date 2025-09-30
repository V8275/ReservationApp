using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestReservationApp
{
    [TestClass]
    public class ReservationManagerTests
    {
        private const string TestFileName = "test_reservations.txt";
        private ReservationManager reservationManager;

        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(TestFileName))
            {
                File.Delete(TestFileName);
            }

            reservationManager = new ReservationManager();

            SetPrivateField(reservationManager, "DataFileName", TestFileName);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(TestFileName))
            {
                File.Delete(TestFileName);
            }
        }

        [TestMethod]
        public void ReservationManager_AddReservation_AddsSuccessfully()
        {
            var reservation = new Reservation("Тест Клиент",
                DateTime.Now,
                DateTime.Now.AddHours(2));

            reservationManager.AddReservation(reservation);

            Assert.AreEqual(1, reservationManager.Reservations.Count);
            Assert.AreEqual("Тест Клиент", reservationManager.Reservations[0].CustomerName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReservationManager_AddReservation_NullReservation_ThrowsException()
        {
            reservationManager.AddReservation(null);
        }

        [TestMethod]
        public void ReservationManager_RemoveReservation_RemovesSuccessfully()
        {
            var reservation = new Reservation("Тест Клиент",
                DateTime.Now,
                DateTime.Now.AddHours(2));
            reservationManager.AddReservation(reservation);

            reservationManager.RemoveReservation(reservation);

            Assert.AreEqual(0, reservationManager.Reservations.Count);
        }

        [TestMethod]
        public void ReservationManager_UpdateReservationStatus_UpdatesSuccessfully()
        {
            var reservation = new Reservation("Тест Клиент",
                DateTime.Now,
                DateTime.Now.AddHours(2));
            reservationManager.AddReservation(reservation);

            reservationManager.UpdateReservationStatus(reservation, ReservationStatus.Cancelled);

            Assert.AreEqual(ReservationStatus.Cancelled, reservation.Status);
        }

        [TestMethod]
        public void ReservationManager_SaveAndLoadReservations_WorksCorrectly()
        {
            var reservation1 = new Reservation("Клиент 1",
                new DateTime(2024, 1, 15, 10, 0, 0),
                new DateTime(2024, 1, 15, 12, 0, 0));
            var reservation2 = new Reservation("Клиент 2",
                new DateTime(2024, 1, 16, 14, 0, 0),
                new DateTime(2024, 1, 16, 16, 0, 0));

            reservationManager.AddReservation(reservation1);
            reservationManager.AddReservation(reservation2);

            var newManager = new ReservationManager();
            SetPrivateField(newManager, "DataFileName", TestFileName);

            Assert.AreEqual(2, newManager.Reservations.Count);
            Assert.AreEqual("Клиент 1", newManager.Reservations[0].CustomerName);
            Assert.AreEqual("Клиент 2", newManager.Reservations[1].CustomerName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReservationManager_AddReservation_OverlappingTime_ThrowsException()
        {
            var reservation1 = new Reservation("Клиент 1",
                new DateTime(2024, 1, 15, 10, 0, 0),
                new DateTime(2024, 1, 15, 12, 0, 0));
            var reservation2 = new Reservation("Клиент 2",
                new DateTime(2024, 1, 15, 11, 0, 0),
                new DateTime(2024, 1, 15, 13, 0, 0));

            reservationManager.AddReservation(reservation1);

            reservationManager.AddReservation(reservation2);
        }

        [TestMethod]
        public void ReservationManager_IsTimeSlotOccupied_ReturnsCorrectValue()
        {
            var reservation = new Reservation("Клиент 1",
                new DateTime(2024, 1, 15, 10, 0, 0),
                new DateTime(2024, 1, 15, 12, 0, 0));
            reservationManager.AddReservation(reservation);

            bool isOccupied = InvokePrivateMethod<bool>(reservationManager, "IsTimeSlotOccupied",
                new DateTime(2024, 1, 15, 11, 0, 0),
                new DateTime(2024, 1, 15, 13, 0, 0));

            Assert.IsTrue(isOccupied);
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        private T InvokePrivateMethod<T>(object obj, string methodName, params object[] parameters)
        {
            var method = obj.GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)method?.Invoke(obj, parameters);
        }
    }
}
