using project.Database;
using project.Interfaces.StudentsInterfaces;
using project.Models;
using Microsoft.EntityFrameworkCore;
namespace project.Tests;

public class StudentIntegrationTests
{
    public readonly DbContextOptions<StudentDbContext> _dbContextOptions;

    public StudentIntegrationTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
            .UseInMemoryDatabase(databaseName: "student_db")
            .Options;
    }

    [Fact]
    public async Task GetStudentsByGroupAsync_KT3120_TwoObjects()
    {
        // Arrange
        var ctx = new StudentDbContext(_dbContextOptions);
        var studentService = new StudentService(ctx);
    
        // Создаем группы и сохраняем их
        var groups = new List<Group>
        {
            new Group { GroupName = "KT-31-20" },
            new Group { GroupName = "KT-41-20" }
        };
        await ctx.Set<Group>().AddRangeAsync(groups);
        await ctx.SaveChangesAsync(); // Сохраняем изменения, чтобы получить GroupId

        // Получаем GroupId для "KT-31-20"
        var kt3120Group = await ctx.Groups.FirstOrDefaultAsync(g => g.GroupName == "KT-31-20");

        // Создаем студентов с правильным GroupId для "KT-31-20"
        var students = new List<Student>
        {
            new Student
            {
                FirstName = "qwerty",
                LastName = "asdf",
                MiddleName = "zxc",
                GroupId = kt3120Group.GroupId, // Используем правильный GroupId
            },
            new Student
            {
                FirstName = "qwerty2",
                LastName = "asdf2",
                MiddleName = "zxc2",
                GroupId = kt3120Group.GroupId, // Используем правильный GroupId
            },
            new Student
            {
                FirstName = "qwerty3",
                LastName = "asdf3",
                MiddleName = "zxc3",
                GroupId = 2, // Этот студент не будет учитываться в тесте
            }
        };
    
        // Добавляем студентов в контекст
        await ctx.Set<Student>().AddRangeAsync(students);
        await ctx.SaveChangesAsync();

        // Act
        var filter = new Filters.StudentFilters.StudentGroupFilter
        {
            GroupName = "KT-31-20"
        };
        var studentsResult = await studentService.GetStudentsByGroupAsync(filter, CancellationToken.None);

        // Assert
        Assert.Equal(2, studentsResult.Length); // Теперь это должно вернуть 2
    }
}