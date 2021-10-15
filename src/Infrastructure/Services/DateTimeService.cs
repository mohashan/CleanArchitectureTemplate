using Application.Common.Interfaces;
using Application.Common.ServiceLifetimes;
using System;

namespace Infrastructure.Services
{
    public class DateTimeService : IDateTimeService, IScopedDependency
    {
        public DateTime Now => DateTime.Now;
    }
}