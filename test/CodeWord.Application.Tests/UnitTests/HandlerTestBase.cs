using CodeWord.Application.Common.Behaviours;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using AutoFixture;

namespace CodeWord.Application.Tests.UnitTests
{
    public abstract class HandlerTestBase<THandler, TRequest, TResponse>
        where THandler : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected AutoMocker Mocker;
        protected Fixture Fixture;
        protected THandler Handler;
        protected TRequest CommandQuery;
        protected TResponse Response;
        protected IMediator Mediator;
        protected IServiceCollection Services;
        protected IServiceProvider ServiceProvider;
        protected Random Randomizer;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Randomizer = new();
            Mocker = new AutoMocker();
            Fixture = new Fixture();
        }

        [SetUp]
        public virtual void Setup()
        {
            Services = new ServiceCollection();
            Services.AddFluentValidation(_ => { _.RegisterValidatorsFromAssemblyContaining<TRequest>(lifetime: ServiceLifetime.Scoped); });
            Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            Services.AddMediatR(typeof(THandler));
            Services.AddAutoMapper(typeof(THandler));
            Services = this.SetupServices(Services);
            ServiceProvider = Services.BuildServiceProvider();
            Mediator = ServiceProvider.GetRequiredService<IMediator>();

            SetupHandler();
        }

        public abstract void SetupHandler();

        public virtual IServiceCollection SetupServices(IServiceCollection services)
        {
            return services;
        }

    }
}
