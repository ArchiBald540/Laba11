namespace Laba11;

public class Note
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}