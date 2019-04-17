﻿using System;

using App2.Models;

namespace App2.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Title;
            Item = item;
        }
    }
}