using SharedAudit.Dto;
using System.Globalization;

namespace SharedAudit;

internal static class AuditLoggerJsonWriter
{
    static System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = false
    };

    public static void CreateJson(
        StringWriter output,
        ref string time,
        ref string? hashId,
        ref long? sequenceId,
        ref AuditLoggerDefaults loggerDefaults,
        AuditEventContext context,
        ReadOnlySpan<char> eventTypeId,
        int eventTypeVersion)
    {
        output.Write("{\"id\":\"");
        if (!string.IsNullOrEmpty(hashId))
            output.Write(hashId);
        output.Write("\",");

        #region meta
        output.Write("\"@meta\":{");

        // logger
        output.Write("\"logger\":\"NOBY\"");

        // seqno
        if (sequenceId.HasValue)
        {
            output.Write(",\"seqNo\":");
            write(output, sequenceId.Value.ToString(CultureInfo.InvariantCulture));
        }

        // timestamp
        output.Write(",\"timestamp\":");
        write(output, time);

        // version
        output.Write(",\"version\":");
        write(output, loggerDefaults.EamVersion.AsSpan());

        // eam
        output.Write(",\"eamApplication\":");
        write(output, loggerDefaults.EamApplication.AsSpan());

        output.Write("}");
        #endregion meta

        #region header
        output.Write(",\"header\":{");

        output.Write("\"message\":");
        write(output, context.Message.AsSpan());

        #region event
        output.Write(",\"event\":{");

        // correlation
        output.Write("\"correlation\":");
        write(output, context.Correlation.AsSpan());

        // result
        output.Write(",\"result\":");
        write(output, context.Result.AsSpan());

        // time
        output.Write(",\"time\":{\"tsServer\":");
        write(output, time);
        output.Write("}");

        // type
        output.Write(",\"type\":{");
        output.Write("\"id\":");
        write(output, eventTypeId);
        output.Write(",\"version\":");
        write(output, eventTypeVersion.ToString(CultureInfo.InvariantCulture));
        output.Write("}");

        // source
        output.Write(",\"source\":{");
        output.Write("\"id\":");
        write(output, loggerDefaults.CisAppKey.AsSpan());
        output.Write(",\"name\":");
        write(output, loggerDefaults.EamApplication.AsSpan());
        output.Write(",\"environment\":");
        write(output, loggerDefaults.EnvironmentName.AsSpan());
        output.Write(",\"ipClient\":");
        write(output, context.ClientIp.AsSpan());
        output.Write(",\"ipServer\":");
        write(output, loggerDefaults.ServerIp.AsSpan());
        output.Write("}");

        output.Write("}");
        #endregion event

        #region actor
        if (!string.IsNullOrEmpty(context.UserIdent))
        {
            int idx = context.UserIdent.IndexOf('=', 1);
            output.Write(",\"actor\":{");
            output.Write("\"idSchema\":");
            write(output, context.UserIdent[..idx].AsSpan());
            output.Write(",\"id\":");
            write(output, context.UserIdent[(idx + 1)..].AsSpan());
            output.Write("}");
        }
        #endregion actor

        if (context.Identities is not null && context.Identities.Count != 0)
        {
            output.Write(",\"identity\":[");
            writeHeaderIdentity(output, context.Identities);
            output.Write("]");
        }

        if (context.Products is not null && context.Products.Count != 0)
        {
            output.Write(",\"product\":[");
            writeHeaderProduct(output, context.Products);
            output.Write("]");
        }

        if (context.Operation is not null)
        {
            output.Write(",\"operation\":{\"type\":");
            write(output, context.Operation.Type.AsSpan());
            if (!string.IsNullOrEmpty(context.Operation.Id))
            {
                output.Write(",\"id\":");
                write(output, context.Operation.Id.AsSpan());
            }
            output.Write("}");
        }

        output.Write("}");
        #endregion header

        #region body
        output.Write(",\"body\":{");

        if (context.BodyBefore is not null)
        {
            output.Write("\"objectsBefore\":");
            //System.Web.HttpUtility.JavaScriptStringEncode()
            //System.Text.Json.JsonEncodedText.Encode
            var t = System.Web.HttpUtility.JavaScriptStringEncode(System.Text.Json.JsonSerializer.Serialize(context.BodyBefore, _jsonSerializerOptions));
            write(output, t);
        }
        
        if (context.BodyAfter is not null)
        {
            if (context.BodyBefore is not null) output.Write(",");

            output.Write("\"objectsAfter\":");
            var t = System.Web.HttpUtility.JavaScriptStringEncode(System.Text.Json.JsonSerializer.Serialize(context.BodyAfter, _jsonSerializerOptions));
            write(output, t);
        }

        output.Write("}");
        #endregion body

        output.Write("}");
    }

    private static void writeHeaderIdentity(StringWriter output, ICollection<AuditLoggerHeaderItem> items)
    {
        int i = 0;
        foreach (var identity in items)
        {
            if (i++ > 0) output.Write(",");
            output.Write("{\"id\":");
            write(output, identity.Id.AsSpan());
            output.Write(",\"idSchema\":");
            write(output, identity.Type.AsSpan());
            output.Write("}");
        }
    }

    private static void writeHeaderProduct(StringWriter output, ICollection<AuditLoggerHeaderItem> items)
    {
        int i = 0;
        foreach (var product in items)
        {
            if (i++ > 0) output.Write(",");
            output.Write("{\"type\":");
            write(output, product.Type.AsSpan());
            if (!string.IsNullOrEmpty(product.Id))
            {
                output.Write(",\"id\":");
                write(output, product.Id.AsSpan());
            }
            output.Write("}");
        }
    }

    private static void write(StringWriter output, ReadOnlySpan<char> text)
    {
        output.Write("\"");
        output.Write(text);
        output.Write("\"");
    }
}
