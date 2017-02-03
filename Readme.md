# Tower Inject

Tower Injector is a simple Dependency Injection or IoC container.

Example
--------

``` c#
    var container = new Container();
    container.Register<ICalculator, Calculator>();
    contianer.Register<IEmailService, EmailService>(LifecycleType.Singleton);
    container.Register<UsersController>();    

    var usersController = container.Resolve<UsersController>();

    public class UsersController
    {
        public UsersController(ICalculator calculator,  IEmailService emailService)
        {
        }
    }
```


Overview
--------
The Container is made up of an IRegistrator and IResolver.  

The registrator registers the services with the container and creates the registration based on the lifecycle.
The resolver looks for a specific IInstanceResolver which is the combination of the Registration and the ILifecycle.  

IInstanceResolver will get or create an instance based on it's lifetime.  

Instances are created by an IFactory.

SingletonLifecycle will create an IInstanceResolver for each type and will return the same instance after it's initial creation by the the IInstanceResolver.

TransientLifecycle will create an IInstanceResolver for each type and will return a new instance for each request.

The IInstanceResolvers are cached by the Container.

If there is a cycle in the services dependencies it will throw an InvalidOperationException.
 
Controller Example
------------------
BookingController is an example that uses the Container to create dependencies.

TowersWatson.postman_collection.json is a collection of postman requests that can be used to test.

Future Features
-------------------
* Func&lt;T> - for lazy creation to break cycles - UsersController(Func&lt;ICalculator> calculator)
* Scope for controlling lifetime of services on a per request basis
* Patterns: 
  * Composite - IEnumerable&lt;T>
  * Decorators
* Open Generics: IFoo<>, Foo<>
* Detect lifetime mismatch
* Execute function after instance creation
* ExpressionTreeFactory for better performance
