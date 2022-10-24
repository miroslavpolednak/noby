﻿using CIS.InternalServices.DocumentDataAggregator.Configuration.Model;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data;

[TransientService, SelfService]
internal class ConfigurationRepository
{
    private readonly ConfigurationContext _dbContext;

    public ConfigurationRepository(ConfigurationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<SourceField>> LoadSourceFields(int documentId, int documentVersion)
    {
        return _dbContext.DocumentDataFields
                         .AsNoTracking()
                         .Where(f => f.DocumentId == documentId && f.DocumentVersion == documentVersion)
                         .Select(f => new SourceField
                         {
                             DataSource = (DataSource)f.DataField.DataServiceId,
                             FieldPath = f.DataField.FieldPath,
                             TemplateFieldName = f.TemplateFieldName
                         })
                         .ToListAsync();
    }

    public Task<List<DynamicInputParameter>> LoadDynamicInputFields(int documentId, int documentVersion)
    {
        return _dbContext.DocumentDynamicInputParameters
                         .AsNoTracking()
                         .Where(d => d.DocumentId == documentId && d.DocumentVersion == documentVersion)
                         .Select(d => new DynamicInputParameter
                         {
                             InputParameterName = d.InputParameter.InputParameterName,
                             TargetDataSource = (DataSource)d.TargetDataServiceId,
                             SourceDataSource = (DataSource)d.SourceDataField.DataServiceId,
                             SourceFieldPath = d.SourceDataField.FieldPath
                         })
                         .ToListAsync();
    }
}