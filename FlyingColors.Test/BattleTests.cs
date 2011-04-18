using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FlyingColors.Test
{
	public class BattleTests
	{
		[Fact]
		[UseInMemoryDatabase]
		public void battle_of_cape_ortugal()
		{
			Scenario capeOrtugal = ScenarioTests.CapeOrtugal();

			Battle battle = Battle.NewBattle(capeOrtugal);

			var ceasar = battle.GetShip("Ceasar");
			Assert.NotNull(ceasar);
			// Set FULL SAILS
			ceasar.FullSails = true;

			var fireAttack = ceasar.FirePort();
			Assert.NotNull(fireAttack);
			
			// check modified rate 
			RelativeRate expectedModifiedRate = RelativeRate.ThirdRate;
			Assert.Equal<string>(expectedModifiedRate.Symbol, fireAttack.ModifiedRate.Symbol);

			// assign target ship
			var formidable = battle.GetShip("Formidable");
			Assert.NotNull(formidable);
			fireAttack.TargetShip = formidable;
			Assert.Same(formidable, fireAttack.TargetShip);			

			// enter range
			int range = 4;
			fireAttack.Range = range;
			Assert.Equal<int>(range, fireAttack.Range);
			Assert.False(fireAttack.IsPointBlank);

			// firing arc
			fireAttack.OutsideBroadsideArc = false;
			fireAttack.DownOwnRakeLine = false;

			// check base firepower 
			FirePower expectedBaseFirePower = expectedModifiedRate.GetFirePowerAtRange(range);
			Assert.Equal<int>(expectedBaseFirePower.Value, fireAttack.BaseFirePower.Value);
			Assert.Equal<bool>(expectedBaseFirePower.IsShaded, fireAttack.BaseFirePower.IsShaded);
			Assert.Equal<bool>(expectedBaseFirePower.HasNoEffect, fireAttack.BaseFirePower.HasNoEffect);

			// check modified fire power, incremented by audacity and first broadside
			FirePower expectedModifiedFirePowerValue = expectedBaseFirePower.IncrementBy(5);
			Assert.Equal<int>(expectedModifiedFirePowerValue.Value, fireAttack.ModifiedFirePower.Value);
			Assert.Equal<bool>(expectedModifiedFirePowerValue.IsShaded, fireAttack.ModifiedFirePower.IsShaded);
			Assert.Equal<bool>(expectedModifiedFirePowerValue.HasNoEffect, fireAttack.ModifiedFirePower.HasNoEffect);

			// select shot target
			TargetArea targetting = TargetArea.Hull;
			fireAttack.Targetting = targetting;
			Assert.Equal<TargetArea>(targetting, fireAttack.Targetting);

			// select wind
			fireAttack.WeatherGauge = WeatherGauge.Leeward;
			
			// rake?
			fireAttack.IsRake = false;
			Assert.Equal<RakeType>(RakeType.None, fireAttack.RakeType);

			// roll die and modify
			int dieRoll = 4;
			// modified by British targetting Hull and Leeward from target.
			int modifiedDieRoll = dieRoll + 1 + 1;
			fireAttack.DieRoll = dieRoll;
			Assert.Equal<int>(dieRoll, fireAttack.DieRoll);
			Assert.Equal<int>(modifiedDieRoll, fireAttack.ModifiedDieRoll);
			Assert.Equal<int>(-1, fireAttack.ModifiedDieRollExcess);
			Assert.Equal<int>(3, fireAttack.TotalHullDamage);
			Assert.Equal<int>(0, fireAttack.TotalRiggingDamage);
			Assert.Equal<int>(0, fireAttack.TotalMarineDamage);
			Assert.Equal<bool>(false, fireAttack.ChanceOfFire);
						
		}
	}
}
