using UnityEngine;
using UnityEngine.UI;
using Serverfull.Models;

namespace Serverfull.UI.Game {
	public class ClientView : MonoBehaviour {
		public Text  NameText;
		public Text  DifficultyText;
		public Text  PriceText;
		public Image MoodImage;

		public Color GoodColor = Color.green;
		public Color BadColor  = Color.red;

		public ClientId     Id    { get; private set; }
		public ClientsPanel Owner { get; private set; }

		public void Init(Client client, ClientsPanel owner) {
			Id                  = client.Id;
			Owner               = owner;
			NameText.text       = client.Id.Name;
			DifficultyText.text = client.Difficulty;
			PriceText.text      = $"{client.Income}/h";

			gameObject.name = string.Format("{0}_View", Id.Name);
		}

		public void UpdateMood(float value) {
			MoodImage.color = Color.Lerp(BadColor, GoodColor, value);
		}
	}
}
