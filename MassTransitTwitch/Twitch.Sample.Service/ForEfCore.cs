//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MassTransit.EntityFrameworkCoreIntegration;
//using MassTransit.EntityFrameworkCoreIntegration.Mappings;
//using MassTransitTwitch.Sample.Components.StateMachines;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace MassTransitTwitch.Sample.Service
//{
//    public class OrderStateMap : SagaClassMap<OrderState>
//    {
//        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
//        {
//            entity.Property(x => x.CurrentState).HasMaxLength(64);
//            entity.Property(x => x.CustomerNumber);
//            entity.Property(x => x.SubmitDate);
//            entity.Property(x => x.Updated);

//            // If using Optimistic concurrency, otherwise remove this property
//            entity.Property(x => x.RowVersion).IsRowVersion();
//        }
//    }
//    public class OrderStateDbContext : SagaDbContext
//    {
//        public OrderStateDbContext(DbContextOptions options)
//            : base(options)
//        {
//        }

//        protected override IEnumerable<ISagaClassMap> Configurations
//        {
//            get { yield return new OrderStateMap(); }
//        }
//    }

//    public class OrderStateContextFactory : IDesignTimeDbContextFactory<OrderStateDbContext>
//    {
//        public OrderStateDbContext CreateDbContext(string[] args)
//        {
//            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MassOrderDb;Integrated Security=True;Connect Timeout=300;";

//            var optionsBuilder = new DbContextOptionsBuilder<OrderStateDbContext>();
//            optionsBuilder.UseSqlServer(connectionString);

//            return new OrderStateDbContext(optionsBuilder.Options);
//        }
//    }
//}

/*
   var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MassOrderDb;Integrated Security=True;Connect Timeout=300;";
   
  c.AddSagaStateMachine<OrderStateMachine, OrderState>()
   //.EntityFrameworkRepository(r =>
   //{
   //    //r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion
   
   //    r.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
   //    {
   //        builder.UseSqlServer(connectionString, m =>
   //        {
   //            m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
   //            m.MigrationsHistoryTable($"__{nameof(OrderStateDbContext)}");
   //        });
   //    });
   //});
 */
