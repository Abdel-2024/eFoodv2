﻿namespace eFood.Domain.Entities
{
    public class Pagination
    {
        public int Page { get; set; } = -1;
        public int PageSize { get; set; } = 10;
    }
}