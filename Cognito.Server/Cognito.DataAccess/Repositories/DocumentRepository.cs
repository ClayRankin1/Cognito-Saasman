﻿using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;

namespace Cognito.DataAccess.Repositories
{
    public class DocumentRepository : CrudRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }
    }
}