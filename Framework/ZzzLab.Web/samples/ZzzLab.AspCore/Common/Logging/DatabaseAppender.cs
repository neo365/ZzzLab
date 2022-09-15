using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using System.Data;
using System.Diagnostics;
using ZzzLab.Data;
using ZzzLab.Data.Configuration;

namespace ZzzLab.AspCore.Logging
{
    /// <summary>
    /// Log4Net에 대한 구현
    /// </summary>
    internal class DatabaseAppender : AdoNetAppender
    {
        private readonly string DefaultSql = ""
                + "INSERT INTO debug_logger ("
                + " machine_name, date_log, stacktrace, log_level, logger, message "
                + " ) VALUES( "
                + $"'{Environment.MachineName}', @date_log, @stacktrace, @log_level, @logger, @message "
                + " ) ";

        internal DatabaseAppender(string loggerName, ConnectionConfig config) : base()
        {
            Name = loggerName;
            BufferSize = 1;
            ConnectionType = config.ServerType.ToConnectionType().ToString();
            CommandType = CommandType.Text;
            CommandText = DefaultSql;
            ConnectionString = config.ConnectionString;
            ReconnectOnError = true;
            UseTransactions = false;

            SetParameter();
        }

        protected virtual void SetParameter()
        {
            this.AddDateTimeParameterToAppender("date_log");
            this.AddStringParameterToAppender("stacktrace", 4000, "%stacktrace{10}");
            this.AddStringParameterToAppender("log_level", 50, "%level");
            this.AddStringParameterToAppender("logger", 255, "%logger");
            this.AddStringParameterToAppender("message", 4000, "%message");
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (events == null || events.Any() == false) return;
            if (string.IsNullOrWhiteSpace(CommandText)) return;
            if (Connection == null) return;

            try
            {
                using (IDbCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = CommandText;
                    cmd.CommandType = CommandType;

                    cmd.Parameters.Clear();

                    // Set the parameter values
                    foreach (AdoNetAppenderParameter param in m_parameters)
                    {
                        param.Prepare(cmd);
                    }

                    // prepare the command, which is significantly faster
                    cmd.Prepare();

                    // run for all events
                    foreach (LoggingEvent e in events)
                    {
                        // Set the parameter values
                        foreach (AdoNetAppenderParameter param in m_parameters)
                        {
                            param.FormatValue(cmd, e);
                        }

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected override void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
        {
            if (events == null || events.Any() == false) return;
            if (string.IsNullOrWhiteSpace(CommandText)) return;
            if (Connection == null) return;

            try
            {
                using (IDbCommand cmd = Connection.CreateCommand())
                {
                    cmd.Transaction = dbTran ?? null;
                    cmd.CommandText = CommandText;
                    cmd.CommandType = CommandType;

                    cmd.Parameters.Clear();

                    // Set the parameter values
                    foreach (AdoNetAppenderParameter param in m_parameters)
                    {
                        param.Prepare(cmd);
                    }

                    // prepare the command, which is significantly faster
                    cmd.Prepare();

                    try
                    {
                        // run for all events
                        foreach (LoggingEvent e in events)
                        {
                            // Set the parameter values
                            foreach (AdoNetAppenderParameter param in m_parameters)
                            {
                                param.FormatValue(cmd, e);
                            }

                            // Execute the query
                            cmd.ExecuteNonQuery();
                        }

                        if (cmd.Transaction != null) cmd.Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        if (cmd.Transaction != null) cmd.Transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected void AddDateTimeParameterToAppender(string paramName)
        {
            AdoNetAppenderParameter param = new AdoNetAppenderParameter
            {
                ParameterName = paramName,
                DbType = System.Data.DbType.DateTime,
                Layout = new RawUtcTimeStampLayout()
            };
            this.AddParameter(param);
        }

        protected void AddErrorParameterToAppender(string paramName, int size)
        {
            AdoNetAppenderParameter param = new AdoNetAppenderParameter
            {
                ParameterName = paramName,
                DbType = System.Data.DbType.String,
                Size = size,
                Layout = new Layout2RawLayoutAdapter(new ExceptionLayout())
            };
            this.AddParameter(param);
        }

        protected void AddStringParameterToAppender(string paramName, int size, string? conversionPattern = null)
        {
            AdoNetAppenderParameter param = new AdoNetAppenderParameter
            {
                ParameterName = paramName,
                DbType = DbType.String,
                Size = size
            };

            if (string.IsNullOrWhiteSpace(conversionPattern) == false)
            {
                param.Layout = new Layout2RawLayoutAdapter(new PatternLayout(conversionPattern));
            }
            this.AddParameter(param);
        }
    }
}