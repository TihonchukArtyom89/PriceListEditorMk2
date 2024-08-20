using System;

namespace PriceListEditor.Application.ViewModels;

public class PageViewModel
{
    public int NumberOfPages { get; private set; }
    public int TotalCountOfPages { get; private set; }
    public PageViewModel(int count, int pageNumber, int pageSize)
    {
        NumberOfPages = pageNumber;
        TotalCountOfPages = (int)Math.Ceiling(count / (double)pageSize);
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
