﻿namespace Finance.Models
{
    public class ATransaction
    {
        public int Id { get; set; }
        public float Value { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsReoccuring { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Category Budget { get; set; }

        public ATransaction()
        {
            Description = string.Empty;
            TimeStamp = DateTime.Now;
            IsReoccuring = false;
            Title = string.Empty;
            Budget = Category.None;
            Value = 0;
        }
    }

}