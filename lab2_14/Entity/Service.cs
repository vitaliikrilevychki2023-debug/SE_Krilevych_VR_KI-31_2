namespace lab2_14.Entity;

public class Service
{
    public static int initID {get; set;}
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public Service(string name, int price)
    {
        initID += 1;
        Id = initID++;
        Name = name;
        Price = price;
    }
}