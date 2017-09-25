using System.Collections.Generic;

namespace MVCBootstrapDemo.Models.DL
{
    public interface ICategory
    {
        List<CategoryModel> SelectAllData(int startIndex, int PageSize, out int TotalCount);
    }
}
