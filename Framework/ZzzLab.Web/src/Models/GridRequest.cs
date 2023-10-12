using System.ComponentModel.DataAnnotations;

namespace ZzzLab.Web.Models
{
    public class GridRequest : RequestBase
    {
        public uint PageNum { set; get; } = 1;

        public uint PageRow { set; get; } = 20;

        public IEnumerable<GridColumn> Columns { set; get; } = Enumerable.Empty<GridColumn>();

        public bool GetWhereQuery(out string whereQuery, out string orderQuery, out string message)
        {
            whereQuery = string.Empty;
            orderQuery = string.Empty;
            message = string.Empty;

            if (this.Columns != null && this.Columns.Any())
            {
                foreach (var column in this.Columns)
                {
                    // SQL injection
                    if (string.IsNullOrWhiteSpace(column.FieldName)) continue;

                    if (string.IsNullOrWhiteSpace(column.Search) == false)
                    {
                        if (column.Search.Contains('*'))
                        {
                            whereQuery += $" AND {column.FieldName} like '{column.Search.Replace('*', '%')}'";
                        }
                        else whereQuery += $" AND {column.FieldName} = '{column.Search}'";
                    }

                    if (column.OrderBy == OrderBy.Ascending) orderQuery += $" , {column.FieldName} ASC";
                    if (column.OrderBy == OrderBy.Descending) orderQuery += $" , {column.FieldName} DESC";
                }

                if (string.IsNullOrWhiteSpace(orderQuery) == false)
                {
                    orderQuery = $"ORDER BY {orderQuery.TrimStart(',', ' ')}";
                }
            }

            return false;
        }
    }

    public class GridColumn
    {
        [Required]
        public string? Name { set; get; }

        private string? _FieldName = null;

        public string? FieldName
        {
            set => _FieldName = value;
            get => string.IsNullOrWhiteSpace(_FieldName) ? this.Name : _FieldName;
        }

        public string? Search { set; get; }
        public OrderBy OrderBy { set; get; } = OrderBy.None;
    }

    public enum OrderBy
    {
        None,
        Ascending,
        Descending
    }
}