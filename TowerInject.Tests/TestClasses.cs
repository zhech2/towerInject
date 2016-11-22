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

    public class DisposableEmailService : IEmailService, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
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
            if (calculator == null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            if (emailService == null)
            {
                throw new ArgumentNullException(nameof(emailService));
            }

            Calculator = calculator;
            EmailService = emailService;
        }
    }

    public interface IService
    {

    }

    public class ClassNoPublicCtors : IService
    {
        protected ClassNoPublicCtors()
        {

        }
    }
}
