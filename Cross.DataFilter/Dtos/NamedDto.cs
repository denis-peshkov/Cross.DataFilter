namespace Cross.DataFilter.Dtos;

public class NamedDto
{
    public int Id { get; set; }

    [Sort]
    public string Name { get; set; }
}

