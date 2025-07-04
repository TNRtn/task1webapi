﻿namespace task1.DTO
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }
    }
}
