
using Inside.TestTask.Model;

namespace Inside.TestTask.MC1.Services;
public interface IDataLayer
{
    public Task<int> StartNewSession();

    public Task<Message> AddNewMessage(Message msg);

    public Task UpdateMessage(Message msg);



}
