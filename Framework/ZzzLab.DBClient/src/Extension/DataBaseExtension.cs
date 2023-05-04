using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ZzzLab.Data
{
    public static partial class DataBaseExtension
    {
        public static bool IsNull(DataTable datatable) => (datatable == null || datatable.Rows.Count == 0);

        public static DataTable Merge(this DataTable table, DataTable appendTable)
        {
            foreach (DataColumn column in appendTable.Columns)
            {
                if (table.Columns.Contains(column.ColumnName)) continue;

                table.Columns.Add(column.ColumnName);
            }

            foreach (DataRow row in appendTable.Rows)
            {
                DataRow addRow = table.NewRow();
                foreach (DataColumn column in appendTable.Columns)
                {
                    addRow[column.ColumnName] = row[column.ColumnName];
                }

                table.Rows.Add(addRow);
            }

            return table;
        }

        private static bool IsDBNullableType(Type type) => (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));

        public static string[] GetNames(this DataColumnCollection columns)
        {
            List<string> list = new List<string>();

            foreach (DataColumn column in columns)
            {
                list.Add(column.ColumnName);
            }
            return list.ToArray();
        }

        public static QueryParameterCollection MakeDBParamFromClass(this object obj)
        {
            if (obj == null) return QueryParameterCollection.Create();

            QueryParameterCollection results = QueryParameterCollection.Create();

            Type type = obj.GetType();
            if (type.IsClass == true)
            {
                PropertyInfo[] propertyInfos = type.GetProperties();

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    try
                    {
                        string key = propertyInfo.Name;
                        object value = propertyInfo.GetValue(obj, null);

                        if (value == null) value = DBNull.Value;
                        else if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            if (((DateTime)value).Year == 1) value = null;
                        }
                        else if (propertyInfo.PropertyType == typeof(decimal))
                        {
                            // P.K or F.K는 0이 들어 가지 않음.
                            // 이름가지고 걸러보자.
                            if (key.ToLower().EndsWith("_no") || key.ToLower().EndsWith("_idx"))
                            {
                                if ((decimal)value == 0) value = null;
                            }
                        }
                        else if (propertyInfo.PropertyType == typeof(int))
                        {
                            // P.K or F.K는 0이 들어 가지 않음.
                            // 이름가지고 걸러보자.
                            if (key.ToLower().EndsWith("_no") || key.ToLower().EndsWith("_idx"))
                            {
                                if ((int)value == 0) value = null;
                            }
                        }

                        results.Add(key, value);
                    }
                    catch (Exception) { }
                    finally { }
                }
            }

            return results;
        }

        public static void FillItem(this DataRow row, object cls)
        {
            Type type = cls.GetType();
            if (type.IsClass == true)
            {
                PropertyInfo[] propertyInfos = type.GetProperties();

                Hashtable columnNames = new Hashtable();

                foreach (DataColumn column in row.Table.Columns)
                {
                    columnNames.Add(column.ColumnName.ToUpper(), column.ColumnName);
                }

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    try
                    {
                        string propertyInfoName = propertyInfo.Name.ToUpper();
                        string columnName = null;

                        if (columnNames.Contains(propertyInfoName)) columnName = columnNames[propertyInfoName]?.ToString();

                        if (string.IsNullOrEmpty(columnName) == false)
                        {
                            object objValue = row[columnName];
                            if (objValue != null && objValue.GetType() == typeof(DBNull))
                            {
                                if (propertyInfo.PropertyType.FullName == typeof(System.Decimal).ToString()) objValue = 0;
                                else if (propertyInfo.PropertyType.FullName == typeof(System.DateTime).ToString()) objValue = null;
                            }

                            if (IsDBNullableType(propertyInfo.PropertyType) == false)
                            {
                                if (propertyInfo.PropertyType.FullName == typeof(System.Boolean).ToString())
                                {
                                    propertyInfo.SetValue(cls, Convert.ChangeType(objValue != null && (bool)objValue, propertyInfo.PropertyType), null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(cls, Convert.ChangeType(objValue, propertyInfo.PropertyType), null);
                                }
                            }
                            else
                            {
                                Type propertyType = propertyInfo.PropertyType.GetGenericArguments()[0];
                                propertyInfo.SetValue(cls, Convert.ChangeType(objValue, propertyType), null);
                            }
                        }
                    }
                    catch (Exception) { }
                    finally { }
                }
            }
        }

        public static bool CheckInjection(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value.Trim().StartsWithIgnoreCaseOr("select", "insert", "update", "merge")) return true;

            return false;
        }

        public static Type ToConnectionType(this DataBaseType serverType)
        {
            switch (serverType)
            {
                case DataBaseType.PostgreSQL: return typeof(Npgsql.NpgsqlConnection);
                case DataBaseType.MSSql: return typeof(System.Data.SqlClient.SqlConnection);
                case DataBaseType.Oracle: return typeof(Oracle.ManagedDataAccess.Client.OracleConnection);
                default: throw new NotSupportedException();
            }
        }
    }
}