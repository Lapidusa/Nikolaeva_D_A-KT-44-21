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
        Task<Student> GetStudentByIdAsync(int id, CancellationToken cancellationToken);
        Task<Curriculum[]> GetObjectsByGroupAsync(ObjectsByGroupFilter filter, CancellationToken cancellationToken);
        Task<Curriculum> AddCurriculumAsync(AddCurriculumRequest request, CancellationToken cancellationToken);
        Task<Curriculum> UpdateCurriculumAsync(int curriculumId, AddCurriculumRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteCurriculumAsync(int curriculumId, CancellationToken cancellationToken);
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
                    (string.IsNullOrEmpty(filter.FirstName) || (s.FirstName != null && s.FirstName.Contains(filter.FirstName))) &&
                    (string.IsNullOrEmpty(filter.LastName) || (s.LastName != null && s.LastName.Contains(filter.LastName))) &&
                    (string.IsNullOrEmpty(filter.MiddleName) || (s.MiddleName != null && s.MiddleName.Contains(filter.MiddleName))))
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
        
        public async Task<Student> GetStudentByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Students.FindAsync(new object[] { id }, cancellationToken);
        }
        
        public async Task<Curriculum[]> GetObjectsByGroupAsync(ObjectsByGroupFilter filter, CancellationToken cancellationToken)
        {
            return await _dbContext.Curriculums
                .Include(x=>x.Group)
                .Include(x=>x.Objects)
                .Where(c => c.GroupId == filter.GroupId)
                .ToArrayAsync(cancellationToken);
        }
        
        public async Task<Curriculum> AddCurriculumAsync(AddCurriculumRequest request, CancellationToken cancellationToken)
        {
            var group = await _dbContext.Groups.FindAsync(new object[] { request.GroupId }, cancellationToken);
            bool groupExists = group != null;
            
            var obj = await _dbContext.Objects.FindAsync(new object[] { request.ObjectId }, cancellationToken);
            bool objectExists = obj != null;
            
            if (!groupExists && !objectExists)
            {
                throw new Exception("Group and Object do not exist.");
            }
            else if (!groupExists)
            {
                throw new Exception($"Group with ID {request.GroupId} does not exist.");
            }
            else if (!objectExists)
            {
                throw new Exception($"Object with ID {request.ObjectId} does not exist.");
            }
            
            var curriculum = new Curriculum
            {
                GroupId = request.GroupId,
                ObjectId = request.ObjectId,
                Hours = request.Hours
            };

            await _dbContext.Curriculums.AddAsync(curriculum, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return curriculum;
        }
        
        public async Task<Curriculum> UpdateCurriculumAsync(int curriculumId, AddCurriculumRequest request, CancellationToken cancellationToken)
        {
            var curriculum = await _dbContext.Curriculums.FirstOrDefaultAsync(x=>x.CurriculumId==curriculumId, cancellationToken);
            if (curriculum == null)
            {
                throw new Exception($"Curriculum with ID {curriculumId} does not exist.");
            }
            
            var group = await _dbContext.Groups.FirstOrDefaultAsync(x=>x.GroupId==request.GroupId, cancellationToken);
            if (group == null)
            {
                throw new Exception($"Group with ID {request.GroupId} does not exist.");
            }

            var obj = await _dbContext.Objects.FirstOrDefaultAsync(x=>x.ObjectId==request.ObjectId, cancellationToken);
            if (obj == null)
            {
                throw new Exception($"Object with ID {request.ObjectId} does not exist.");
            }
            
            curriculum.GroupId = request.GroupId;
            curriculum.ObjectId = request.ObjectId;
            curriculum.Hours = request.Hours;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return curriculum;
        }
        
        public async Task<bool> DeleteCurriculumAsync(int id, CancellationToken cancellationToken)
        {
            var curriculum = await _dbContext.Curriculums.FirstOrDefaultAsync(x=>x.CurriculumId==id, cancellationToken);
            if (curriculum == null)
            {
                return false;
            }

            _dbContext.Curriculums.Remove(curriculum);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}