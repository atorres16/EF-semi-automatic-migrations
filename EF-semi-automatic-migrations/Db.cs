using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_semi_automatic_migrations
{
    public class Db : DbContext
    {
        public Db()
        {

        }
        public Db(string connString) : base(connString)
        {
            //for example Data Source=(local)\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=true;
        }
        public DbSet<Thing> Things { get; set; }
    }
}
