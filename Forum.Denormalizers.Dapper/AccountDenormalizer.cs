using System.Threading.Tasks;
using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    public class AccountDenormalizer : AbstractDenormalizer, IMessageHandler<NewAccountRegisteredEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(NewAccountRegisteredEvent evnt)
        {
            //throw new System.Exception("asds");
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Name = evnt.Name,
                    Password = evnt.Password,
                    CreatedOn = evnt.Timestamp,
                    UpdatedOn = evnt.Timestamp,
                    Sequence = evnt.Sequence,
                    Version = evnt.Version
                }, Constants.AccountTable);
            });
        }
    }
}