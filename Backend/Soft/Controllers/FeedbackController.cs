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
        public FeedbackController(IFeedbackRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedbackModel>>> GetFeedbacks()
        {
            var feedbackList = await _repository.Get();

            var feedbackModelList = feedbackList.Select(FeedbackMapper.MapToModel).ToList();

            return feedbackModelList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackModel>> GetFeedback(int id)
        {
            var feedback = await _repository.Get(id);

            if (feedback.Data == null) return NotFound();

            return Ok(FeedbackMapper.MapToModel(feedback));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, UpdateFeedbackRequest updateFeedbackRequest)
        {
            if (updateFeedbackRequest.Description == null || updateFeedbackRequest.DueDate == default) return BadRequest();

            var feedbackToUpdate = await _repository.Get(id);

            if (feedbackToUpdate.Data == null)
                return BadRequest();

            await _repository.Update(FeedbackMapper.MapToDomainFromUpdateRequest(feedbackToUpdate, updateFeedbackRequest));

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<FeedbackModel>> PostFeedback(AddFeedbackRequest addFeedbackPost)
        {
            if (addFeedbackPost?.Description == null || addFeedbackPost.DueDate == default) 
                return BadRequest();

            var feedback = FeedbackMapper.MapToDomainFromAddRequest(addFeedbackPost);
            //if (feedback.Data.DueDate < feedback.Data.DateAdded) return BadRequest();
            
            var result = await _repository.Add(feedback);
            if (result.Data == null) 
                return Conflict();

            return CreatedAtAction("GetFeedback", new { id = result.Data.Id }, FeedbackMapper.MapToModel(result));
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
