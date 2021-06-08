//using Domain.Common;
//using Domain.Feedbacks;
//using Infra.Feedbacks;
//using Marten;
//using Marten.Services;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Threading.Tasks;

//namespace Soft
//{

//    public static class MartenInstaller
//    {
//        public static void AddMarten(this IServiceCollection services, string cnnString)
//        {
//            services.AddSingleton(CreateDocumentStore(cnnString));

//            services.AddScoped<IDataStore, MartenDataStore>();
//        }

//        private static IDocumentStore CreateDocumentStore(string cn)
//        {
//            return DocumentStore.For(_ =>
//            {
//                _.Connection(cn);
//                _.DatabaseSchemaName = "feedbacks";
//                _.Serializer(CustomizeJsonSerializer());
//            });
//        }

//        private static JsonNetSerializer CustomizeJsonSerializer()
//        {
//            var serializer = new JsonNetSerializer();

//            return serializer;
//        }
//    }
//    public class MartenDataStore : IDataStore
//    {
//        private readonly IDocumentSession session;

//        public MartenDataStore(IDocumentStore documentStore)
//        {
//            session = documentStore.LightweightSession();
//            Feedbacks = new FeedbackRepository(session);
//        }

//        public IFeedbackRepository Feedbacks { get; }

//        public IFeedbackRepository FeedbacksRepository => throw new System.NotImplementedException();

//        public async Task CommitChanges()
//        {
//            await session.SaveChangesAsync();
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                session.Dispose();
//            }

//        }
//    }
//}
