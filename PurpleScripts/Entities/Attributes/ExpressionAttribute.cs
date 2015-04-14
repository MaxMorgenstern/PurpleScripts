using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PurpleAttributes
{
	public enum ExpressionType { EMail, URL, HTMLTag };

	public class ExpressionAttribute : Attribute
	{
		public string ErrorMessage;
		public Regex Expression;


		public ExpressionAttribute(Regex Expression)
		{
			init(Expression, string.Empty);
		}

		public ExpressionAttribute(Regex Expression, string ErrorMessage)
		{
			init(Expression, ErrorMessage);
		}


		public ExpressionAttribute(string Expression)
		{
			init(new Regex(Expression), string.Empty);
		}

		public ExpressionAttribute(string Expression, string ErrorMessage)
		{
			init(new Regex(Expression), ErrorMessage);
		}


		public ExpressionAttribute(ExpressionType Type)
		{

			init (new Regex (get_regex(Type)), string.Empty);
		}

		public ExpressionAttribute(ExpressionType Type, string ErrorMessage)
		{
			init (new Regex (get_regex(Type)), ErrorMessage);
		}


		public bool validate<T>(T data, PropertyInfo singleProperty, out string error)
		{
			error = string.Empty;
			bool returnValue = true;

			try {
				string stringValue = singleProperty.GetValue(data, null).ToString();

				if(string.IsNullOrEmpty(stringValue))
					return returnValue;

				Match match = this.Expression.Match(stringValue);
				if (!match.Success)
					returnValue = false;

			} catch { }

			if (!returnValue && !string.IsNullOrEmpty(this.ErrorMessage))
				error = this.ErrorMessage;
			return returnValue;
		}


		private string get_regex(ExpressionType Type)
		{
			string regexString = string.Empty;
			switch (Type)
			{
			case ExpressionType.EMail:
				regexString = @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$";
				break;

			case ExpressionType.HTMLTag:
				regexString = @"^<([a-z]+)([^<]+)*(?:>(.*)<\/\1>|\s+\/>)$";
				break;

			case ExpressionType.URL:
				regexString = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
				break;
			}
			return regexString;
		}

		private void init(Regex Expression, string ErrorMessage)
		{
			this.ErrorMessage = ErrorMessage;
			this.Expression = Expression;
		}
	}
}
