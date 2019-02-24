using NLog;
using sempacklib;

namespace sempack
{
    public static class VerbosityToNlogConverter
    {
        public static LogLevel ConvertLevels(VerbosityLevel level)
        {
            switch(level)
            {
                case VerbosityLevel.None:
                case VerbosityLevel.Q:
                case VerbosityLevel.Quiet:
                case VerbosityLevel.M:
                case VerbosityLevel.Minimal:
                    return LogLevel.Error;
                case VerbosityLevel.N:
                case VerbosityLevel.Normal:
                    return LogLevel.Info;
                case VerbosityLevel.D:
                case VerbosityLevel.Detailed:
                    return LogLevel.Trace;
                default:
                    return LogLevel.Trace;
            }
        }
    }
}