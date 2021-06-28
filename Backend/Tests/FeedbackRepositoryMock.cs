using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Data.Feedbacks;
using Domain.Feedbacks;

namespace Tests
{
    public class FeedbackRepositoryMock : IFeedbackRepository
    {
        internal Fixture Fixture;
        internal List<Feedback> FeedbacksList;
        internal Feedback UserFeedback;
        internal int ExistingId;
        internal int AddCount;
        internal int DeleteCount;
        internal int UpdateCount;

        public FeedbackRepositoryMock()
        {
            Fixture = new Fixture();
        }

        public void SetupFeedbacks()
        {
            FeedbacksList.AddRange(Fixture.CreateMany<FeedbackData>().Select(x => new Feedback(x))); 
        }
        public void SetupFeedbacks(List<Feedback> list)
        {
            FeedbacksList = list;
        }
        public int GetExistingIdOrAddRandomFeedbackAndGetId()
        {
            if(FeedbacksList.Count>0) return ExistingId = FeedbacksList[0].Data.Id;

            FeedbacksList.Add(new Feedback(Fixture.Create<FeedbackData>()));
            ExistingId = FeedbacksList[^1].Data.Id;

            return ExistingId;
        }

        public bool VerifyAdd() => AddCount > 0;
        
        public bool VerifyUpdate() => UpdateCount > 0;

        public bool VerifyDelete() => DeleteCount > 0;

        public void SetupUserFeedback(Feedback feedback)
        {
            UserFeedback = feedback;
        }

        public Task<List<Feedback>> Get()
        {
            return Task.FromResult(FeedbacksList.ToList());
        }

        public Task<Feedback> Get(int id)
        {
            return Task.FromResult(UserFeedback);
        }

        public Task Delete(int id)
        {
            DeleteCount += 1;
            return Task.CompletedTask;
        }

        public Task<Feedback> Add(Feedback obj)
        {
            AddCount += 1;
            return Task.FromResult(UserFeedback);
        }

        public Task Update(Feedback obj)
        {
            UpdateCount += 1;
            return Task.CompletedTask;
        }
    }
}
