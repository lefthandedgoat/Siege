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

using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using Siege.Provisions.NHibernate;
using Siege.Provisions.UnitOfWork;

namespace Siege.Provisions.Tests
{
    [TestFixture]
    public class NHibernateRepositoryTests
    {
        private MockRepository mocks;
        private IUnitOfWorkStore store;
        private NHibernateUnitOfWorkFactory factory;
        private ISessionFactory sessionFactory;
        private ISession session;
        private NHibernateUnitOfWorkManager unitOfWorkManager;
        private NHibernateRepository<NullDatabase> repository;
        private ITransaction transaction;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            session = mocks.DynamicMock<ISession>();
            sessionFactory = mocks.DynamicMock<ISessionFactory>();
            store = new ThreadedUnitOfWorkStore();
            transaction = mocks.DynamicMock<ITransaction>();
            factory = mocks.Stub<NHibernateUnitOfWorkFactory>(sessionFactory);
            unitOfWorkManager = mocks.Stub<NHibernateUnitOfWorkManager>();
            unitOfWorkManager.Add(new NullDatabase(factory));
            repository = new NHibernateRepository<NullDatabase>(unitOfWorkManager);
        }

        [Test]
        public void ShouldCallUnitOfWorkManagerCurrentOnGet()
        {
            using (mocks.Record())
            {
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                session.Expect(s => s.BeginTransaction()).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(false).Repeat.Any();
                sessionFactory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                unitOfWorkManager.Expect(uow => uow.For<NullDatabase>());
            }

            using (mocks.Playback())
            {
                repository.Get<object>(1);
            }
        }

        [Test]
        public void ShouldCallUnitOfWorkManagerCurrentOnSave()
        {
            using (mocks.Record())
            {
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                session.Expect(s => s.BeginTransaction()).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(false).Repeat.Any();
                sessionFactory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                unitOfWorkManager.Expect(uow => uow.For<NullDatabase>());
            }

            using (mocks.Playback())
            {
                repository.Save<object>(1);
            }
        }

        [Test]
        public void ShouldCallUnitOfWorkManagerCurrentOnDelete()
        {
            using (mocks.Record())
            {
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                session.Expect(s => s.BeginTransaction()).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(false).Repeat.Any();
                sessionFactory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                unitOfWorkManager.Expect(uow => uow.For<NullDatabase>());
            }

            using (mocks.Playback())
            {
                repository.Delete<object>(1);
            }
        }

        [Test]
        public void ShouldCallSessionWhenRequested()
        {
            using (mocks.Record())
            {
                sessionFactory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
            }

            using (mocks.Playback())
            {
                var session = repository.Session;
            }
        }

        [TearDown]
        public void TearDown()
        {
            mocks.BackToRecordAll();

            using (mocks.Record())
            {
                session.Expect(s => s.Close()).Return(null).Repeat.Any();
            }
            using (mocks.Playback())
            {
                store.Dispose();
            }
        }
    }
}