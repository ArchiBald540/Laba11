using Microsoft.EntityFrameworkCore;

namespace Laba11;

public class Crud
{
    public static async Task<Note> CreateNote(string text, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var note = new Note
        {
            Text = text,
            CreatedAt = DateTimeOffset.UtcNow
        };
        db.Notes.Add(note);
        await db.SaveChangesAsync(ct);
        return note;
    }

    public static async Task<List<Note>> ReadNotes(string search, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        return await db.Notes
            .Where(x => EF.Functions.Like(x.Text, $"%{search}%"))
            .ToListAsync(ct);
    }

    public static async Task<Note?> ReadNoteById(int id, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        return await db.Notes.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public static async Task<List<Note>> ReadAllNotes(CancellationToken ct = default)
    {
        await using var db = new DataContext();
        return await db.Notes.ToListAsync(ct);
    }

    public static async Task UpdateNote(Note note, string newText, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        note.Text = newText;
        db.Notes.Update(note);
        await db.SaveChangesAsync(ct);
    }

    public static async Task DeleteNote(Note note, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        db.Notes.Remove(note);
        await db.SaveChangesAsync(ct);
    }

    public static async Task<bool> DeleteNoteById(int id, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var note = await db.Notes.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (note == null) return false;
        db.Notes.Remove(note);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public static async Task<User> CreateUser(string name, string email, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var user = new User
        {
            Name = name,
            Email = email
        };
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return user;
    }

    public static async Task<List<User>> GetAllUsers(CancellationToken ct = default)
    {
        await using var db = new DataContext();
        return await db.Users.ToListAsync(ct);
    }

    public static async Task<User?> GetUserById(int id, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        return await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public static async Task<List<User>> SearchUsersByName(string search, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        return await db.Users
            .Where(u => EF.Functions.Like(u.Name, $"%{search}%"))
            .ToListAsync(ct);
    }

    public static async Task UpdateUser(User user, string newName, string newEmail, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        user.Name = newName;
        user.Email = newEmail;
        db.Users.Update(user);
        await db.SaveChangesAsync(ct);
    }

    public static async Task DeleteUser(User user, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        db.Users.Remove(user);
        await db.SaveChangesAsync(ct);
    }

    public static async Task<bool> DeleteUserById(int id, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user == null) return false;
        db.Users.Remove(user);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public static async Task<Note> CreateNoteForUser(int userId, string text, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user == null)
            throw new InvalidOperationException($"Пользователь с ID {userId} не найден");
        
        var note = new Note
        {
            Text = text,
            CreatedAt = DateTimeOffset.UtcNow,
            UserId = userId
        };
        db.Notes.Add(note);
        await db.SaveChangesAsync(ct);
        return note;
    }

    public static async Task<List<Note>> GetNotesByUserId(int userId, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        return await db.Notes
            .Where(n => n.UserId == userId)
            .Include(n => n.User)
            .ToListAsync(ct);
    }

    public static async Task<User?> GetUserWithNotes(int userId, CancellationToken ct = default)
    {
        await using var db = new DataContext();
        return await db.Users
            .Include(u => u.Notes)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
    }
}