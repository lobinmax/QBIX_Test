using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

using System;
using System.Data.Linq;
using System.Linq;

namespace QBIX_Test
{
    public enum DialogFormType
    {
        [Helpers.StringValueAttribute("Добавить")] AddForm,
        [Helpers.StringValueAttribute("Редактировать")] EditForm
    }

    public static class Helpers
    {
        public class StringValueAttribute : Attribute
        {
            public string StringValue { get; protected set; }
            public StringValueAttribute(string value)
            {
                this.StringValue = value;
            }
        }

        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();

            var fieldInfo = type.GetField(value.ToString());

            var attribs = (StringValueAttribute[])fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false);

            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public static T? CustomValue<T>(this object editValue) where T : struct
        {
            if (editValue == null || editValue == DBNull.Value)
            {
                return null;
            }

            if (typeof(T) == typeof(int))
            {
                var vInt = int.Parse(editValue.ToString());
                return vInt as T?;
            }

            return (T?)editValue;
        }

        public static T CustomValueNn<T>(this object editValue) where T : struct
        {
            return editValue?.CustomValue<T>() ?? throw new ArgumentNullException("");
        }

        public static T CustomValueNn<T>(this object editValue, T valueIfNull) where T : struct
        {
            return editValue?.CustomValue<T>() ?? valueIfNull;
        }

        public static string CustomValue(this object editValue)
        {
            if (editValue == null || ((string)editValue).IsNullOrEmptyOrWhiteSpace())
            {
                return null;
            }
            return (string)editValue;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str.StrTrim()) || string.IsNullOrWhiteSpace(str.StrTrim());
        }

        public static string StrTrim(this string str)
        {
            if (str == null || string.IsNullOrEmpty(str.Trim()) || string.IsNullOrWhiteSpace(str.Trim()))
            {
                return null;
            }
            return str.Trim();
        }

        public static T GetEntityFromFocusRow<T>(this GridView gridView) where T : class
        {
            var focusRow = gridView.GetFocusedRow();
            return (T) focusRow;
        }

        public static void SetFocuseRow(
            this GridView gridView, GridColumn column, object value, object currentDefaultValue = null)
        {
            value = value ?? currentDefaultValue;
            if (column == null || value == null) { return; }

            var columnType = column.ColumnType;
            var memObject = Convert.ChangeType(value, columnType);
            gridView.FocusedRowHandle = gridView.LocateByValue(column.FieldName, memObject);
        }

        public static void SaveChange<T>(this Table<T> linqTable, T obj) where T : class
        {
            var singleOrDefault = linqTable.SingleOrDefault(arg => arg == obj);
            if (singleOrDefault == null)
            {
                linqTable.InsertOnSubmit(obj);
            }
            linqTable.Context.SubmitChanges();
        }

        public static void SetActionSelectionChanged(this GridView view, Action action)
        {
            view.SelectionChanged += (o, args) => action();
            view.FocusedRowChanged += (o, args) => action();
            view.DataSourceChanged += (o, args) => action();
        }
         
    }
}
