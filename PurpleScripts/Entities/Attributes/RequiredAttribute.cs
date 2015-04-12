using System;
using System.Reflection;

namespace PurpleAttributes
{
	public class RequiredAttribute : Attribute
	{
		private string ErrorMessage;
		private bool Required;

		public RequiredAttribute()
		{
			this.ErrorMessage = String.Empty;
			this.Required = true;
		}

		public RequiredAttribute(string ErrorMessage)
		{
			this.ErrorMessage = ErrorMessage;
			this.Required = true;
		}

		public RequiredAttribute(bool Required, string ErrorMessage)
		{
			this.ErrorMessage = ErrorMessage;
			this.Required = Required;
		}

		public bool validate<T>(T data, PropertyInfo singleProperty, out string error)
		{
			error = string.Empty;
			if(this.Required == false)
				return true;

			bool returnValue = true;

			try {
				switch (Type.GetTypeCode(singleProperty.PropertyType))
				{
				case TypeCode.DateTime:
					DateTime dateValue = (DateTime)singleProperty.GetValue(data, null);
					DateTime dtNull = Convert.ToDateTime("0001-01-01 00:00:00");
					if(dateValue == null || dateValue.Equals(dtNull) || dateValue.Equals(DateTime.MinValue))
						returnValue =  false;
					break;

				case TypeCode.Boolean:
					break;

				case TypeCode.Int32:
					int intValue = Convert.ToInt32(singleProperty.GetValue(data, null));
					if (intValue == null || intValue == 0)
						returnValue =  false;
					break;

				case TypeCode.String:
					string stringValue = singleProperty.GetValue(data, null).ToString();
					if(string.IsNullOrEmpty(stringValue) || stringValue.Equals("NULL"))
						returnValue =  false;
					break;

				default:
					object defaultValue = singleProperty.GetValue(data, null);
					if(defaultValue == null)
						returnValue =  false;
					break;
				}
			} catch {
				returnValue =  false;
			}

			if (!returnValue)
				error = this.ErrorMessage + " " + singleProperty.Name;
			return returnValue;
		}
	}
}
