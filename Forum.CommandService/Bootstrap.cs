﻿using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Configurations;
using ENode.Infrastructure;
using ENode.MySQL;
using Forum.Domain.Accounts;
using Forum.Infrastructure;
using System.Reflection;

namespace Forum.CommandService
{
    public class Bootstrap
    {
        private static ENodeConfiguration _enodeConfiguration;

        public static void Initialize()
        {
            InitializeENode();
            InitializeCommandService();
        }

        public static void Start()
        {
            _enodeConfiguration.StartEQueue();
        }

        public static void Stop()
        {
            _enodeConfiguration.ShutdownEQueue();
        }

        private static void InitializeCommandService()
        {
            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(Account).Name);
            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program)).Info("Command service initialized.");
        }

        private static void InitializeENode()
        {
            ConfigSettings.Initialize();

            var assemblies = new[]
            {
                Assembly.Load("Forum.Infrastructure"),
                Assembly.Load("Forum.Commands"),
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Domain.Dapper"),
                Assembly.Load("Forum.CommandHandlers"),
                Assembly.Load("Forum.CommandService")
            };
            var setting = new ConfigurationSetting();

            _enodeConfiguration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode(setting)
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseMySqlLockService()
                .UseMySqlEventStore()
                .UseEQueue()
                .BuildContainer()
                .InitializeMySqlEventStore(ConfigSettings.ENodeConnectionString)
                .InitializeMySqlLockService(ConfigSettings.ENodeConnectionString)
                .InitializeBusinessAssemblies(assemblies);
        }
    }
}