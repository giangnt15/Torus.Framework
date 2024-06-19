using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Commands;

namespace Torus.Framework.Saga
{
    public class RemoteSagaStep<TData> : SagaStep<TData> where TData : SagaData
    {
        private Func<TData, CommandEnvelop> _action;
        private Func<TData, CommandEnvelop> _compensation;
        public RemoteSagaStep(string name) : base(name)
        {
        }

        public RemoteSagaStep() : base()
        {

        }


        public void SetAction(Func<TData, CommandEnvelop> action)
        {
            _action = action;
        }

        public void SetCompensation(Func<TData, CommandEnvelop> compensation)
        {
            _compensation = compensation;
        }

        public override Task<IStepOutcome> CompensateAsync(TData data)
        {
            try
            {
                var command = _compensation?.Invoke(data);
                return Task.FromResult<IStepOutcome>(StepOutcome.Create(command));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IStepOutcome>(StepOutcome.Create(ex));
            }
        }

        public override Task<IStepOutcome> ExecuteAsync(TData data)
        {
            try
            {
                var command = _action?.Invoke(data);
                return Task.FromResult<IStepOutcome>(StepOutcome.Create(command));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IStepOutcome>(StepOutcome.Create(ex));
            }
        }

        public override bool HasAction()
        {
            return _action != null;
        }

        public override bool HasCompensation()
        {
            return _compensation != null;
        }

        public override bool IsLocal()
        {
            return false;
        }
    }
}
