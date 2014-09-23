﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zlatmet2.Domain.Entities.References
{
    [Table("ReferenceOrganizations")]
    internal class OrganizationEntity
    {
        public Guid Id { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Inn { get; set; }

        public string Bik { get; set; }

        public string Bank { get; set; }

        public string Contract { get; set; }
    }
}
