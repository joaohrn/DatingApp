using System;

namespace Api.Helpers;

public class PaginationParams
{
    private const int MaxPageSize = 49;
    public int pageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
