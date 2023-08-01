﻿namespace Pronia.Models.Common
{
    public class BaseSectionEntity :BaseEntity
    {
        public bool IsDeleted { get; set; }

        public  DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
        public  string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

    }
}
