﻿using Shop.SharedKernel;

namespace Shop.Domain.Catalog
{
    public class CategoryBrand : BaseEntity
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
