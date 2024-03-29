﻿using ListingTracker.DbEntities;
using ListingTracker.LogicClasses;
using NPOI.SS.Formula.Functions;

namespace ListingTracker.Classes
{
    public class PersonViewModel
    {
        public AcceptedPersonViewModel AcceptedPerson { get; set; }
        public List<PersonViewData> PersonList { get; set; }
        public List<AcceptedSourceTrackingVM> SourceTrackers { get; set; }
    }
    public class PersonViewData
    {
        // id
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }

        // first name
        public string FirstName { get; set; }
        // last name
        public string LastName { get; set; }
        // email
        public string Email { get; set; }
        // phone number
        public string PhoneNumber { get; set; }
        // address
        public string Address { get; set; }
        // city
        public string City { get; set; }
        // state
        public string State { get; set; }
        // zip code
        public string ZipCode { get; set; }
        // country
        public string Country { get; set; }
        public bool IsDeleted { get; set; }
        public string FileName { get; set; }
    }
    public class AcceptedPersonViewModel
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        // first name
        public string FirstName { get; set; }
        // last name
        public string LastName { get; set; }
        // email
        public string Email { get; set; }
        // phone number
        public string PhoneNumber { get; set; }
        // address
        public string Address { get; set; }
        // city
        public string City { get; set; }
        // state
        public string State { get; set; }
        // zip code
        public string ZipCode { get; set; }
        // country
        public string Country { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class UpdateDataModel
    {
        public string UpdateData { get; set; }
    }
    public class AcceptedSourceTrackingVM
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public Guid AcceptedPersonId { get; set; }
        public Guid PersonId { get; set; }
    }

}
