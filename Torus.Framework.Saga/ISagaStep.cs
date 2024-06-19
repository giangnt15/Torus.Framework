using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Saga
{
    public interface ISagaStep<TData> where TData : SagaData
    {
        Task<IStepOutcome> ExecuteAsync(TData data);
        Task<IStepOutcome> CompensateAsync(TData data);
        void ExcuteReplyHandler(string replyTypeName, int state, string stateName, string replyMessage, TData data);
        bool HasCompensation();
        bool HasAction();
        string GetStateName();
        bool IsSuccessfullReply(string commandOutcome);

        void AddReplyHanlder<TReply>(Action<TReply, int, string, TData> action) where TReply : SagaReplyMessage;
        void SetStateName(string stateName);
        bool IsLocal();
    }
}
