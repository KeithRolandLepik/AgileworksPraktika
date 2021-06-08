//using Data.Feedbacks;
//using Microsoft.EntityFrameworkCore;
//using System;

//namespace Infra.Feedbacks
//{
//    public class FeedbackDbContext : DbContext
//    {
//        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) : base(options)
//        { }
//        public DbSet<FeedbackData> FeedbackDatas { get; set; }
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);
//            modelBuilder.Entity<FeedbackData>().HasKey(x => x.Id);

//            modelBuilder.Entity<FeedbackData>().HasData(
//            new FeedbackData
//            {
//                Id = 1,
//                Description = "Test1",
//                DueDate = new DateTime(2021, 6, 6, 20, 40, 0),
//                DateAdded = DateTime.Now,
//                Completed = false
//            },
//            new FeedbackData
//            {
//                Id =2,
//                Description = "Test2",
//                DueDate = new DateTime(2021, 6, 6, 20, 40, 0),
//                DateAdded = DateTime.Now,
//                Completed = false
//            },
//            new FeedbackData
//            {
//                Id=3,
//                Description = "Test3",
//                DueDate = DateTime.Now.AddHours(1),
//                DateAdded = DateTime.Now,
//                Completed = false
//            });
//        }
//    }
//}
