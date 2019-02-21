using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace sempacklib
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddSempackLib(this IServiceCollection services)
        {
            services.AddTransient<ISempackLibrary, SempackLibrary>();
            services.AddTransient<CsProjModifier>();
            services.AddTransient<CommandBuilder>();
            services.AddTransient<CommandLine.Parser>(s => new CommandLine.Parser(cfg => {
             cfg.CaseInsensitiveEnumValues = true;
             cfg.HelpWriter = Console.Error;
            }));
            services.AddTransient<CommandRunner>();

            return services;
        }
    }
}