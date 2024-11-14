using Microsoft.EntityFrameworkCore;
using project.Database;
using project.Filters.StudentFilters;
using project.Models;

namespace project.Interfaces.StudentsInterfaces
{
    public interface IStudentService
    {
        Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken);
        Task<Student[]> GetStudentsByFIOAsync(StudentFIOFilter filter, CancellationToken cancellationToken);

        Task<Student> AddStudentAsync(AddStudentRequest request, CancellationToken cancellationToken);
        Task<Student> UpdateStudentAsync(int id, AddStudentRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteStudentAsync(int id, CancellationToken cancellationToken);
    }

    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _dbContext;

        public StudentService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<Student>()
                .Where(w => w.Group.GroupName == filter.GroupName)
                .ToArrayAsync(cancellationToken);
        }

        public Task<Student[]> GetStudentsByFIOAsync(StudentFIOFilter filter, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<Student>()
                .Where(s =>
                    (s.FirstName == null || s.FirstName.Contains(filter.FirstName)) &&
                    (s.LastName == null || s.LastName.Contains(filter.LastName)) &&
                    (s.MiddleName == null || s.MiddleName.Contains(filter.MiddleName)))
                .ToArrayAsync(cancellationToken);
        }

        public async Task<Student> AddStudentAsync(AddStudentRequest request, CancellationToken cancellationToken)
        {
            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                GroupId = request.GroupId
            };

            await _dbContext.Students.AddAsync(student, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return student;
        }

        public async Task<Student> UpdateStudentAsync(int id, AddStudentRequest request, CancellationToken cancellationToken)
        {
            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return null; 
            }

            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.MiddleName = request.MiddleName;
            student.GroupId = request.GroupId;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id, CancellationToken cancellationToken)
        {
            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}