using System;
using Microsoft.Extensions.DependencyInjection;

namespace sempack
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
    		var application = new Application(serviceCollection, args);
    		application.Run();	
        }
    }
}
