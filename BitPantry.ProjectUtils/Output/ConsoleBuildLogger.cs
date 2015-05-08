using System;
using BitPantry.StringWriters;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BitPantry.ProjectUtils.Output
{
    /// <summary>
    /// A console build logger implementation to be used with the MsBuild process
    /// </summary>
    public class ConsoleBuildLogger : Logger
    {
        private readonly LogLevel _level;
        private readonly IConsolidatedStringWriterCollection _output;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleBuildLogger"/> class.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="output">Output writer collection</param>
        public ConsoleBuildLogger(IConsolidatedStringWriterCollection output, LogLevel level = LogLevel.Info)
        {
            _level = level;
            _output = output ?? new NullConsolidatedStringWriterCollection();
        }

        /// <summary>
        /// Is always invoked by the build process before the build begins
        /// </summary>
        /// <param name="eventSource">The available events that a logger can subscribe to.</param>
        public override void Initialize(IEventSource eventSource)
        {
            if(eventSource == null)
                throw new ArgumentNullException("eventSource");

            eventSource.ErrorRaised += eventSource_ErrorRaised;

            if (_level >= LogLevel.Warning)
            {
                eventSource.WarningRaised += eventSource_WarningRaised;
            }

            if (_level >= LogLevel.Info)
            {
                eventSource.BuildStarted += eventSource_buildStarted;
                eventSource.ProjectStarted += eventSource_ProjectStarted;

                eventSource.BuildFinished += eventSource_BuildFinished;
                eventSource.ProjectFinished += eventSource_ProjectFinished;

                eventSource.CustomEventRaised += eventSource_CustomEventRaised;
            }
            
            if (_level >= LogLevel.Debug)
            {
                eventSource.TargetStarted += eventSource_TargetStarted;
                eventSource.TargetFinished += eventSource_TargetFinished;
            }

            if (_level >= LogLevel.Verbose)
            {
                eventSource.TaskStarted += eventSource_TaskStarted;
                eventSource.TaskFinished += eventSource_TaskFinished;

                eventSource.MessageRaised += eventSource_MessageRaised;
            }
        }

        void eventSource_MessageRaised(object sender, BuildMessageEventArgs e)
        {
            _output.Verbose.WriteLine(": {0} - {1}", e.Importance, e.Message);
        }

        void eventSource_CustomEventRaised(object sender, CustomBuildEventArgs e)
        { 
            _output.Standard.WriteLine(e.Message);
        }

        void eventSource_TaskFinished(object sender, TaskFinishedEventArgs e)
        {
            _output.Verbose.WriteLine("{0} - TASK FINISHED, {1}, Succeeded: {2} : {3}", e.ProjectFile, e.TaskName, e.Succeeded, e.Message);
        }

        void eventSource_TaskStarted(object sender, TaskStartedEventArgs e)
        {
            _output.Verbose.WriteLine("{0} - TASK STARTED, {1} : {2}", e.ProjectFile, e.TaskName, e.Message);
        }

        void eventSource_TargetFinished(object sender, TargetFinishedEventArgs e)
        {
            _output.Verbose.WriteLine("{0} - TARGET FINISHED, {1}, Succeeded: {2} : {3}", e.ProjectFile, e.TargetName, e.Succeeded, e.Message);
        }

        void eventSource_TargetStarted(object sender, TargetStartedEventArgs e)
        {
            _output.Verbose.WriteLine("{0} - TARGET STARTED, {1} : {2}", e.ProjectFile, e.TargetName, e.Message);
        }

        void eventSource_WarningRaised(object sender, BuildWarningEventArgs e)
        {
            _output.Warning.WriteLine(BuildReferenceLogLine("WARNING", e.File, e.LineNumber, e.ColumnNumber, e.Message));
        }

        void eventSource_ProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            _output.Standard.WriteLine("{0} - PROJECT FINISHED, {1} : {2}", e.ProjectFile, e.Succeeded, e.Message);
        }

        void eventSource_BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            _output.Standard.WriteLine("BUILD FINISHED, Succeeded: {0}", e.Succeeded);
        }

        private void eventSource_buildStarted(object sender, BuildStartedEventArgs e)
        {
            _output.Standard.WriteLine(string.Format("BUILD STARTED"));
        }

        void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            _output.Error.WriteLine(": ERROR {0}({1},{2}): {3}", e.File, e.LineNumber, e.ColumnNumber, e.Message);
        }

        void eventSource_ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            _output.Standard.WriteLine("{0} - PROJECT STARTED : {1}", e.ProjectFile, e.Message);
        }

        #region MESSAGE HELPERS

        static string BuildReferenceLogLine(string severity, string file, int lineNumber, int columnNumber,
            string message)
        {
            return String.Format(": {0} {1}({2},{3}): {4}", severity, file, lineNumber, columnNumber, message);
        }

        #endregion

        /// <summary>
        /// Shutdown() is guaranteed to be called by MsBuild at the end of the build, after all
        /// events have been raised.
        /// </summary>
        public override void Shutdown()
        {
            // do nothing - nothing to clean up
        }
    }
}
