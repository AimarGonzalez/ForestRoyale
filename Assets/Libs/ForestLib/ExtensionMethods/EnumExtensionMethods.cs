using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ForestLib.ExtensionMethods
{
	public static class EnumExtensionMethods
	{
		
		public static bool IsSameFlag<TEnum>(this TEnum flagsA, TEnum flagsB) where TEnum : struct, Enum
		{
			// Non-boxing alternative to flagsA.Equals(flagsB)
			return EqualityComparer<TEnum>.Default.Equals(flagsA, flagsB);
		}
		
		/// <summary>
		/// Returns true if all the bit fields in <c>requiredFlags</c> are also set in <c>existingFlags</c>, following boolean expression:
		/// <br/><c>(existingFlags &amp; requiredFlags) == requiredFlags</c>
		/// <remarks>Except when <c>requiredFlags=Enum.None</c>, then the method will always return false.</remarks>
		/// </summary>
		/// <param name="existingFlags"></param>
		/// <param name="requiredFlags"></param>
		/// <example>
		/// <code><![CDATA[
		/// [Flags]
		/// public enum Letters
		/// {
		///     None = 0,
		///     A = 1 << 0,
		///     B = 1 << 1,
		///     C = 1 << 2,
		///     Any = int.MaxValue
		/// }
		/// 
		/// public void CheckFlags()
		/// {
		///     Letters existingFlags = Letters.A | Letters.B;
		///     
		///     Debug.Log(existingFlags.HasAllFlags(Letters.A)); // true
		///     Debug.Log(existingFlags.HasAllFlags(Letters.B)); // true
		///     Debug.Log(existingFlags.HasAllFlags(Letters.C)); // false
		///     Debug.Log(existingFlags.HasAllFlags(Letters.A | Letters.B)); // true
		///     Debug.Log(existingFlags.HasAllFlags(Letters.A | Letters.C)); // false
		///     Debug.Log(existingFlags.HasAllFlags(Letters.Any)); // false
		///     Debug.Log(existingFlags.HasAllFlags(Letters.None)); // false
		/// }
		/// ]]>
		/// </code>
		/// </example>
		public static bool HasAllFlags(this Enum existingFlags, Enum requiredFlags)
		{
			long currentFlagsValue = Convert.ToInt64(requiredFlags);
			if (currentFlagsValue == 0)
			{
				// Enum.none - fails the match 
				return false;
			}

			return existingFlags.HasFlag(requiredFlags);
		}
		
		/// <summary>
		/// Returns true if one or more bit fields are set in both instances, following boolean expression:
		/// <br/><c>(validFlagsValue &amp; currentFlagsValue) != 0</c>
		/// </summary>
		/// <param name="flagA"></param>
		/// <param name="flagB"></param>
		/// <example>
		/// <code><![CDATA[
		/// [Flags]
		/// public enum Letters
		/// {
		///     None = 0,
		///     A = 1 << 0,
		///     B = 1 << 1,
		///     C = 1 << 2,
		///     Any = int.MaxValue
		/// }
		/// 
		/// public void CheckFlags()
		/// {
		///     Letters validFlags = Letters.A | Letters.B;
		///     
		///     Debug.Log(validFlags.HasAnyFlag(Letters.A)); // true
		///     Debug.Log(validFlags.HasAnyFlag(Letters.B)); // true
		///     Debug.Log(validFlags.HasAnyFlag(Letters.C)); // false
		///     Debug.Log(validFlags.HasAnyFlag(Letters.A | Letters.B)); // true
		///     Debug.Log(validFlags.HasAnyFlag(Letters.A | Letters.C)); // true
		///     Debug.Log(validFlags.HasAnyFlag(Letters.B | Letters.C)); // true
		///     Debug.Log(validFlags.HasAnyFlag(Letters.Any)); // true
		///     Debug.Log(validFlags.HasAnyFlag(Letters.None)); // false
		/// }
		/// ]]>
		/// </code>
		/// </example>
		public static bool HasAnyFlag<T>(this T flagA, T 	flagB) where T : struct, Enum
		{
			long flagAValue = Convert.ToInt64(flagA);
			long flagBValue = Convert.ToInt64(flagB);
			return (flagAValue & flagBValue) != 0;
		}
		
		// The following set of methods can be used to convert values between an Enum and its EnumFlag conuterpart.
		// The Enum is meant to be used in game logic while the EnumFlag is meant to be used in UI classes, where
		// we allow the user to pick multiple or any values.
		//
		// See an example:
		//
		//		public enum RunningState
		//		{
		//			Idle = 0,
		//			Walk = 10,
		//			Jog = 20,
		//			Run = 30
		//		}
		//		
		//		[Flags]
		//		public enum RunningStateFlag
		//		{
		//			Idle = 1 << 0,
		//			Walk = 1 << 1,
		//			Jog = 1 << 2,
		//			Run = 1 << 3,
		//			
		//			Any = int.MaxValue
		//		}
	
		public static TEnumFlag ToFlag<TEnumFlag, TEnum>(this TEnum enumValue)
			where TEnum : struct, Enum
			where TEnumFlag : struct, Enum
		{
			if (!Enum.TryParse(enumValue.ToString(), out TEnumFlag flag))
			{
				throw new ArgumentException($"Enum type mismatch: value {typeof(TEnum).Name}.{enumValue.ToString()} is" +
				                            $" not present in {typeof(TEnumFlag).Name}");
			}

			return flag;
		}

		public static bool HasEnum<TEnumFlag, TEnum>(this TEnumFlag flag, TEnum enumValue)
			where TEnum : struct, Enum
			where TEnumFlag : struct, Enum
		{
			return flag.HasFlag(enumValue.ToFlag<TEnumFlag, TEnum>());
		}
		
		// Asserts all the values in Enum are also present in EnumFlag.
		// To be used in a static assert, like the example below:
		//
		//		[InitializeOnLoad]
		//		internal static class AssertEnumFlagTypeIsComplete
		//		{
		//			static AssertEnumFlagTypeIsComplete()
		//			{ 
		//				EnumExtensionMethods.AssertEnumFlagTypeIsComplete<RunningState, RunningStateFlag>();
		//			}
		//		}

		public static void AssertEnumFlagTypeIsComplete<TEnum, TEnumFlag>()
			where TEnum : struct, Enum
			where TEnumFlag : struct, Enum
		{
			foreach (var enumValueName in Enum.GetNames(typeof(TEnum)))
			{
				Debug.Assert(Enum.TryParse(enumValueName, out TEnumFlag outFlag),
					$"Enum type mismatch: value {typeof(TEnum).Name}.{enumValueName} is not present in {typeof(TEnumFlag).Name}");
			}
		}
	}
}