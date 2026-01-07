namespace lab2_14.Entity;

public class Booking
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Time { get; set; }

    public Booking(string name, string phone, string time)
    {
        Name = name;
        Phone = phone;
        Time = time;
    }
}