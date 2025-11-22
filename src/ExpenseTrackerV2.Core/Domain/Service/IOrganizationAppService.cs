using System;
using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface IOrganizationAppService
{
    Task<Organization> CreateAsync(OrganizationRequest request);
}
