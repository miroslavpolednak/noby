﻿using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CIS.Infrastructure.WebApi.Swagger;

public sealed class SwaggerOneOfSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var oneOfAttribute = context.MemberInfo?.GetCustomAttribute<SwaggerOneOfAttribute>();

        if (oneOfAttribute is null)
            return;

        schema.AllOf = new List<OpenApiSchema>();

        foreach (var possibleType in oneOfAttribute.PossibleTypes)
        {
            var possibleTypeSchema = context.SchemaGenerator.GenerateSchema(possibleType, context.SchemaRepository);

            schema.OneOf.Add(possibleTypeSchema);
        }
    }
}