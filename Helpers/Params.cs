﻿namespace MedicineStorage.Helpers
{
    public class Params
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;
        public int _pagesize = 10;

        public int PageSize
        {
            get => _pagesize;
            set => _pagesize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }
}
