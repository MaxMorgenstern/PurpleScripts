using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace PurpleAttributes
{
	public class Validator
	{
		public static bool Validate<T>(T data)
		{
			List<string> e = new List<string> ();
			return Validate (data, out e);
		}

		public static bool Validate<T>(T data, out List<string> error)
		{
			bool returnValue = true;
			error = new List<string> ();
			foreach (PropertyInfo singleProperty in typeof(T).GetProperties())
			{
				foreach(object ob in singleProperty.GetCustomAttributes(true))
				{
					string errorMessage = string.Empty;

					if(ob is RequiredAttribute)
					{
						RequiredAttribute reqAttribute = (RequiredAttribute)ob;
						if(!reqAttribute.validate(data, singleProperty, out errorMessage))
						{
							error.Add(errorMessage);
							returnValue = false;
						}
					}

					if(ob is ExpressionAttribute)
					{
						ExpressionAttribute expAttribute = (ExpressionAttribute)ob;
						if(!expAttribute.validate(data, singleProperty, out errorMessage))
						{
							error.Add(errorMessage);
							returnValue = false;
						}
					}

					if(ob is ScopeAttribute)
					{
						ScopeAttribute scopeAttribute = (ScopeAttribute)ob;
						if(!scopeAttribute.validate(data, singleProperty, out errorMessage))
						{
							error.Add(errorMessage);
							returnValue = false;
						}
					}
					// Add additional Attributes
				}
			}
			error.RemoveAll(item => item.Length == 0);
			return returnValue;
		}



		// MOCKUP ////////////////////////////

		public bool validate_mockup<T>(T data, PropertyInfo singleProperty, out string error)
		{
			error = string.Empty;
			bool returnValue = true;

			try {
				switch (Type.GetTypeCode(singleProperty.PropertyType))
				{
				case TypeCode.DateTime:
					DateTime dateValue = (DateTime)singleProperty.GetValue(data, null);
					// TODO: validation
					break;

				case TypeCode.Boolean:
					bool boolValue = Convert.ToBoolean(singleProperty.GetValue(data, null));
					// TODO: validation
					break;


				case TypeCode.Int32:
					int intValue = Convert.ToInt32(singleProperty.GetValue(data, null));
					// TODO: validation
					break;

				case TypeCode.String:
					string stringValue = singleProperty.GetValue(data, null).ToString();
					// TODO: validation
					break;


				default:
					object tmpPropertyValue = singleProperty.GetValue(data, null);
					// TODO: validation
					break;
				}
			} catch {
				returnValue = false;
			}

			//if (!returnValue)
			//	error = this.ErrorMessage + " " + singleProperty.Name;
			return returnValue;
		}
	}
}


