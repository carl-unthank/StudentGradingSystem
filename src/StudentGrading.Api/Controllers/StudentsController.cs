using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentGrading.Api.Services.Interfaces;
using StudentGrading.Api.Models;

namespace StudentGrading.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentGradeService _studentGradeService;

        public StudentsController(IStudentGradeService studentGradeService)
        {
            _studentGradeService = studentGradeService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStudentsWithDetails()
        {
            var studentDetails = await _studentGradeService.GetAllStudentsWithDetailsAsync();

            return Ok(studentDetails);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentDetails(int id)
        {
            try
            {
                var studentDetails = await _studentGradeService.GetStudentDetailsAsync(id);
                return Ok(studentDetails);
            }
            catch (ArgumentException)
            {
                return NotFound($"Student with ID {id} not found.");
            }
        }
    }
}