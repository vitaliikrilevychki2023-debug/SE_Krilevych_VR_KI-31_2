namespace lab3_14.api.Models;

public class Appointment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
    public string Phone { get; set; }
    public int ServiceId { get; set; }
}