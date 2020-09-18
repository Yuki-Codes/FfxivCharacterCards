// Licensed under the MIT license.

namespace FfxivCharacterCards
{
	internal static class FlagsUtils
	{
		public static bool IsSet<T>(T flags, T flag)
			where T : struct
		{
			int flagsValue = (int)(object)flags;
			int flagValue = (int)(object)flag;

			return (flagsValue & flagValue) != 0;
		}

		public static void Set<T>(ref T flags, T flag, bool set)
			where T : struct
		{
			if (set)
			{
				Set(ref flags, flag);
			}
			else
			{
				Unset(ref flags, flag);
			}
		}

		public static void Set<T>(ref T flags, T flag)
			where T : struct
		{
			int flagsValue = (int)(object)flags;
			int flagValue = (int)(object)flag;

			flags = (T)(object)(flagsValue | flagValue);
		}

		public static void Unset<T>(ref T flags, T flag)
			where T : struct
		{
			int flagsValue = (int)(object)flags;
			int flagValue = (int)(object)flag;

			flags = (T)(object)(flagsValue & (~flagValue));
		}
	}
}
