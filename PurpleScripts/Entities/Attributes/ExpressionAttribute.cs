using System.Collections;
using System;
using System.Text.RegularExpressions;

namespace PurpleAttributes
{
	public class ExpressionAttribute : Attribute
	{
		public string ErrorMessage;
		public Regex Expression;

		public ExpressionAttribute(Regex Expression, string ErrorMessage)
		{
			this.ErrorMessage = ErrorMessage;
			this.Expression = Expression;
		}

		public ExpressionAttribute(Regex Expression)
		{
			this.ErrorMessage = string.Empty;
			this.Expression = Expression;
		}
	}
}
