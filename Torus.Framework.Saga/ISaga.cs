using System.Threading.Tasks;

namespace Torus.Framework.Saga
{
    public interface ISaga<TData> where TData : SagaData
    {
        Task<IStepOutcome> HandleReply(SagaReplyEnvelop sagaReplyEnvelop, SagaInstance sagaInstance);
        void AddStep(ISagaStep<TData> step);

        string SerializeSagaData(TData data);

        TData DeserializeSagaData(string data);

        Task<IStepOutcome> StartAsync(SagaInstance sagaInstance);
        SagaInstance NewInstance(TData data);
        string GetSagaType();
        string GetReplyChannel();
    }
}
