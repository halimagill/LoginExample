using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LoginExample.Data
{
    public class UnitOfWork : BaseUnitOfWork<IAppIdentityDbContext>, IUnitOfWork<IAppIdentityDbContext>
    {
        public UnitOfWork(IAppIdentityDbContext context) : base(context)
        {
        }             
    }
 }
   