using System;
using IronSphere.Extensions;

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

    public class EnvironmentDiscovery : IEnvironmentDiscovery
    {
        private static string? _getVariable(string variable, EnvironmentVariableTarget target)
            => System.Environment.GetEnvironmentVariable(variable, target);

        /// <inheritdoc cref="IEnvironmentDiscovery"/>
        public bool IsTestEnvironment
        {
            get
            {
                string? environmentVariableDevelopment =
                    _getVariable("WaspDevelopment", EnvironmentVariableTarget.User)
                    ?? _getVariable("WaspDevelopment", EnvironmentVariableTarget.Process);

                if (environmentVariableDevelopment is null)
                    return false;

                return !environmentVariableDevelopment.IsNullOrWhiteSpace()
                       && (environmentVariableDevelopment == "1" ||
                           environmentVariableDevelopment.ToLowerInvariant() == "true");
            }
        }

        public bool IsDocker
        {
            get
            {
                string? docker =
                    _getVariable("WaspDocker", EnvironmentVariableTarget.User)
                    ?? _getVariable("WaspDocker", EnvironmentVariableTarget.Process);

                return !docker.IsNullOrWhiteSpace() && (docker == "1" || docker?.ToLowerInvariant() == "true");
            }
        }

        #if DEBUG
        public bool Staging => false;
        public bool Live => false;
        #else
        public bool Staging => IsTestEnvironment;
        public bool Live => !IsTestEnvironment;
        #endif
    }
}