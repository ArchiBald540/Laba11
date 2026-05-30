namespace Laba11;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Note> Notes { get; set; } = new();
}