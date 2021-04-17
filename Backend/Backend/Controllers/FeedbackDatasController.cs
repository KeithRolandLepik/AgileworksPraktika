using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infra.Feedbacks;
using Domain.Feedbacks;
using Facade.Feedbacks;

namespace Soft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackDatasController : ControllerBase
    {
        private IFeedbackRepository fr;
        public FeedbackDatasController(FeedbackDbContext context)
        {
            fr = new FeedbackRepository(context);
        }

        // GET: api/FeedbackDatas
        [HttpGet]
        public async Task<ActionResult<List<FeedbackView>>> GetFeedbacks()
        {
            var list = await fr.Get();
            var viewList = new List<FeedbackView>();
            foreach(var i in list)
            {
                viewList.Add(FeedbackViewFactory.Create(i));
            }
            return viewList;
        }

        // GET: api/FeedbackDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackView>> GetFeedback(int id)
        {
            var feedback = await fr.Get(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return FeedbackViewFactory.Create(feedback);
        }

        // PUT: api/FeedbackDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, FeedbackView feedbackView)
        {
            if (id != feedbackView.Id)
            {
                return BadRequest();
            }

            await fr.Update(FeedbackViewFactory.Create(feedbackView));

            return NoContent();
        }

        // POST: api/FeedbackDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeedbackView>> PostFeedback(FeedbackView feedbackView)
        {
            await fr.Add(FeedbackViewFactory.Create(feedbackView));

            return CreatedAtAction("GetFeedback", new { id = feedbackView.Id }, FeedbackViewFactory.Create(feedbackView));
        }

        // DELETE: api/FeedbackDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedbackData = await fr.Get(id);
            if (feedbackData == null)
            {
                return NotFound();
            }

            await fr.Delete(id);

            return NoContent();
        }
    }
}
