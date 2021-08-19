
using Inside.TestTask.MC1.DomainModel;
using Inside.TestTask.Model;

namespace Inside.TestTask.MC1.Services;
public class MariadbDataLayer : IDataLayer
{
    private readonly IConfiguration _config;
    private readonly string _connStr;
    public MariadbDataLayer(IConfiguration configuration)
    {
        _config = configuration;
        _connStr = _config.GetConnectionString("DefaultConnection");
    }
    public async Task<TestTask.Model.Message> AddNewMessage(TestTask.Model.Message msg)
    {
        using var dbContext = new MariaDbContext(_connStr);
        var domainMsg = new DomainModel.Message()
        {
            Session_Id = msg.Session_id,
            MC1_timestamp = msg.MC1_timestamp
        };
        dbContext.Messages.Add(domainMsg);
        await dbContext.SaveChangesAsync();
        msg.Id = domainMsg.Id;
        return msg;
    }

    public async Task<int> StartNewSession()
    {
        using MariaDbContext dbContext = new MariaDbContext(_connStr);
        var session = new Session();
        dbContext.Sessions.Add(session);
        await dbContext.SaveChangesAsync();
        return session.Id;
    }

    public async Task UpdateMessage(TestTask.Model.Message msg)
    {
        using MariaDbContext dbContext = new MariaDbContext(_connStr);
        var storedMsg = dbContext.Messages.Where(m => m.Id == msg.Id).FirstOrDefault();
        if (storedMsg != null)
        {
            storedMsg.MC2_timestamp = msg.MC2_timestamp;
            storedMsg.MC3_timestamp = msg.MC3_timestamp;
            storedMsg.End_timestamp = msg.End_timestamp;
        }
        await dbContext.SaveChangesAsync();
    }
}
