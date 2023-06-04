﻿using Microsoft.OpenApi.Models;
using NOBY.Dto.Workflow;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail;

internal sealed class GetTaskDetailSwaggerSchema : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(WorkflowTaskDetail))
            return;

        var possibleTypes = new[]
        {
            typeof(AmendmentsConsultationData),
            typeof(AmendmentsRequest),
            typeof(AmendmentsSigning),
        };

        foreach (var type in possibleTypes)
        {
            var typeSchema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
            schema.Properties[nameof(WorkflowTaskDetail.Amendments).ToLowerInvariant()].OneOf.Add(typeSchema);
        }
    }
}
