using System;
using System.Reflection;

namespace PurpleAttributes
{
	public class ScopeAttribute : Attribute
	{
		private string ErrorMessage;
		private float max;
		private float min;

		public ScopeAttribute(float min, float max)
		{
			init (min, max, string.Empty);
		}

		public ScopeAttribute(float min, float max, string ErrorMessage)
		{
			init (min, max, ErrorMessage);
		}


		public ScopeAttribute(int min, int max)
		{
			init ((float)min, (float)max, string.Empty);
		}

		public ScopeAttribute(int min, int max, string ErrorMessage)
		{
			init ((float)min, (float)max, ErrorMessage);
		}


		public ScopeAttribute(decimal min, decimal max)
		{
			init ((float)min, (float)max, string.Empty);
		}

		public ScopeAttribute(decimal min, decimal max, string ErrorMessage)
		{
			init ((float)min, (float)max, ErrorMessage);
		}


		public bool validate<T>(T data, PropertyInfo singleProperty, out string error)
		{
			error = string.Empty;
			bool returnValue = true;
			float numberToTest = float.NaN;

			try {
				switch (Type.GetTypeCode(singleProperty.PropertyType))
				{
				case TypeCode.Int32:
					numberToTest = (float)Convert.ToInt32(singleProperty.GetValue(data, null));
					break;

				default:
					numberToTest = (float)Convert.ToDecimal(singleProperty.GetValue(data, null));
					break;
				}
			} catch {
				returnValue = false;
			}

			if(numberToTest < this.min || numberToTest > this.max)
				returnValue = false;

			if (!returnValue)
				error = this.ErrorMessage + " " + singleProperty.Name;
			return returnValue;
		}


		private void init(float min, float max, string ErrorMessage)
		{
			this.ErrorMessage = ErrorMessage;
			this.min = min;
			this.max = max;
		}
	}
}

