﻿namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Net;
    using EEFApps.ApiInstructions.BaseEntities.Entities;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class RemovalOptimizedUserContextedInstruction<TEntity, TId> : RemovalOptimizedInstruction<TEntity, TId>
        where TEntity : BaseEntityWithUserContext<TId>, new()
    {
        public RemovalOptimizedUserContextedInstruction(DbContext context, RemovalInstructionParams<TId> options, string userId) 
            : base(context, options, new TEntity() { Id = options.Id, RowVersion = options.RowVersion, UserId = userId })
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InstructionException("User ID must be provided for the instruction!", HttpStatusCode.BadRequest);
            }
        }
    }
}