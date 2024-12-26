﻿using Common.Domain;
using Common.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace Modules.HotelAdmin.Persistence;

public sealed class UnitOfWork(HotelAdminContext context) : IUnitOfWork, IDisposable
{
    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries) await entry.ReloadAsync(cancellationToken);
            throw new ConcurrencyException(ex.Message, ex);
        }
    }

    public void Dispose() => context.Dispose();
}