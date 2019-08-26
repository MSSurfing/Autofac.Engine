<p align="center">
  <img src="https://autofac.org/img/autofac_logo-type.svg" height="100">
</p>

有关Autofac的使用帮助可以查看：[Autofac中文文档](https://autofaccn.readthedocs.io/zh/latest/index.html)

## 注册组件
可以在任意地方通过现实 IDependencyRegistrar 接口进行注册组件。

Order属性代表注册优化级。

Register方法 提供了ContainerBuilder 用于组件注册的，还提供了 ITypeFinder 可用于动态加载程序并查找程序集中的类型，可用于动态加载插件。

**IDependencyRegistrar 的实现 可以有多个，可放在项目的任意地方。**

```C#
// Surf.Data.dll
public class DependencyRegistrar : IDependencyRegistrar
{
    public int Order => 1;

    public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
    {
        //泛型类型的注册
        builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).SingleInstance();
    }
}

// Surf.Service.dll
public class DependencyRegistrar2 : IDependencyRegistrar
{
    public int Order => 2; // or 1

    public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
    {
        builder.RegisterType<Service>().As<IService>().InstancePerLifetimeScope();
    }
}
```
组件注册更多参考：[Autofac中文文档-组件注册](https://autofaccn.readthedocs.io/zh/latest/register/parameters.html)

组件注册更多参考：[Autofac英文文档-Registration Concepts](https://autofac.readthedocs.io/en/latest/register/registration.html)

## 应用启动
在应用程序启动时调用 EngineContext.Initialie();初始化注册，就可以再任意地方解析服务。

#### Main 的启动方式（使用于Console、Service）

```C#
static void Main(string[] args)
{
    EngineContext.Initialize();
}
```
#### .Net Framework 的启动方式

在 Global.cs  的 Application_Start() 中调用 EngineContext.Initialize() 

Web和Api程序需要分别设置解析器，这里需要引用Autofac.Mvc5、Autofac.WebApi2

```C#
protected void Application_Start()
{
    // 初始化
    EngineContext.Initialize(ScopeTag.Http);           

    //用于 Web Mvc, 需引入 PM>  Install-Package Autofac.Mvc5
    System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(EngineContext.Scope));  
	     
    //用于Web Api,需引入 PM>  Install-Package Autofac.WebApi2
    System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(EngineContext.Scope);                
}
```

#### .Net Core 的启动方式
需要在 Startup.cs 的ConfigureServices方法中调用 EngineContext.Initialize(services);

并把 ConfigureServices 的返回类型 从 Void 改为 IServiceProvider 

~~public void ConfigureServices(IServiceCollection services)~~


```C#
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddMvc();

	// 必须要 return 用于 Only Api
	return EngineContext.Initialize(services, ScopeTag.Http);       
    //or  return EngineContext.Initialize(services);    //用于 Web / Api
}
```

应用启动参考：[Application Startup](https://autofaccn.readthedocs.io/zh/latest/getting-started/index.html#id3)

应用启动-英文参考：[Application Startup](https://autofac.readthedocs.io/en/latest/getting-started/index.html#application-startup)


## 解析服务（获取对象）
可以在任意地方 使用 EngineContext.Resolve 方法来解析服务

```C#
EngineContext.Resolve<IService>();
```
如果要使用BeginLifetimeScope()，可以这样
```C#
using(var scope = EngineContext.BeginLifetimeScope())
{
   var service = scope.Resolve<IService>();
   var service2 = scope.Resolve<IService2>();
}
```

解析服务可以参考：[解析服务](https://autofaccn.readthedocs.io/zh/latest/resolve/index.html)

## 更多方式的应用

#### ITypeFinder 应用例子
使用ITypeFinder加载安全的IPlugin

写好的Plugin可以在需要时放到Bin目录下，通过ITypeFinder加载、执行。
```C#
public JsonResult LoadAllPlugins()
{
    // 动态加载插件
    var typeFinder = EngineContext.Resolve<ITypeFinder>();
    var type = typeFinder.FindClassesOfType(typeof(IPlugin)).FirstOrDefault();

    // 创建插件对象
    IPlugin plugin = null;
    if (!EngineContext.TryResolve(type, out object instance))
        instance = EngineContext.BeginLifetimeScope().ResolveUnregistered(type);

    plugin = instance as IPlugin;
    if (plugin == null)
    return Json(false);

    // 执行插件方法
    var executedResult = plugin.Execute();
    return Json(executedResult);
}
```
#### 服务注册 方式
一般服务注册
```C#
// PerDependency
builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();

// PerLifetime
builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();
```

匹配后缀注册 typeEndName
```C#
// 注册 Services.dll 下所有 以Service结尾的服务。
// 如：AppRoleService、WorksheetService、
builder.RegisterAssemblyTypes(typeFinder, "Services.dll", typeEndName: "Service").AsImplementedInterfaces().InstancePerLifetimeScope();
```

泛型注册 RegisterGeneric
```C#
builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).SingleInstance();
```

单例服务的注册 SingleInstance
```C#
builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();
```

例子代码 可查看：[Samples](https://github.com/MSSurfing/Autofac.Engine/tree/master/Samples)

NuGet：[Autofac.Engine](https://www.nuget.org/packages?q=autofac.engine)
