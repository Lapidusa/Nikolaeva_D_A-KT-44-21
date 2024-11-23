﻿using System.Text.RegularExpressions;
namespace project.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public bool isValidGroupName()
        {
            return Regex.IsMatch(GroupName, @"^[\p{Lu}]{2,5}-\d{2}-\d{2}$");
        }
    }
}
