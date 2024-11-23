﻿using System.Text.RegularExpressions;
namespace project.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public bool isValidGroupName()
        {
            return Regex.IsMatch(GroupName, @"^\D+-\d+-\d{2}$");
        }
    }
}
