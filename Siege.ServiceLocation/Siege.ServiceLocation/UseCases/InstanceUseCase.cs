/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using Siege.ServiceLocation.Rules;

namespace Siege.ServiceLocation.UseCases
{
    public abstract class InstanceUseCase<TBaseService> : UseCase<TBaseService, TBaseService>
    {
        protected TBaseService implementation;

        public virtual void BindTo(TBaseService implementation)
        {
            this.implementation = implementation;
        }

        public override TBaseService GetBinding()
        {
            return implementation;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new ImplementationActivationStrategy(implementation);
        }

        protected override IRuleEvaluationStrategy GetRuleEvaluationStrategy()
        {
            return new ContextEvaluationStrategy();
        }

        public override Type GetBoundType()
        {
            return implementation.GetType();
        }

        public override Type GetBaseBindingType()
        {
            return typeof (TBaseService);
        }

        public class ImplementationActivationStrategy : IActivationStrategy
        {
            private readonly TBaseService implementation;

            public ImplementationActivationStrategy(TBaseService implementation)
            {
                this.implementation = implementation;
            }

            public object Resolve(IInstanceResolver locator, IStoreAccessor context)
            {
                //context.ExecutionStore.AddRequestedType(implementation.GetType());
                return implementation;
            }
        }
    }
}