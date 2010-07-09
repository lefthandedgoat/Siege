﻿/*   Copyright 2009 - 2010 Marcus Bratton

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

using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Policies
{
    public class Singleton : AbstractRegistrationPolicy
    {
        private readonly object lockObject = new object();
        private object instance;

        public Singleton(IUseCase useCase) : base(useCase)
        {

        }

        public override object Resolve(IResolutionStrategy strategy, IServiceLocatorStore accessor)
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if(instance == null)
                    {
                        instance = useCase.Resolve(strategy, accessor);
                    }
                }
            }

            return instance;
        }
    }
}