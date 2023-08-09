﻿using System;
using UnityEngine;

namespace HierarchyOrganizer.Editor.Hierarchy.Conditions
{
	public sealed class ComponentCondition : ConditionBase
	{
		// TODO: Complete prototype predicate
		
		private readonly Mode _mode;
		private readonly Type _scriptType;

		public enum Mode
		{
			Has,
			Lacks,
			Prototype
		}

		public ComponentCondition(Mode mode, Type type)
		{
			_mode = mode;
			_scriptType = type;

			Condition = mode switch
			{
				Mode.Has => HasPredicate,
				
				Mode.Lacks => LacksPredicate,
			
				Mode.Prototype => (go) =>
				{
					Component script;
					if (go.TryGetComponent(_scriptType, out script))
					{
						return PrototypePredicate(script);
					}

					return false;
				},
				_ => Condition
			};
		}

		private bool PrototypePredicate(Component script) => false;

		private bool HasPredicate(GameObject obj) => obj.TryGetComponent(_scriptType, out _);

		private bool LacksPredicate(GameObject obj) => !obj.TryGetComponent(_scriptType, out _);
	}
}