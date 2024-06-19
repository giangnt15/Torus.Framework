using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Saga
{
    public class SagaBuilder<TData> where TData : SagaData
    {
        private readonly ISaga<TData> _saga;
        private readonly SagaStepBuilder<TData> _stepBuilder;
        public SagaBuilder(ISaga<TData> saga)
        {
            _saga = saga;
            _stepBuilder = new SagaStepBuilder<TData>(_saga);
        }

        public SagaStepBuilder<TData> Step()
        {
            return _stepBuilder;
        }

    }
}
