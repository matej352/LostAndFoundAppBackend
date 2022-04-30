﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EF.Model
{
    public partial class Item
    {
        public Item()
        {
            Category = new HashSet<Category>();
        }

        public int ItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? FindingDate { get; set; }
        public DateTime? LossDate { get; set; }
        public int AdvertisementId { get; set; }

        public virtual Advertisement Advertisement { get; set; }
        public virtual ICollection<Category> Category { get; set; }
    }
}