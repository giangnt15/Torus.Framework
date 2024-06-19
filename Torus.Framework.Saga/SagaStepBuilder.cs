using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Commands;

namespace Torus.Framework.Saga
{
    public class SagaStepBuilder<TData> where TData : SagaData
    {
        protected readonly ISaga<TData> _parent;
        protected IActualSagaStepBuilder<TData> _sagaStepBuilder;
        private string _tempStateName;

        public SagaStepBuilder(ISaga<TData> parent)
        {
            _parent = parent;
        }

        public virtual SagaStepBuilder<TData> Step()
        {
            return new SagaStepBuilder<TData>(_parent);
        }

        public LocalSagaStepBuilder<TData> InvokeLocal(Func<TData, Task> action)
        {
            var localSagaStepBuilder = new LocalSagaStepBuilder<TData>(_parent);
            _parent.AddStep(localSagaStepBuilder.GetStep());
            localSagaStepBuilder.WithStateName(_tempStateName);
            _tempStateName = null;
            localSagaStepBuilder.WithAction(action);
            _sagaStepBuilder = localSagaStepBuilder;
            return localSagaStepBuilder;
        }

        public RemoteSagaStepBuilder<TData> InvokeParticipant(Func<TData, CommandEnvelop> action)
        {
            var remoteSagaStepBuilder = new RemoteSagaStepBuilder<TData>(_parent);
            _parent.AddStep(remoteSagaStepBuilder.GetStep());
            remoteSagaStepBuilder.WithStateName(_tempStateName);
            _tempStateName = null;
            remoteSagaStepBuilder.WithAction(action);
            _sagaStepBuilder = remoteSagaStepBuilder;
            return remoteSagaStepBuilder;
        }

        public RemoteSagaStepBuilder<TData> WithCompensation(Func<TData, CommandEnvelop> action)
        {
            var remoteSagaStepBuilder = new RemoteSagaStepBuilder<TData>(_parent);
            _parent.AddStep(remoteSagaStepBuilder.GetStep());
            remoteSagaStepBuilder.WithStateName(_tempStateName);
            _tempStateName = null;
            remoteSagaStepBuilder.WithCompensation(action);
            _sagaStepBuilder = remoteSagaStepBuilder;
            return remoteSagaStepBuilder;
        }

        public SagaStepBuilder<TData> WithStateName(string stateName)
        {
            _tempStateName = stateName;
            return this;
        }

    }

    public class LocalSagaStepBuilder<TData> : AbtractSagaStepBuilder<TData> where TData : SagaData
    {
        private readonly LocalSagaStep<TData> _step;

        public LocalSagaStepBuilder(ISaga<TData> parent) : base(parent)
        {
            _step = new LocalSagaStep<TData>();
        }

        public override ISagaStep<TData> GetStep()
        {
            return _step;
        }

        public LocalSagaStepBuilder<TData> WithAction(Func<TData, Task> action)
        {
            _step.SetAction(action);
            return this;
        }

        public LocalSagaStepBuilder<TData> WithCompensation(Func<TData, Task> compensation)
        {
            _step.SetCompensation(compensation);
            return this;
        }

        public LocalSagaStepBuilder<TData> WithStateName(string stateName)
        {
            _step.SetStateName(stateName);
            return this;
        }
    }

    public class RemoteSagaStepBuilder<TData> : AbtractSagaStepBuilder<TData> where TData : SagaData
    {
        private readonly RemoteSagaStep<TData> _step;

        public RemoteSagaStepBuilder(ISaga<TData> parent) : base(parent)
        {
            _step = new RemoteSagaStep<TData>();
        }

        public RemoteSagaStepBuilder<TData> WithCompensation(Func<TData, CommandEnvelop> compensation)
        {
            _step.SetCompensation(compensation);
            return this;
        }

        public RemoteSagaStepBuilder<TData> WithAction(Func<TData, CommandEnvelop> action)
        {
            _step.SetAction(action);
            return this;
        }

        public RemoteSagaStepBuilder<TData> OnReply<TReply>(Action<TReply, int, string, TData> action) where TReply : SagaReplyMessage
        {
            _step.AddReplyHanlder(action);
            return this;
        }

        public override ISagaStep<TData> GetStep()
        {
            return _step;
        }

        public RemoteSagaStepBuilder<TData> WithStateName(string stateName)
        {
            _step.SetStateName(stateName);
            return this;
        }


    }

    public interface IActualSagaStepBuilder<TData> where TData : SagaData
    {
        SagaStepBuilder<TData> Step();

        ISagaStep<TData> GetStep();
        ISaga<TData> Build();
    }

    public abstract class AbtractSagaStepBuilder<TData> : IActualSagaStepBuilder<TData> where TData : SagaData
    {
        protected readonly ISaga<TData> _parent;

        public AbtractSagaStepBuilder(ISaga<TData> parent)
        {
            _parent = parent;
        }

        public ISaga<TData> Build()
        {
            return _parent;
        }

        public SagaStepBuilder<TData> Step()
        {
            return new SagaStepBuilder<TData>(_parent);
        }

        public abstract ISagaStep<TData> GetStep();
    }

}
