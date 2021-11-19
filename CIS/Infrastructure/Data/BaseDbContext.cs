using System;
using CIS.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace CIS.Infrastructure.Data
{
    public abstract class BaseDbContext : DbContext
    {
        /// <summary>
        /// ID of current user
        /// </summary>
        protected int? _currentUserId = null;

        public BaseDbContext(DbContextOptions options, Core.Security.ICurrentUserProvider userProvider)
            : base(options)
        {
            _currentUserId = userProvider.Get()?.Id;
        }

        /// <summary>
        /// Automaticka aplikace created/modified/actual interfacu
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            addInterfaceFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            addInterfaceFields();
            return base.SaveChanges();
        }

        private void addInterfaceFields()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        if (entry.Entity is IActual)
                        {
                            entry.State = EntityState.Modified;

                            ((IActual)entry.Entity).IsActual = false;

                            if (entry.Entity is IUpdatable)
                            {
                                var obj = (IUpdatable)entry.Entity;
                                obj.UpdateTime = DateTime.Now;
                                obj.UpdateUserId = _currentUserId ?? 0;
                            }
                        }
                        break;

                    case EntityState.Added:
                        if (entry.Entity is IInsertable)
                        {
                            var obj = (IInsertable)entry.Entity;
                            if (!obj.InsertTime.HasValue)
                            {
                                obj.InsertTime = DateTime.Now;
                            }
                            if (!obj.InsertUserId.HasValue)
                            {
                                obj.InsertUserId = _currentUserId ?? 0;
                            }
                        }
                        if (entry.Entity is IActual)
                        {
                            ((IActual)entry.Entity).IsActual = true;
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is IUpdatable)
                        {
                            var obj = (IUpdatable)entry.Entity;
                            obj.UpdateTime = DateTime.Now;
                            obj.UpdateUserId = _currentUserId ?? 0;
                        }
                        break;
                }
            }
        }
    }
}
