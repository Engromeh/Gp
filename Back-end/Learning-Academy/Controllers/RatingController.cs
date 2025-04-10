using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        public RatingController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<StudentRatingCourse>> GetRating()
        {
            var Ratings = _ratingRepository.GetAllRating();
            return Ok(Ratings);
        }

        [HttpGet("{id}")]
        public IActionResult GetRatingById(int id)
        {
            var Rate = _ratingRepository.GetRatingById(id);

            if (Rate == null)
            {
                return NotFound();
            }
            return Ok(Rate);
        }
        [HttpPost]
        public IActionResult AddRating([FromBody] RatingDto ratingDto)
        {
            if (ratingDto == null)
            {
                return BadRequest("Rating Data is required.");
            }

            var Rate = new StudentRatingCourse
            {
                Rate = ratingDto.Rate,
                StudentId= ratingDto.StudentId,
                CourseId = ratingDto.CourseId

            };

            _ratingRepository.AddRate(Rate);

            return CreatedAtAction(nameof(GetRatingById), new { id = Rate.Id }, Rate);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRating(int id, [FromBody] RatingDto ratingDto)
        {
            if (ratingDto == null) return BadRequest();

            var existingRate = _ratingRepository.GetRatingById(id);
            if (existingRate == null) return NotFound();

            existingRate.Rate = ratingDto.Rate;
           existingRate.StudentId= ratingDto.StudentId;
           existingRate.CourseId = ratingDto.StudentId;
           
            _ratingRepository.UpdateRate(existingRate);

            return Ok("Rating is Updated");
        }
        [HttpDelete]
        public IActionResult DeleteRating(int id)
        {
            var existingRate = _ratingRepository.GetRatingById(id);
            if (existingRate == null) return NotFound();

            _ratingRepository.DeleteRate(id);
            return Ok($"Rating with ID {id} has been deleted successfully.");
        }
    }
}
