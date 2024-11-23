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
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Уникальное имя базы данных для каждого теста
            .Options;
    }
    
    [Fact]
    public async Task GetStudentsByGroupAsync_KT3120_TwoObjects()
    {
        var ctx = new StudentDbContext(_dbContextOptions);
        var studentService = new StudentService(ctx);
        
        var groups = new List<Group>
        {
            new Group { GroupId = 1, GroupName = "KT-31-20" }, // Указываем GroupId
            new Group { GroupId = 2, GroupName = "KT-41-20" }  // Указываем GroupId
        };
        await ctx.Set<Group>().AddRangeAsync(groups);
        await ctx.SaveChangesAsync();
        
        var students = new List<Student>
        {
            new Student
            {
                FirstName = "qwerty",
                LastName = "asdf",
                MiddleName = "zxc",
                GroupId = 1,
            },
            new Student
            {
                FirstName = "qwerty2",
                LastName = "asdf2",
                MiddleName = "zxc2",
                GroupId = 1
            },
            new Student
            {
                FirstName = "qwerty3",
                LastName = "asdf3",
                MiddleName = "zxc3",
                GroupId = 2,
            },
        };
        
        await ctx.Set<Student>().AddRangeAsync(students);
        await ctx.SaveChangesAsync();
        
        var filter = new Filters.StudentFilters.StudentGroupFilter
        {
            GroupName = "KT-31-20"
        };
        var studentsResult = await studentService.GetStudentsByGroupAsync(filter, CancellationToken.None);

        Assert.Equal(2, studentsResult.Length);
    }
    
    [Fact]
    public async Task GetObjectsByGroupAsync_GroupWithThreeObjects_ReturnsThreeObjects()
    
    {
        // Arrange
        var ctx = new StudentDbContext(_dbContextOptions);
        var curriculumService = new StudentService(ctx);

        // Создаем группы
        var group1 = new Group { GroupId = 1, GroupName = "KT-31-20" };
        var group2 = new Group { GroupId = 2, GroupName = "KT-32-20" };
        await ctx.Set<Group>().AddRangeAsync(new[] { group1, group2 });
        await ctx.SaveChangesAsync();

        // Создаем предметы
        var curriculums = new List<Curriculum>
        {
            new Curriculum { CurriculumId = 1, GroupId = 1, ObjectId = 1, Objects = new Objects { ObjectId = 1, Name = "Math" }, Hours = 5 },
            new Curriculum { CurriculumId = 2, GroupId = 1, ObjectId = 2, Objects = new Objects { ObjectId = 2, Name = "Physics" }, Hours = 4 },
            new Curriculum { CurriculumId = 3, GroupId = 1, ObjectId = 3, Objects = new Objects { ObjectId = 3, Name = "Chemistry" }, Hours = 3 },
            new Curriculum { CurriculumId = 4, GroupId = 2, ObjectId = 4, Objects = new Objects { ObjectId = 4, Name = "Biology" }, Hours = 2 } // Этот предмет не должен быть возвращен
        };

        await ctx.Set<Curriculum>().AddRangeAsync(curriculums);
        await ctx.SaveChangesAsync();

        // Act
        var filter = new Filters.StudentFilters.ObjectsByGroupFilter{GroupId = 1};
        var result = await curriculumService.GetObjectsByGroupAsync(filter, CancellationToken.None);

        Assert.Equal(3, result.Length);
    }
    
    [Fact]
    public async Task GetStudentsByFIOAsync_FilterByLastName_ReturnsMatchingStudents()
    {
        // Arrange
        var ctx = new StudentDbContext(_dbContextOptions);
        var studentService = new StudentService(ctx);
        
        var groups = new List<Group>
        {
            new Group { GroupId = 1, GroupName = "KT-42-21" },
            new Group { GroupId = 2, GroupName = "KT-44-21" }
        };
        
        await ctx.Set<Group>().AddRangeAsync(groups);
        await ctx.SaveChangesAsync();

        var students = new List<Student>
        {
            new Student { StudentId = 1, FirstName = "John", LastName = "Doe", MiddleName = "A.", GroupId = 1 },
            new Student { StudentId = 2, FirstName = "Jane", LastName = "Doe", MiddleName = "B.", GroupId = 2 },
            new Student { StudentId = 3, FirstName = "Jim", LastName = "Doe", MiddleName = "C.", GroupId = 1 },
            new Student { StudentId = 4, FirstName = "Alice", LastName = "Smith", MiddleName = "D.", GroupId = 1 }
        };
        
        await ctx.Set<Student>().AddRangeAsync(students);
        await ctx.SaveChangesAsync();
        
        var filter = new Filters.StudentFilters.StudentFIOFilter { LastName = "Doe" };
        var result = await studentService.GetStudentsByFIOAsync(filter, CancellationToken.None);
        
        Assert.Equal(3, result.Length);
        Assert.All(result, s => Assert.Equal("Doe", s.LastName));
    }
}
