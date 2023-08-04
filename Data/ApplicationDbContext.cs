using Learning.CoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Learning.CoreApi.Data {

    // dbContext that will manage all the entities in our application
    public class ApplicationDbContext : DbContext {
        // we need to create individual db set inside the dbcontext
        public DbSet<Villa> Villa { get; set; }
    }
}
