using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace FlyingColors.Test
{
	public class DamageTests
	{
		public class TestDamageExpected
		{
			public string Result { get; set; }
			public int Hull { get; set; }
			public int Rigging { get; set; }
			public int Marines { get; set; }
			public bool ChanceOfFire { get; set; }			
		}

		public static IEnumerable<object[]> DamageExpectedList
		{
			get
			{
				yield return new object[] { new TestDamageExpected()
				{
					Result = "-",
					Hull = 0,
					Rigging = 0,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "R",
					Hull = 0,
					Rigging = 1,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "H",
					Hull = 1,
					Rigging = 0,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "2R",
					Hull = 0,
					Rigging = 2,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "2H",
					Hull = 2,
					Rigging = 0,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "2R*",
					Hull = 0,
					Rigging = 2,
					Marines = 0,
					ChanceOfFire = true
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "3H*",
					Hull = 3,
					Rigging = 0,
					Marines = 0,
					ChanceOfFire = true
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "5R2H",
					Hull = 2,
					Rigging = 5,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "6H2R",
					Hull = 6,
					Rigging = 2,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "6RH",
					Hull = 1,
					Rigging = 6,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "5HR",
					Hull = 5,
					Rigging = 1,
					Marines = 0,
					ChanceOfFire = false
				}};
				yield return new object[] { new TestDamageExpected()
				{
					Result = "4RM",
					Hull = 0,
					Rigging = 4,
					Marines = 1,
					ChanceOfFire = false
				}};
			}
		}

		[Theory]
		[PropertyData("DamageExpectedList")]
		public void damage_ctor_test(TestDamageExpected expected)
		{
			Damage actual = new Damage(expected.Result);
			Assert.Equal<string>(expected.Result, actual.Result);			
			Assert.Equal<int>(expected.Hull, actual.Hull);
			Assert.Equal<int>(expected.Rigging, actual.Rigging);
			Assert.Equal<int>(expected.Marines, actual.Marines);
			Assert.Equal<bool>(expected.ChanceOfFire, actual.ChanceOfFire);
		}
	}
}
