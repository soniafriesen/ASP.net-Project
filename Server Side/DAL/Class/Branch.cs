﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
/*
 Project: Case Study
 Purpose:Class for Branch
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.DAL.DomainClasses
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(150)]
        public string Street { get; set; }

        [StringLength(150)]
        public string City { get; set; }

        [StringLength(2)]
        public string Region { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }
        // distance is a hack to hold output from sproc
        public double? Distance { get; set; }
    }
}