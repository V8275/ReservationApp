using System;
using System.Drawing;
using System.Windows.Forms;

public class ReservationForm : Form
{
    private ReservationManager reservationManager;
    private TextBox customerNameTextBox;
    private DateTimePicker startTimePicker;
    private DateTimePicker endTimePicker;
    private ComboBox statusComboBox;
    private Button addReservationButton;
    private Button removeReservationButton;
    private Button updateStatusButton;
    private ListBox reservationsListBox;
    private Label titleLabel;

    public ReservationForm()
    {
        InitializeComponent();
        reservationManager = new ReservationManager();
        UpdateReservationsList();
    }

    private void InitializeComponent()
    {
        this.Text = "Управление резервированием";
        this.Width = 700;
        this.Height = 500;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Font = new Font("Segoe UI", 9);

        // Заголовок
        titleLabel = new Label
        {
            Text = "Система управления резервированием",
            Location = new Point(10, 10),
            Width = 400,
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };

        // Поля ввода
        var customerLabel = new Label { Text = "Имя клиента:", Location = new Point(10, 50), Width = 100 };
        customerNameTextBox = new TextBox
        {
            Location = new Point(120, 50),
            Width = 200,
            Text = "Введите имя клиента"
        };

        var startTimeLabel = new Label { Text = "Время начала:", Location = new Point(10, 80), Width = 100 };
        startTimePicker = new DateTimePicker
        {
            Location = new Point(120, 80),
            Width = 200,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "yyyy-MM-dd HH:mm",
            ShowUpDown = true
        };

        var endTimeLabel = new Label { Text = "Время окончания:", Location = new Point(10, 110), Width = 100 };
        endTimePicker = new DateTimePicker
        {
            Location = new Point(120, 110),
            Width = 200,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "yyyy-MM-dd HH:mm",
            ShowUpDown = true
        };

        var statusLabel = new Label { Text = "Статус:", Location = new Point(10, 140), Width = 100 };
        statusComboBox = new ComboBox
        {
            Location = new Point(120, 140),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        statusComboBox.Items.AddRange(Enum.GetNames(typeof(ReservationStatus)));
        statusComboBox.SelectedIndex = 0;

        // Кнопки
        addReservationButton = new Button
        {
            Location = new Point(10, 180),
            Text = "Добавить резерв",
            Width = 120,
            BackColor = Color.LightGreen
        };
        addReservationButton.Click += AddReservationButton_Click;

        removeReservationButton = new Button
        {
            Location = new Point(140, 180),
            Text = "Удалить резерв",
            Width = 120,
            BackColor = Color.LightCoral
        };
        removeReservationButton.Click += RemoveReservationButton_Click;

        updateStatusButton = new Button
        {
            Location = new Point(270, 180),
            Text = "Обновить статус",
            Width = 120,
            BackColor = Color.LightBlue
        };
        updateStatusButton.Click += UpdateStatusButton_Click;

        // Список резервов
        var listLabel = new Label { Text = "Список резервов:", Location = new Point(10, 220), Width = 200, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
        reservationsListBox = new ListBox
        {
            Location = new Point(10, 240),
            Width = 660,
            Height = 200,
            Font = new Font("Consolas", 9)
        };

        // Добавление элементов на форму
        this.Controls.AddRange(new Control[]
        {
            titleLabel, customerLabel, customerNameTextBox, startTimeLabel, startTimePicker,
            endTimeLabel, endTimePicker, statusLabel, statusComboBox, addReservationButton,
            removeReservationButton, updateStatusButton, listLabel, reservationsListBox
        });
    }

    private void UpdateReservationsList()
    {
        reservationsListBox.Items.Clear();
        foreach (var reservation in reservationManager.Reservations)
        {
            reservationsListBox.Items.Add(reservation.ToString());
        }
    }

    private void AddReservationButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(customerNameTextBox.Text))
        {
            MessageBox.Show("Введите имя клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DateTime startTime = startTimePicker.Value;
        DateTime endTime = endTimePicker.Value;
        
        if (startTime >= endTime)
        {
            MessageBox.Show("Время начала должно быть раньше времени окончания!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Reservation newReservation = new Reservation(customerNameTextBox.Text.Trim(), startTime, endTime);
            reservationManager.AddReservation(newReservation);
            customerNameTextBox.Clear();
            UpdateReservationsList();
            MessageBox.Show("Резерв успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при добавлении резерва: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RemoveReservationButton_Click(object sender, EventArgs e)
    {
        if (reservationsListBox.SelectedIndex == -1)
        {
            MessageBox.Show("Выберите резерв для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedReservation = GetSelectedReservation();
        if (selectedReservation != null)
        {
            var result = MessageBox.Show($"Вы уверены, что хотите удалить резерв для {selectedReservation.CustomerName}?", 
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    reservationManager.RemoveReservation(selectedReservation);
                    UpdateReservationsList();
                    MessageBox.Show("Резерв успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении резерва: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void UpdateStatusButton_Click(object sender, EventArgs e)
    {
        if (reservationsListBox.SelectedIndex == -1)
        {
            MessageBox.Show("Выберите резерв для обновления статуса!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedReservation = GetSelectedReservation();
        if (selectedReservation != null)
        {
            try
            {
                ReservationStatus newStatus = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), statusComboBox.SelectedItem.ToString());
                reservationManager.UpdateReservationStatus(selectedReservation, newStatus);
                UpdateReservationsList();
                MessageBox.Show("Статус резерва успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private Reservation GetSelectedReservation()
    {
        if (reservationsListBox.SelectedIndex == -1) return null;

        int selectedIndex = reservationsListBox.SelectedIndex;
        if (selectedIndex >= 0 && selectedIndex < reservationManager.Reservations.Count)
        {
            return reservationManager.Reservations[selectedIndex];
        }
        return null;
    }
}