using Laba11;
using Microsoft.EntityFrameworkCore;

namespace Crud_Test;

public class UnitTest1
{
    private DataContext GetContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite($"Data Source=test_{Guid.NewGuid()}.db")
            .Options;
        return new DataContext(options);
    }

    [Fact]
    public async Task Create_ShouldAddNote()
    {
        var note = await Crud.Create("Тест");
        Assert.NotEqual(0, note.Id);
        Assert.Equal("Тест", note.Text);
    }

    [Fact]
    public async Task Read_ById_ShouldReturnNote()
    {
        var createdNote = await Crud.Create("Найти меня");
        var foundNote = await Crud.Read(createdNote.Id);
        Assert.NotNull(foundNote);
        Assert.Equal("Найти меня", foundNote?.Text);
    }

    [Fact]
    public async Task Read_ByText_ShouldReturnMatchingNotes()
    {
        await Crud.Create("Купить яблоки");
        await Crud.Create("Купить бананы");
        await Crud.Create("Сдать работу");
        
        var results = await Crud.Read("Купить");
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task ReadAll_ShouldReturnAllNotes()
    {
        await Crud.Create("Заметка 1");
        await Crud.Create("Заметка 2");
        
        var all = await Crud.ReadAll();
        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task Update_ShouldChangeText()
    {
        var note = await Crud.Create("Старый текст");
        await Crud.Update(note, "Новый текст");
        
        var updated = await Crud.Read(note.Id);
        Assert.Equal("Новый текст", updated?.Text);
    }

    [Fact]
    public async Task Delete_ShouldRemoveNote()
    {
        var note = await Crud.Create("Удалить меня");
        await Crud.Delete(note);
        
        var found = await Crud.Read(note.Id);
        Assert.Null(found);
    }
}