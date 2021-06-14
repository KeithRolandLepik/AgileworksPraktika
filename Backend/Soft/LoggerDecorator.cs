using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Feedbacks;
using Infra.Feedbacks;

namespace Soft
{
    public class LoggerDecorator : IFeedbackRepository
    {
        public LoggerDecorator(FeedbackRepository feedbackRepository)
        {
            FeedbackRepository = feedbackRepository;
        }

        public FeedbackRepository FeedbackRepository { get; }

        public Task<Feedback> Add(Feedback obj)
        {
            // LOG logic

            return FeedbackRepository.Add(obj);
        }

        public Task Delete(int id)
        {
            return FeedbackRepository.Delete(id);
        }

        public Task<List<Feedback>> Get()
        {
            return FeedbackRepository.Get();
        }

        public Task<Feedback> Get(int id)
        {
            return FeedbackRepository.Get(id);
        }

        public Task Update(Feedback obj)
        {
            return FeedbackRepository.Update(obj);
        }
    }
}