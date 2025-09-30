using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestReservationApp
{

    [TestClass]
    public class ReservationFormTests
    {
        [TestMethod]
        public void ReservationForm_Constructor_InitializesCorrectly()
        {
            // Act
            var form = new ReservationForm();

            // Assert
            Assert.IsNotNull(form);
            Assert.AreEqual("Управление резервированием", form.Text);
            Assert.IsTrue(form.Width > 0);
            Assert.IsTrue(form.Height > 0);
        }

        [TestMethod]
        public void ReservationForm_Controls_AreCreated()
        {
            // Arrange
            var form = new ReservationForm();

            // Act & Assert
            Assert.IsTrue(HasControlOfType<TextBox>(form));
            Assert.IsTrue(HasControlOfType<DateTimePicker>(form));
            Assert.IsTrue(HasControlOfType<ComboBox>(form));
            Assert.IsTrue(HasControlOfType<Button>(form));
            Assert.IsTrue(HasControlOfType<ListBox>(form));
        }

        [TestMethod]
        public void ReservationForm_AddReservation_ValidData()
        {
            // Arrange
            var form = new ReservationForm();
            var reservationManager = new ReservationManager();

            SetPrivateField(form, "reservationManager", reservationManager);

            SetPrivateField(form, "customerNameTextBox", new TextBox { Text = "Тестовый Клиент" });
            SetPrivateField(form, "startTimePicker", new DateTimePicker { Value = DateTime.Now });
            SetPrivateField(form, "endTimePicker", new DateTimePicker { Value = DateTime.Now.AddHours(2) });

            // Act
            InvokePrivateMethod(form, "AddReservationButton_Click", null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(1, reservationManager.Reservations.Count);
        }

        [TestMethod]
        public void ReservationForm_UpdateReservationsList_WorksCorrectly()
        {
            // Arrange
            var form = new ReservationForm();
            var reservationManager = new ReservationManager();
            var listBox = new ListBox();

            SetPrivateField(form, "reservationManager", reservationManager);
            SetPrivateField(form, "reservationsListBox", listBox);

            reservationManager.AddReservation(new Reservation("Тест",
                DateTime.Now,
                DateTime.Now.AddHours(2)));

            // Act
            InvokePrivateMethod(form, "UpdateReservationsList");

            // Assert
            Assert.AreEqual(1, listBox.Items.Count);
        }

        [TestMethod]
        public void ReservationForm_GetSelectedReservation_ReturnsCorrectReservation()
        {
            // Arrange
            var form = new ReservationForm();
            var reservationManager = new ReservationManager();
            var listBox = new ListBox();

            var reservation = new Reservation("Тест Клиент",
                DateTime.Now,
                DateTime.Now.AddHours(2));
            reservationManager.AddReservation(reservation);

            SetPrivateField(form, "reservationManager", reservationManager);
            SetPrivateField(form, "reservationsListBox", listBox);

            // Обновить список
            InvokePrivateMethod(form, "UpdateReservationsList");
            listBox.SelectedIndex = 0;

            // Act
            var selected = InvokePrivateMethod<Reservation>(form, "GetSelectedReservation");

            // Assert
            Assert.IsNotNull(selected);
            Assert.AreEqual("Тест Клиент", selected.CustomerName);
        }

        private bool HasControlOfType<T>(Control parent) where T : Control
        {
            foreach (Control control in parent.Controls)
            {
                if (control is T) return true;
                if (HasControlOfType<T>(control)) return true;
            }
            return false;
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        private void InvokePrivateMethod(object obj, string methodName, params object[] parameters)
        {
            var method = obj.GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(obj, parameters);
        }

        private T InvokePrivateMethod<T>(object obj, string methodName, params object[] parameters)
        {
            var method = obj.GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)method?.Invoke(obj, parameters);
        }
    }
}
