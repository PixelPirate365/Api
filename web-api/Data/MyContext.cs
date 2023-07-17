using Microsoft.EntityFrameworkCore;
using web_api.Data.Entities;

namespace web_api.Data {
    public class MyContext:DbContext {
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;

        public MyContext(DbContextOptions<MyContext> options) : base(options) {

        }
       
    }
}
