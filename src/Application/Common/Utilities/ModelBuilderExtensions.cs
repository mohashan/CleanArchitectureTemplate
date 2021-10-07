using Microsoft.EntityFrameworkCore;
using Pluralize.NET.Core;
using System;
using System.Linq;
using System.Reflection;

namespace Application.Common.Utilities
{
    public static class ModelBuilderExtensions
    {
        public static void RegisterAllEntities<TBaseType>(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(TBaseType).IsAssignableFrom(c));

            foreach (var type in types)
            {
                modelBuilder.Entity(type);
            }
        }

        public static void RegisterEntityTypeConfiguration(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var applyGenericMethod = typeof(ModelBuilder).GetMethods().First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

            var types = assembly.GetExportedTypes()
                .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic);

            foreach (var type in types)
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(modelBuilder, new[] { Activator.CreateInstance(type) });
                    }
                }
            }
        }

        public static void AddPluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            var pluralizer = new Pluralizer();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                entityType.SetTableName(pluralizer.Pluralize(tableName));
            }
        }

        public static void AddSequentialGuidForIdConvention(this ModelBuilder modelBuilder)
        {
            modelBuilder.AddDefaultValueSqlConvention("Id", typeof(Guid), "NEWSEQUENTIALID()");
        }

        public static void AddDefaultValueSqlConvention(this ModelBuilder modelBuilder, string propertyName, Type propertyType, string defaultValueSql)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var property = entityType.GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
                if (property != null && property.ClrType == propertyType)
                {
                    property.SetDefaultValueSql(defaultValueSql);
                }
            }
        }

        public static void AddRestrictDeleteBehaviorConvention(this ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}