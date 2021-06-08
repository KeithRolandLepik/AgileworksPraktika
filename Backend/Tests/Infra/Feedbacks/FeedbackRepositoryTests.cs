//using Data.Feedbacks;
//using Domain.Feedbacks;
//using Infra.Feedbacks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;

//namespace Tests.Infra.Feedbacks
//{
//    [TestClass]
//    public class FeedbackRepositoryTests
//    {

//        protected FeedbackData data;
//        protected FeedbackRepository obj;
//        protected DbSet<FeedbackData> dbSet;
//        protected int count;
//        protected Type type;
//        [TestInitialize]
//        public void TestInitialize()
//        {
//            type = typeof(FeedbackRepository);
//            data = GetRandom.FeedbackData();
//            count = GetRandom.RndInteger(5, 10);
//            var options = new DbContextOptionsBuilder<FeedbackDbContext>().UseInMemoryDatabase("TestDb").Options;
//            db = new FeedbackDbContext(options);
//            dbSet = ((FeedbackDbContext)db).FeedbackDatas;
//            obj = new FeedbackRepository((FeedbackDbContext)db);
//            cleanDbSet();
//            addItems();
//        }

//        [TestCleanup] public void TestCleanup() => cleanDbSet();
//        [TestMethod] public void IsSealedTest() => Assert.IsTrue(type.IsSealed); 
        

//        private void addItems()
//        {
//            for (int i = 0; i < count; i++)
//            {
//                obj.Add(new Feedback(GetRandom.FeedbackData())).GetAwaiter();
//            }
//        }
//        protected void cleanDbSet()
//        {
//            foreach (var p in dbSet)
//            {
//                db.Entry(p).State = EntityState.Deleted;
//            }
//            db.SaveChanges();
//        }
//    }
//}
