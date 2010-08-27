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

using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.Registrations;
using Siege.Requisitions.Resolution;

namespace Siege.Requisitions.RegistrationTemplates.Default
{
    public class DefaultRegistrationTemplate : AbstractRegistrationTemplate
    {
        public override void Register(IServiceLocatorAdapter adapter, IServiceLocatorStore store, IRegistration registration, IResolutionTemplate template)
        {
            if (registration.GetMappedFromType() != registration.GetMappedToType())
            {
                adapter.RegisterFactoryMethod(registration.GetMappedFromType(), () => template.Resolve(registration.GetMappedFromType()));
                RegisterLazy(adapter, registration.GetMappedFromType(), template);
            }

            adapter.Register(registration.GetMappedToType(), registration.GetMappedToType());
            RegisterLazy(adapter, registration.GetMappedToType(), template);
        }
    }
}