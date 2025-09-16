using Akka.Actor;
using FinanceTracker.Api.Actors.Messages;

namespace FinanceTracker.Api.Actors;

public class FinanceTrackerActor : ReceiveActor
{
    public FinanceTrackerActor()
    {
        ReceiveAsync<ComputeTransactionMessage>(ComputeTransactionAsync);
    }
    
    private Task ComputeTransactionAsync(ComputeTransactionMessage message)
    {
        // Handle the message
        return Task.CompletedTask;
    }
}
