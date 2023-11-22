// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1860:Avoid using 'Enumerable.Any()' extension method", Justification = "<Pending>", Scope = "member", Target = "~M:DomainServices.CaseService.Api.Endpoints.CancelCase.CancelCaseHandler.isDebtorIdentified(System.Int32,System.Threading.CancellationToken)~System.Threading.Tasks.Task{System.Boolean}")]
[assembly: SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>", Scope = "member", Target = "~M:DomainServices.CaseService.Api.CaseExtensions.ToWorkflowTask(System.Collections.Generic.IReadOnlyDictionary{System.String,System.String})~DomainServices.CaseService.Contracts.WorkflowTask")]
[assembly: SuppressMessage("Performance", "CA1860:Avoid using 'Enumerable.Any()' extension method", Justification = "<Pending>", Scope = "member", Target = "~M:DomainServices.CaseService.Api.Endpoints.CreateTask.CreateTaskHandler.Handle(DomainServices.CaseService.Contracts.CreateTaskRequest,System.Threading.CancellationToken)~System.Threading.Tasks.Task{DomainServices.CaseService.Contracts.CreateTaskResponse}")]
[assembly: SuppressMessage("Performance", "CA1860:Avoid using 'Enumerable.Any()' extension method", Justification = "<Pending>", Scope = "member", Target = "~M:DomainServices.CaseService.Api.Endpoints.SearchCases.SearchCasesHandler.getQuery(DomainServices.CaseService.Contracts.SearchCasesRequest,CIS.Core.Types.Paginable)~System.Linq.IQueryable{DomainServices.CaseService.Api.Database.Entities.Case}")]
