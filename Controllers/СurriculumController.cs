// using Microsoft.AspNetCore.Mvc;
// using project.Interfaces.CurriculumInterfaces;
// using System.Threading;
// using System.Threading.Tasks;
// using project.Filters.StudentFilters;
//
// namespace project.Controllers
// {
//     [ApiController]
//     [Route("[controller]")]
//     public class CurriculumController : ControllerBase
//     {
//         private readonly ILogger<CurriculumController> _logger;
//         private readonly ICurriculumService _curriculumService;
//
//         public CurriculumController(ICurriculumService curriculumService, ILogger<CurriculumController> logger)
//         {
//             _logger = logger;
//             _curriculumService = curriculumService;
//         }
//
//         // [HttpPost("AddCurriculum")]
//         // public async Task<IActionResult> AddCurriculumAsync([FromBody] AddCurriculumRequest request, CancellationToken cancellationToken = default)
//         // {
//         //     try
//         //     {
//         //         var curriculum = await _curriculumService.AddCurriculumAsync(request.GroupId, request.ObjectId, request.Hours, cancellationToken);
//         //         return CreatedAtAction(nameof(AddCurriculumAsync), new { id = curriculum.CurriculumId }, curriculum);
//         //     }
//         //     catch (ArgumentException ex)
//         //     {
//         //         return BadRequest(ex.Message);
//         //     }
//         // }
//         [HttpPost("GetStudentsByGroup")]
//         public async Task<IActionResult> GetStudentsByGroupAsync(ObjectsByGroupFilter filter, CancellationToken cancellationToken = default)
//         {
//             var students = await _curriculumService.GetObjectsByGroupAsync(filter, cancellationToken);
//             return Ok(students);
//         }
//     }
// }