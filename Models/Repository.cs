using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Microsoft.Extensions.Configuration;
using nhibernate_mvc.Settings;
using Microsoft.Extensions.Options;

namespace nhibernate_mvc.Models
{
    public sealed class Repository
    {
        ISessionFactory _sessionFactory;
        ISession _session;

        public string _Conf { get; private set; }
        

        public Repository(string conf)
        {
            _Conf = conf;
            InitializeSession();
        }

        void InitializeSession()
        {
            try
            {
                //var conString = "User ID= xdba; Password= sangram; Server= localhost; Port=5432; Database= sangram; Integrated Security=true; Pooling = true;";
                _sessionFactory = Fluently.Configure()
                    .Database(FluentNHibernate.Cfg.Db.PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(_Conf))
                    .Mappings(m => m
                    .FluentMappings.AddFromAssemblyOf<Repository>())
                    .BuildSessionFactory();
                _session = _sessionFactory.OpenSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public IList<Book> GetAllBooks()
        {
            try
            {
                return _session.QueryOver<Book>().List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Book GetBook(int id)
        {
            try
            {
                return _session.Get<Book>(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Book AddBook(Book Book)
        {
            Book entity = null;
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    entity = _session.Save(Book) as Book;
                    transaction.Commit();
                }
                catch (StaleObjectStateException ex)
                {
                    try
                    {
                        entity = _session.Merge(Book);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                return entity;
            }
        }

        public void UpdateBook(Book Book)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _session.Update(Book);
                    transaction.Commit();
                }
                catch (StaleObjectStateException ex)
                {
                    try
                    {
                        _session.Merge(Book);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }

        public void DeleteBook(int id)
        {
            using (var transaction = _session.BeginTransaction())
            {
                var Book = _session.Get<Book>(id);
                if (Book != null)
                {
                    try
                    {
                        _session.Delete(Book);
                        transaction.Commit();
                    }
                    catch (StaleObjectStateException ex)
                    {
                        try
                        {
                            _session.Merge(Book);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}
