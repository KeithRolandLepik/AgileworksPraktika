using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infra.Feedbacks;
using Domain.Feedbacks;
using Facade.Feedbacks;
using System;

namespace Soft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private IFeedbackRepository _repository;
        public FeedbackController(FeedbackDbContext context)
        {
            _repository = new FeedbackRepository(context);
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedbackView>>> GetFeedbacks()
        {
            var list = await _repository.Get();
            var viewList = new List<FeedbackView>();
            foreach(var i in list)
            {
                viewList.Add(FeedbackMapper.MapToView(i));
            }
            return viewList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackView>> GetFeedback(int id)
        {
            var feedback = await _repository.Get(id);
            if (feedback.Data == null)
            {
                return NotFound();
            }
            return Ok(FeedbackMapper.MapToView(feedback));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, FeedbackUpdate feedbackUpdate)
        {   
            if(feedbackUpdate.Description != null && feedbackUpdate.DueDate != default)
            {
                var updatedFeedback = FeedbackMapper.MapToDomainFromUpdate(feedbackUpdate);

                updatedFeedback.Data.Id = id;

                var feedbackToUpdate = await _repository.Get(id);

                if (feedbackToUpdate.Data != null)
                {
                    await _repository.Update(updatedFeedback);
                    return Ok();
                }
            }
            return BadRequest();

        }

        [HttpPost]
        public async Task<ActionResult<FeedbackView>> PostFeedback(FeedbackInput feedbackPost)
        {
            if(feedbackPost != null)
            {
                if(feedbackPost.Description != null && feedbackPost.DueDate != default) 
                { 
                    var result = await _repository.Add(FeedbackMapper.MapToDomainFromInput(feedbackPost));

                    if(result == null)
                    {
                        return Conflict();
                    }
                    return CreatedAtAction("GetFeedback", new { id = result.Data.Id}, FeedbackMapper.MapToView(result));
                }
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedbackData = await _repository.Get(id);

            if (feedbackData.Data == null)
            {
                return NotFound();
            }

            await _repository.Delete(id);

            return NoContent();
        }
    }
}
