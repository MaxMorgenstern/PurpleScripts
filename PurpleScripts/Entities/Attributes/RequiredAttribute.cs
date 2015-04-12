using System;

namespace PurpleAttributes
{
	public class RequiredAttribute : Attribute
	{
		public string ErrorMessage;
		public bool Required;

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
	}
}
