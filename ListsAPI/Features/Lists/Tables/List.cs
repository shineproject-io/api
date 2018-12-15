using ListsAPI.Features.Lists.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ListsAPI.Features.Lists.Tables
{
    public class List
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string BackgroundImageFilePath { get; set; }

        [Required]
        public string BackgroundImageFileName { get; set; }

        [Required]
        public ListState State { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        public int? Position { get; set; }
    }
}