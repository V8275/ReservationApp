using System;
using System.Windows.Forms;

namespace ReservationApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ReservationForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка при запуске приложения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
