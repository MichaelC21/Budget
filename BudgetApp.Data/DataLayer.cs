using BudgetApp.Business.Interfaces;
using BudgetApp.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetApp.Data
{
    public static class DataLayer
    {
        public static void AddDataLayer(this IServiceCollection services)
        {
            services.AddTransient<IMoneyService, MoneyService>();
        }
    }
}
