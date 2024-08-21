using System;

namespace PriceListEditor.Application.ViewModels;

public class PageViewModel
{
    public int NumberOfPages { get; private set; }
    public int TotalCountOfPages { get; private set; }
    public PageViewModel( int pageNumber, int pageSize, int? count=0)
    {
        NumberOfPages = pageNumber;
        TotalCountOfPages = (int)Math.Ceiling((double)(count! / (double)pageSize));
    }
    public bool HasPreviousPage
    {
        get
        {
            return NumberOfPages > 1;
        }
    }
    public bool HasNextPage
    {
        get
        {
            return NumberOfPages < TotalCountOfPages;
        }
    }
}
