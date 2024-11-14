using Microsoft.AspNetCore.Mvc;
using project.Filters.StudentFilters;
using project.Interfaces.StudentsInterfaces;

namespace project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;

        public StudentController(ILogger<StudentController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [HttpPost("GetStudentsByGroup")]
        public async Task<IActionResult> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken = default)
        {
            var students = await _studentService.GetStudentsByGroupAsync(filter, cancellationToken);
            return Ok(students);
        }

        [HttpPost("GetStudentsByFIO")]
        public async Task<IActionResult> GetStudentsByFIOAsync(StudentFIOFilter filter, CancellationToken cancellationToken = default)
        {
            var students = await _studentService.GetStudentsByFIOAsync(filter, cancellationToken);
            return Ok(students);
        }

        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                return BadRequest("Invalid student data.");
            }

            var student = await _studentService.AddStudentAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetStudentsByFIOAsync), new { id = student.StudentId }, student);
        }

        [HttpPost("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudentAsync(int id, [FromBody] AddStudentRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                return BadRequest("Invalid student data.");
            }

            var student = await _studentService.UpdateStudentAsync(id, request, cancellationToken);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return Ok(student);
        }

        [HttpPost("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudentAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _studentService.DeleteStudentAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound("Student not found.");
            }

            return NoContent();
        }

        [HttpGet("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var student = await _studentService.GetStudentByIdAsync(id, cancellationToken);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return Ok(student);
        }
    }
}