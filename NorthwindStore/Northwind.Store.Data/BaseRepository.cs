﻿using Microsoft.EntityFrameworkCore;
using Northwind.Store.Model;
using Northwind.Store.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Northwind.Store.Data
{
    /// <summary>
    /// Clase base la gestión del acceso a los datos. Incluye métodos básicos para el acceso a los datos.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TK"></typeparam>
    public class BaseRepository<T> : IRepository<T> where T : class, IObjectWithState
    {
        protected readonly NwContext db;

        public BaseRepository(NwContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Aplica todos los cambios que han sido aplicados a una entidad y a todos objetos relacionados con la misma.
        /// </summary>
        /// <param name="model">Instancia del objeto.</param>
        /// <param name="nm">Mensaje de notificación.</param>
        public async Task<int> Save(T model, Notifications nm = null)
        {
            int result = 0;

            try
            {
                var validationErrors = db.ChangeTracker.Entries<IValidatableObject>()
                    .SelectMany(e => e.Entity.Validate(null!))
                    .Where(r => r != ValidationResult.Success).ToList();

                if (validationErrors.Any())
                {
                    // Reportar los mensajes de validación
                    foreach (var ve in validationErrors)
                    {
                        var member = ve.MemberNames.First();
                        nm?.Add(new Message()
                        {
                            Level = Level.Validation,
                            Description = $"La propiedad {member}. Tiene {ve.ErrorMessage}."
                        });
                    }
                }
                else
                {
                    result = await db.ApplyChanges(model);
                }

                return result;
            }
            catch (DbUpdateConcurrencyException dce)
            {
                ManageConcurrency(dce, nm);
            }
            catch (Exception ex)
            {
                if (nm == null)
                {
                    throw;
                }
                else
                {
                    while (ex != null)
                    {
                        var msg = Messages.General.EXCEPTION;
                        msg.Description = ex.Message;
                        nm.Add(msg);

                        ex = ex.InnerException;
                    }
                }
            }

            return result;
        }

        public virtual async Task<T> Get(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = db.Set<T>();

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return db.Set<T>().ToList();
        }

        public virtual async Task<IEnumerable<T>> GetList(int? pageNumber, int pageSize, string includeProperties = "")
        {
            IQueryable<T> query = db.Set<T>();

            query = includeProperties
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            var page = pageNumber ?? 1;

            return await query.AsNoTracking().ToPagedListAsync(page, pageSize);
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await db.Set<T>()
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public virtual async Task<int> Delete(T model)
        {
            // agregar al async en la definición
            //_db.Set<T>().Remove(await _db.FindAsync<T>(key));

            return await db.ApplyChanges(model);
        }

        #region Concurrencia
        /// <summary>
        /// Administración de la excepción de concurrencia al momento de la actualización de datos. Construye una notificación cuando se incluye el parámetro NotificationMessage.
        /// </summary>
        /// <param name="dbe"></param>
        /// <param name="nm">Si el incluye la instancia se agrega los datos relacionados con la excepción.</param>
        protected void ManageConcurrency(DbUpdateConcurrencyException dbe, Notifications nm = null)
        {
            // Si no se incluye el parámetro de notificación se lanza la excepción
            if (nm == null)
            {
                throw dbe;
            }
            else
            {
                if (dbe != null)
                {
                    var entry = dbe.Entries[0];
                    var client = (T)entry.Entity;
                    var serverEntry = entry.GetDatabaseValues();

                    // El problema de concurrencia se da porque los datos fueron eliminados
                    if (serverEntry == null)
                    {
                        ConcurrencyMessage mc = Messages.General.CONCURRENCY_DELETE;
                        mc.Original = client;
                        nm.Add(mc);
                    }
                    // El problema de concurrencia se da porque los datos fueron actualizados
                    else
                    {
                        var server = (T)serverEntry.ToObject();

                        ConcurrencyMessage mc = Messages.General.CONCURRENCY_UPDATE;
                        mc.Original = client;
                        mc.Current = server;
                        nm.Add(mc);
                    }
                }
            }
        }
        #endregion

        #region Dispose
        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
