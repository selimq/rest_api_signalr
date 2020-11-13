using Microsoft.EntityFrameworkCore;

namespace Login.Repo{
    public class MessageDbContext:DbContext{
        public MessageDbContext(DbContextOptions<MessageDbContext> options) :base(options){}

        public DbSet<ChatMessage> Messages {get;set;}


    }
    
}