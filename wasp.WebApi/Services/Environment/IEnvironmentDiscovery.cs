using System.Runtime.InteropServices;

namespace wasp.WebApi.Services.Environment
{
    public interface IEnvironmentDiscovery
    {
        /// <summary>
        /// Detects whether or not we are on a test system (development/local).<para> </para>
        /// This is done by testing if there is a user environment variable called "DasTeamRevolution_Development" set to either <c>1</c> or <c>true</c>.
        /// </summary>
        bool IsTestEnvironment { get; }
        
        /// <summary>
        /// Checks whether or not we are in a dockerized environment.
        /// </summary>
        bool IsDocker { get; }

        bool Staging { get; }
        bool Live { get; }
    }
}