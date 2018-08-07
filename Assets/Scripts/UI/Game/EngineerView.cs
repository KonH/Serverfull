using System;
using UnityEngine;
using UnityEngine.UI;
using Serverfull.Models;

namespace Serverfull.UI.Game {
	public class EngineerView : MonoBehaviour {
		public Text   NameText;
		public Text   PriceText;
		public Text   SalaryText;
		public Text   LevelText;
		public Button HireButton;

		public EngineerId Id { get; private set; }

		public void Init(Engineer engineer, bool canHire, Action<EngineerId> hireCallback) {
			Id              = engineer.Id;
			NameText.text   = Id.Name;
			PriceText.text  = engineer.Price.ToString();
			SalaryText.text = $"{engineer.Salary}/h";
			LevelText.text  = engineer.Level;

			HireButton.onClick.RemoveAllListeners();
			HireButton.onClick.AddListener(() => hireCallback(Id));

			UpdateCanHire(canHire);
		}

		public void UpdateCanHire(bool canHire) {
			HireButton.interactable = canHire;
		}
	}
}
