using Autofac;
using TaskManager.Api.Accessors;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.Managers;
using TaskManager.Api.Managers.Interfaces;

namespace TaskManager.Api
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountManager>().As<IAccountManager>();
            builder.RegisterType<TeamManager>().As<ITeamManager>();
            builder.RegisterType<TaskPlanManager>().As<ITaskPlanManager>();
            builder.RegisterType<DBAccessor>().As<IDBAccessor>()
                .WithParameter("rapaportConnectionString", EnvironmentVariables.DbConnectionString);
        }
    }
}
