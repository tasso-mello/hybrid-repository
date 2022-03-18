namespace core.hybrid.repository.Enums
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensionMethodsAndGenerics
	{
		public static string GetDescription<T>(this Enum GenericEnum) where T : System.ComponentModel.DescriptionAttribute
		{
			Type genericEnumType = GenericEnum.GetType();
			MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
			if ((memberInfo != null && memberInfo.Length > 0))
			{
				var _Attribs = memberInfo[0].GetCustomAttributes(typeof(T), false);
				if ((_Attribs != null && _Attribs.Any()))
				{
					return ((T)_Attribs.ElementAt(0)).Description;
				}
			}
			return GenericEnum.ToString();
		}

		public static string GetDescription(this Enum GenericEnum)
		{
			return GetDescription<System.ComponentModel.DescriptionAttribute>(GenericEnum);
		}

		public static int GetInt(this Enum GenericEnum)
		{
			return Convert.ToInt32(GenericEnum);
		}
	}
}
