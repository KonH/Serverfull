using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Serverfull.UI.Common {
	public class TabsView : MonoBehaviour {
		[Serializable]
		public class TabSetup {
			public Button     Header;
			public GameObject Content;
		}

		public List<TabSetup> Tabs;

		void Awake() {
			InitTabs();
			OpenTab(0);
		}

		void InitTabs() {
			for ( var i = 0; i < Tabs.Count; i++ ) {
				var index = i;
				Tabs[i].Header.onClick.AddListener(() => OpenTab(index));
			}
		}

		void OpenTab(int index) {
			for ( var i = 0; i < Tabs.Count; i++ ) {
				var isCurrentTab = (i == index);
				Tabs[i].Header.interactable = !isCurrentTab;
				Tabs[i].Content.SetActive(isCurrentTab);
			}
		}
	}
}
