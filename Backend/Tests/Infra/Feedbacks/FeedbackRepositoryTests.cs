﻿using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Infra.Feedbacks
{
    [TestClass]
    public class FeedbackRepositoryTests
    {

        protected FeedbackData data;
        protected FeedbackRepository obj;
        protected FeedbackDbContext db;
        protected DbSet<FeedbackData> dbSet;
        protected int count;
        protected GetRandom GetRandom;
        protected Type type;
        [TestInitialize]
        public void TestInitialize()
        {
            GetRandom = new GetRandom();
            type = typeof(FeedbackRepository);
            data = GetRandom.FeedbackData();
            count = GetRandom.RndInteger(5, 10);
            var options = new DbContextOptionsBuilder<FeedbackDbContext>().UseInMemoryDatabase("TestDb").Options;
            db = new FeedbackDbContext(options);
            dbSet = ((FeedbackDbContext)db).FeedbackDatas;
            obj = new FeedbackRepository((FeedbackDbContext)db);
            cleanDbSet();
            addItems();
        }

        [TestCleanup] public void TestCleanup() => cleanDbSet();
        [TestMethod] public void IsSealedTest() => Assert.IsTrue(type.IsSealed); 
        
        [TestMethod]
        public void GetTest()
        {
            var l = obj.Get().GetAwaiter().GetResult();

            Assert.AreEqual(l.Count, count);

            var first = l[0].Data;
            var second = l[1].Data;
            var third = l[2].Data;

            Assert.IsTrue(first.DueDate < second.DueDate);
            Assert.IsTrue(second.DueDate < third.DueDate);
        }

        private void addItems()
        {
            for (int i = 0; i < count; i++)
            {
                obj.Add(new Feedback(GetRandom.FeedbackData())).GetAwaiter();
            }
        }
        protected void cleanDbSet()
        {
            foreach (var p in dbSet)
            {
                db.Entry(p).State = EntityState.Deleted;
            }
            db.SaveChanges();
        }
    }
}