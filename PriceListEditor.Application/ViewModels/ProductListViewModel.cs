﻿using PriceListEditor.Application.Models;
using System.Collections.Generic;

namespace PriceListEditor.Application.ViewModels;

public class ProductListViewModel
{
    public IEnumerable<Product>? Products { get; set; }
    public PageViewModel? PageViewModel { get; set; }
    public string? CurrentCategory { get; set; }
    public long? CurrentCategoryId { get; set; }
}
