using ListsAPI.Features.TodoItems.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ListsAPI.Features.TodoItems.Tables
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public int ListId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public TodoItemState State { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        public int? Position { get; set; }
    }
}