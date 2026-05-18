using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Formatting;
using Serilog.Parsing;

namespace DataUploader.Application.Serilog
{
    public class SerilogJsonFormatter : ITextFormatter
    {
        private readonly JsonValueFormatter _valueFormatter;

        //
        // 摘要:
        //     Construct a Serilog.Formatting.Compact.SerilogJsonFormatter, optionally supplying
        //     a formatter for Serilog.Events.LogEventPropertyValues on the event.
        //
        // 参数:
        //   valueFormatter:
        //     A value formatter, or null.
        public SerilogJsonFormatter(JsonValueFormatter valueFormatter = null)
        {
            _valueFormatter = valueFormatter ?? new JsonValueFormatter("$type");
        }

        //
        // 摘要:
        //     Format the log event into the output. Subsequent events will be newline-delimited.
        //
        // 参数:
        //   logEvent:
        //     The event to format.
        //
        //   output:
        //     The output.
        public void Format(LogEvent logEvent, TextWriter output)
        {
            FormatEvent(logEvent, output, _valueFormatter);
            output.WriteLine();
        }

        //
        // 摘要:
        //     Format the log event into the output.
        //
        // 参数:
        //   logEvent:
        //     The event to format.
        //
        //   output:
        //     The output.
        //
        //   valueFormatter:
        //     A value formatter for Serilog.Events.LogEventPropertyValues on the event.
        public static void FormatEvent(LogEvent logEvent, TextWriter output, JsonValueFormatter valueFormatter)
        {
            ArgumentNullException.ThrowIfNull(logEvent);

            ArgumentNullException.ThrowIfNull(output);

            ArgumentNullException.ThrowIfNull(valueFormatter);

            output.Write("{\"上传时间\":\"");
            output.Write(logEvent.Timestamp.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            output.Write("\",\"内容\":");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Text, output);
            IEnumerable<PropertyToken> enumerable = from pt in logEvent.MessageTemplate.Tokens.OfType<PropertyToken>()
                                                    where pt.Format != null
                                                    select pt;
            if (enumerable.Any())
            {
                output.Write(",\"@r\":[");
                string value = "";
                foreach (PropertyToken item in enumerable)
                {
                    output.Write(value);
                    value = ",";
                    StringWriter stringWriter = new StringWriter();
                    item.Render(logEvent.Properties, stringWriter);
                    JsonValueFormatter.WriteQuotedJsonString(stringWriter.ToString(), output);
                }

                output.Write(']');
            }

            if (logEvent.Level != LogEventLevel.Information)
            {
                output.Write(",\"@l\":\"");
                output.Write(logEvent.Level);
                output.Write('"');
            }

            if (logEvent.Exception != null)
            {
                output.Write(",\"@x\":");
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }

            foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
            {
                string text = property.Key;
                if (text.Length > 0 && text[0] == '@')
                {
                    text = "@" + text;
                }

                output.Write(',');
                JsonValueFormatter.WriteQuotedJsonString(text, output);
                output.Write(':');
                valueFormatter.Format(property.Value, output);
            }

            output.Write('}');
        }
    }
}
