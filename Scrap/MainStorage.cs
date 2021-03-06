﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Scrap.Core;
using Scrap.Core.Classes;
using Scrap.Core.Classes.References;
using Scrap.Core.Classes.Service;
using Scrap.Core.Enums;
using Scrap.Domain.Repositories;
using Scrap.Domain.Repositories.Documents;
using Scrap.Domain.Repositories.References;
using Scrap.Domain.Repositories.Service;
using Scrap.Tools;

namespace Scrap
{
    public sealed partial class MainStorage : IDisposable, IModelContext
    {
        public const string AppName = "Учёт лома";

        #region Поля

        private readonly ObservableCollection<Nomenclature> _nomenclatures = new ObservableCollection<Nomenclature>();

        private readonly ObservableCollection<Organization> _contractors = new ObservableCollection<Organization>();

        private readonly ObservableCollection<Organization> _bases = new ObservableCollection<Organization>();

        private readonly ObservableCollection<Employee> _responsiblePersons = new ObservableCollection<Employee>();

        private readonly ObservableCollection<Transport> _transports = new ObservableCollection<Transport>();

        private readonly ObservableCollection<Employee> _drivers = new ObservableCollection<Employee>();

        private readonly ObservableCollection<Template> _templates = new ObservableCollection<Template>();

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        static MainStorage()
        {
            Instance = new MainStorage();
        }

        /// <summary>
        /// Приватный конструктор экземпляра
        /// </summary>
        private MainStorage()
        {
            Nomenclatures = new ReadOnlyObservableCollection<Nomenclature>(_nomenclatures);
            Bases = new ReadOnlyObservableCollection<Organization>(_bases);
            Contractors = new ReadOnlyObservableCollection<Organization>(_contractors);
            ResponsiblePersons = new ReadOnlyObservableCollection<Employee>(_responsiblePersons);
            Transports = new ReadOnlyObservableCollection<Transport>(_transports);
            Drivers = new ReadOnlyObservableCollection<Employee>(_drivers);

            Templates = new ReadOnlyObservableCollection<Template>(_templates);
        }

        #region Свойства

        /// <summary>
        /// Статическая ссылка на экземпляр класса
        /// </summary>
        public static MainStorage Instance { get; private set; }

        /// <summary>
        /// Номенклатура
        /// </summary>
        public ReadOnlyObservableCollection<Nomenclature> Nomenclatures { get; private set; }

        /// <summary>
        /// Контрагенты
        /// </summary>
        public ReadOnlyObservableCollection<Organization> Contractors { get; private set; }

        /// <summary>
        /// Базы
        /// </summary>
        public ReadOnlyObservableCollection<Organization> Bases { get; private set; }

        /// <summary>
        /// Ответственные лица
        /// </summary>
        public ReadOnlyObservableCollection<Employee> ResponsiblePersons { get; private set; }

        /// <summary>
        /// Автотранспорт
        /// </summary>
        public ReadOnlyObservableCollection<Transport> Transports { get; private set; }

        /// <summary>
        /// Шаблоны
        /// </summary>
        public ReadOnlyObservableCollection<Template> Templates { get; private set; }

        /// <summary>
        /// Водители
        /// </summary>
        public ReadOnlyObservableCollection<Employee> Drivers { get; private set; }

        public UsersRepository UsersRepository { get; private set; }

        public NomenclatureRepository NomenclaturesRepository { get; private set; }

        public BasesRepository BasesRepository { get; private set; }

        public ContractorsRepository ContractorsRepository { get; private set; }

        public ResponsiblePersonsRepository ResponsiblePersonsRepository { get; private set; }

        public TransportsRepository TransportsRepository { get; private set; }

        public DriversRepository DriversRepository { get; private set; }

        public JournalRepository JournalRepository { get; private set; }

        public TransportationRepository TransportationRepository { get; private set; }

        public ProcessingRepository ProcessingRepository { get; private set; }

        public RemainsRepository RemainsRepository { get; private set; }

        public TemplatesRepository TemplatesRepository { get; private set; }

