using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Feedbacks;
using Facade.Feedbacks;
using System.Linq;

namespace Soft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _repository;
        public FeedbackController(IFeedbackRepository Repository)
        {
            _repository = Repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedbackView>>> GetFeedbacks()
        {
            var list = await _repository.Get();

            var viewList = list.Select(x => FeedbackMapper.MapToView(x)).ToList();

            return viewList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackView>> GetFeedback(int id)
        {
            var feedback = await _repository.Get(id);

            if (feedback.Data == null) return NotFound();

            return Ok(FeedbackMapper.MapToView(feedback));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, FeedbackUpdate feedbackUpdate)
        {
            if (feedbackUpdate.Description == null || feedbackUpdate.DueDate == default) return BadRequest();

            var feedbackToUpdate = await _repository.Get(id);

            if (feedbackToUpdate.Data == null)
                return BadRequest();
            
            FeedbackMapper.MapToDomainFromUpdate(feedbackToUpdate, feedbackUpdate);

            await _repository.Update(FeedbackMapper.MapToDomainFromUpdate(feedbackToUpdate, feedbackUpdate));

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<FeedbackView>> PostFeedback(FeedbackInput feedbackPost)
        {
            if (feedbackPost == null 
                || feedbackPost.Description == null 
                || feedbackPost.DueDate == default) 
                return BadRequest();


            var result = await _repository.Add(FeedbackMapper.MapToDomainFromInput(feedbackPost));
            if (result.Data == null) 
                return Conflict();

            return CreatedAtAction("GetFeedback", new { id = result.Data.Id }, FeedbackMapper.MapToView(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedbackData = await _repository.Get(id);

            if (feedbackData.Data == null) 
                return NotFound();

            await _repository.Delete(id);

            return NoContent();
        }
    }
}
