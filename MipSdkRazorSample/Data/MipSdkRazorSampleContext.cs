using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Data
{
    public class MipSdkRazorSampleContext : DbContext
    {    
        public MipSdkRazorSampleContext (DbContextOptions<MipSdkRazorSampleContext> options)
            : base(options)
        {         
            Database.EnsureCreated();
        }

        public DbSet<MipSdkRazorSample.Models.Employee> Employees { get; set; }

        public DbSet<MipSdkRazorSample.Models.DataPolicy> DataPolicy { get; set; }
    }
}
