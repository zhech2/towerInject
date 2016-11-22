using System;

namespace TowerInject.Tests
{
    public class A
    {
        public A(B b)
        {

        }
    }

    public class B
    {
        public B(A a)
        {
        }
    }

    public interface IEmailService
    {
    }

    public class NullEmailService : IEmailService
    {
    }

    public interface ICalculator
    {
        ILogger Logger { get; }
    }

    public class NullCalculator : ICalculator
    {
        public ILogger Logger { get; }

        public NullCalculator(ILogger logger)
        {
            Logger = logger;
        }
    }

    public interface ILogger
    {
    }

    public class NullLogger : ILogger
    {

    }

    public class UsersController
    {
        public ICalculator Calculator { get; }
        public IEmailService EmailService { get; }

        public UsersController(ICalculator calculator, IEmailService emailService)
        {
            Calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            EmailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }
    }
}
