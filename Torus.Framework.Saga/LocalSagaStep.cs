using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Saga
{
    public class LocalSagaStep<TData> : SagaStep<TData> where TData : SagaData
    {
        private Func<TData, Task> _action;
        private Func<TData, Task> _compensation;

        public LocalSagaStep(string name) : base(name)
        {

        }

        public LocalSagaStep() : base()
        {

        }

        public void SetAction(Func<TData, Task> action)
        {
            _action = action;
        }

        //set compensation
        public void SetCompensation(Func<TData, Task> compensation)
        {
            _compensation = compensation;
        }

        public override async Task<IStepOutcome> CompensateAsync(TData data)
        {
            try
            {
                await _compensation?.Invoke(data);
                return StepOutcome.SuccessLocal();
            }catch(Exception ex)
            {
                return StepOutcome.Create(ex);
            }
        }

        public override async Task<IStepOutcome> ExecuteAsync(TData data)
        {
            try
            {
                await _action?.Invoke(data);
                return StepOutcome.SuccessLocal();
            }catch(Exception ex)
            {
                return StepOutcome.Create(ex);
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
            return true;
        }
    }
}
