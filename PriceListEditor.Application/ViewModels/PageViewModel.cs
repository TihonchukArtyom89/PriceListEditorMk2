using System;

namespace PriceListEditor.Application.ViewModels;

public class PageViewModel
{
    public int PageNumber { get; private set; }
    public int TotalCountOfPages { get; private set; }
    public PageViewModel( int pageNumber, int pageSize, int count )
    {
        PageNumber = pageNumber;
        TotalCountOfPages = (int)Math.Ceiling(count / (double)pageSize);
    }
    public bool HasPreviousPage
    {
        get
        {
            return PageNumber > 1;
        }
    }
    public bool HasNextPage
    {
        get
        {
            return PageNumber < TotalCountOfPages;
        }
    }
}
