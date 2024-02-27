﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaQuickDraw1 : KatanaQuickDrawBase
{
	public KatanaQuickDraw1(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack4", false)
	{
	}
}

public class KatanaQuickDraw2 : KatanaQuickDrawBase
{
	public KatanaQuickDraw2(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack5", false)
	{

	}
}
public class KatanaQuickDraw3 : KatanaQuickDrawBase
{
	public KatanaQuickDraw3(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack31", false)
	{
	}
}

public class KatanaQuickDraw4 : KatanaQuickDrawBase
{
	public KatanaQuickDraw4(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack6", true, "Prefab/CrackSlashVFX", 1f)
	{
	}
}


public class KatanaQuickDraw5 : KatanaQuickDrawBase
{
	public KatanaQuickDraw5(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack17", true)
	{
	}

	protected override void Attack(bool clockwise)
	{
		//base.Attack(clockwise);
		owner.PlayDeathfaultFeedback();
	}
}

public class KatanaQuickDraw6 : KatanaQuickDrawBase
{
	public KatanaQuickDraw6(Katana owner, StateMachine<Katana.State, Katana> stateMachine)
		: base(owner, stateMachine, "Attack7", true, "Prefab/CrackSlashVFX", 1f)
	{
	}

	protected override void VFXSetting(GameObject vfx)
	{
		base.VFXSetting(vfx);
		CrackSlashVFXController controller = vfx.GetComponent<CrackSlashVFXController>();
		controller.Init(2f);
	}

	protected override void AttackEnd()
	{
		//base.AttackEnd();
		stateMachine.ChangeState(Katana.State.QuickDraw7);
	}
}

public class KatanaQuickDraw7 : StateBase<Katana.State, Katana>
{
	const float moveDist = 8f; 
	PlayerAttack playerAttack;
	PlayerMove playerMove;
	PlayerLook playerLook;
	PlayerAnimEvent playerAnimEvent;

	bool attacked;
	public KatanaQuickDraw7(Katana owner, StateMachine<Katana.State, Katana> stateMachine) : base(owner, stateMachine)
	{
	}

	public override void Enter()
	{
		attacked = false;
		Time.timeScale = 0.5f;
		playerAnimEvent.OnEquipChange.AddListener(EquipChange);
		playerAnimEvent.OnClockWiseAttack.AddListener(Attack);

		playerMove.ManualMove(playerMove.transform.forward * moveDist, 1f);
		GameManager.Resource.Instantiate<GameObject>("Prefab/RushVFX", 
			playerMove.transform.position, playerMove.transform.rotation, true);
		playerAttack.SetAnimTrigger("Attack34");
		playerLook.AutoRotate(0.5f, 150f, 0f);
	}

	public override void Exit()
	{
		Time.timeScale = 1f;
		playerAnimEvent.OnEquipChange.RemoveListener(EquipChange);
		playerAnimEvent.OnClockWiseAttack.RemoveListener(Attack);
	}

	public override void Setup()
	{
		playerMove = owner.Player.GetComponent<PlayerMove>();
		playerLook = owner.Player.GetComponent<PlayerLook>();
		playerAttack = owner.PlayerAttack;
		playerAnimEvent = owner.PlayerAnimEvent;
	}

	public override void Transition()
	{
		if (attacked == false) return;

		if (playerAttack.IsAnimWait(0) == true)
		{
			playerAttack.SetAnimTrigger("BaseExit");
			stateMachine.ChangeState(Katana.State.Unarmed);
		}
	}

	public override void Update()
	{

	}

	private void EquipChange()
	{
		owner.SetDummyRender(true);
	}

	private void Attack()
	{
		attacked = true;
		GameManager.Resource.Instantiate<GameObject>("Prefab/FX_splash_explosion_air", 
			playerMove.transform.position - (playerMove.transform.forward * moveDist * 0.5f) + Vector3.up * 2f, 
			Quaternion.identity, true);
	}
}