        public ReportsRepository ReportsRepository { get; private set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        #endregion

        #region Методы

        /// <summary>
        /// Инициализация
        /// </summary>
        internal void Initialize()
        {
            // Загрузка настроек из профиля пользователя
            LoadSettings();

            // Создание репозитариев
            UsersRepository = new UsersRepository(this);
            NomenclaturesRepository = new NomenclatureRepository(this);
            BasesRepository = new BasesRepository(this);
            ContractorsRepository = new ContractorsRepository(this);
            ResponsiblePersonsRepository = new ResponsiblePersonsRepository(this);
            TransportsRepository = new TransportsRepository(this);
            DriversRepository = new DriversRepository(this);
            JournalRepository = new JournalRepository();
            TransportationRepository = new TransportationRepository(this);
            ProcessingRepository = new ProcessingRepository(this);
            RemainsRepository = new RemainsRepository(this);
            TemplatesRepository = new TemplatesRepository(this);
            ReportsRepository = new ReportsRepository();

            // Загрузка данных справочников из репозитариев
            _nomenclatures.AddRange(NomenclaturesRepository.GetAll());
            _contractors.AddRange(ContractorsRepository.GetAll());
            _bases.AddRange(BasesRepository.GetAll());
            _responsiblePersons.AddRange(ResponsiblePersonsRepository.GetAll());
            _transports.AddRange(TransportsRepository.GetAll());
            _drivers.AddRange(DriversRepository.GetAll());

            _templates.AddRange(TemplatesRepository.GetAll());
        }

        /// <summary>
        /// Очистка перед закрытием приложения
        /// </summary>
        public void Dispose()
        {
            SaveSettings();
        }

        /// <summary>
        /// Создание или обновление объекта в базе
        /// </summary>
        /// <param name="o"></param>
        public void CreateOrUpdateObject(object o)
        {
            if (o == null)
                throw new ArgumentNullException("o");

            if (o is Nomenclature)
            {
                CreateOrUpdateNomenclature(o as Nomenclature);
                return;
            }

            if (o is Organization)
            {
                CreateOrUpdateOrganization(o as Organization);
                return;
            }

            if (o is Employee)
            {
                CreateOrUpdateEmployee(o as Employee);
                return;
            }

            if (o is Transport)
            {
                CreateOrUpdateTransport(o as Transport);
                return;
            }

            if (o is Template)
            {
                CreateOrUpdateTemplate(o as Template);
                return;
            }

            if (o is User)
            {
                CreateOrUpdateUser(o as User);
                return;
            }
        }

        private void CreateOrUpdateTemplate(Template template)
        {
            if (Templates.Any(x => x.Id == template.Id))
            {
                TemplatesRepository.Update(template);
            }
            else
            {
                TemplatesRepository.Create(template);
                _templates.Add(template);
            }
        }

        private void CreateOrUpdateUser(User user)
        {
            UsersRepository.CreateOrUpdate(user);
        }

        private void CreateOrUpdateNomenclature(Nomenclature nomenclature)
        {
            if (Nomenclatures.Any(x => x.Id == nomenclature.Id))
            {
                NomenclaturesRepository.Update(nomenclature);
            }
            else
            {
                NomenclaturesRepository.Create(nomenclature);
                _nomenclatures.Add(nomenclature);
            }
        }

        private void CreateOrUpdateOrganization(Organization organization)
        {
            switch (organization.Type)
            {
                case OrganizationType.Contractor:
                    if (Contractors.Any(x => x.Id == organization.Id))
                    {
                        ContractorsRepository.Update(organization);
                    }
                    else
                    {
                        ContractorsRepository.Create(organization);
                        _contractors.Add(organization);
                    }
                    break;
                case OrganizationType.Base:
                    if (_bases.Any(x => x.Id == organization.Id))
                    {
                        BasesRepository.Update(organization);
                    }
                    else
                    {
                        BasesRepository.Create(organization);
                        _bases.Add(organization);
                    }
                    break;
            }
        }

        private void CreateOrUpdateEmployee(Employee employee)
        {
            switch (employee.Type)
            {
                case EmployeeType.Responsible:
                    if (ResponsiblePersons.Any(x => x.Id == employee.Id))
                        ResponsiblePersonsRepository.Update(employee);
                    else
                    {
                        ResponsiblePersonsRepository.Create(employee);
                        _responsiblePersons.Add(employee);
                    }
                    break;
                case EmployeeType.Driver:
                    if (Drivers.Any(x => x.Id == employee.Id))
                        DriversRepository.Update(employee);
                    else
                    {
                        DriversRepository.Create(employee);
                        _drivers.Add(employee);
                    }
                    break;
            }
        }

        private void CreateOrUpdateTransport(Transport transport)
        {
            if (Transports.Any(x => x.Id == transport.Id))
                TransportsRepository.Update(transport);
            else
            {
                TransportsRepository.Create(transport);
                _transports.Add(transport);
            }
        }

        /// <summary>
        /// Удаление объекта в базе
        /// </summary>
        /// <param name="o"></param>
        public void DeleteObject(object o)
        {
            // Номенклатура
            if (o is Nomenclature)
            {
                Nomenclature nomenclature = o as Nomenclature;
                NomenclaturesRepository.Delete(nomenclature.Id);
                _nomenclatures.Remove(nomenclature);
                return;
            }

            // Организация
            if (o is Organization)
            {
                Organization organization = o as Organization;
                switch (organization.Type)
                {
                    case OrganizationType.Contractor:
                        ContractorsRepository.Delete(organization.Id);
                        _contractors.Remove(organization);
                        break;
                    case OrganizationType.Base:
                        BasesRepository.Delete(organization.Id);
                        _bases.Remove(organization);
                        break;
                }
                return;
            }

            // Сотрудник
            if (o is Employee)
            {
                Employee employee = o as Employee;
                switch (employee.Type)
                {
                    case EmployeeType.Responsible:
                        ResponsiblePersonsRepository.Delete(employee.Id);
                        _responsiblePersons.Remove(employee);
                        break;
                    case EmployeeType.Driver:
                        DriversRepository.Delete(employee.Id);
                        _drivers.Remove(employee);
                        break;
                }
                return;
            }

            // Транспорт
            if (o is Transport)
            {
                Transport transport = o as Transport;
                TransportsRepository.Delete(transport.Id);
                _transports.Remove(transport);
                return;
            }

            // Шаблон
            if (o is Template)
            {
                Template template = o as Template;
                TemplatesRepository.Delete(template);
                _templates.Remove(template);
                return;
            }

            // Пользователь
            if (o is User)
            {
                UsersRepository.Delete(o as User);
                return;
            }
        }

        #endregion
    }
}
