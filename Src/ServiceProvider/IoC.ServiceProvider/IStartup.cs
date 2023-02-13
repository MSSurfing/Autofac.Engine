using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoCServiceProvider
{
    public interface IStartup
    {
        /// <summary>This method gets called by the runtime. Use this method to add services to the container.</summary>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</summary>
        void Configure(IApplicationBuilder application);

        /// <summary>The startup order of this IStartup implementation</summary>
        int Order { get; }
    }
}
