using System;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
	public static GUIManager instance;

	public MainMenuGUI mainMenuGUI;

	public InGameGUI inGameGUI;

	public TutorialGUI tutorialGUI;

	public ShareGUI shareGUI;

	public MaskLayerController maskGUI;

	public PurchaseMaskGUI purchseGUI;

	public SettingsGUI settingGUI;

	private void Awake()
	{
		GUIManager.instance = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ShowMainMenuGUI(int tag)
	{
		if (tag == 0)
		{
			Camera.main.transform.position = new Vector3(9999f, 9999f, 9999f);
			this.mainMenuGUI.gameObject.SetActive(true);
			this.inGameGUI.gameObject.SetActive(false);
		}
		else
		{
			SoundsManager.instance.PlayAudioSource(SoundsManager.instance.UIButton);
			Pause.instance.UnPause();
			if (ScoreHandler.instance.score > InGameGUI.instance.lastHighScore)
			{
				ScoreHandler.instance.highScore = ScoreHandler.instance.score;
				ScoreHandler.instance.saveHighScoreToPrefs();
				
			}
			this.mainMenuGUI.gameObject.SetActive(true);
			this.inGameGUI.gameObject.SetActive(false);
		}
	}

	public void ShowPauseGUI()
	{
		Pause.instance.InPause();
	}

	public void ShowInGameGUI()
	{
		this.inGameGUI.gameObject.SetActive(true);
	}

	public void CloseInGameGUI()
	{
		this.inGameGUI.gameObject.SetActive(false);
	}

	public void ShowGameOverGUI()
	{
		Camera.main.transform.position = new Vector3(9999f, 9999f, 9999f);
        AdsControl.Instance.showAds();
		this.shareGUI.Activete();
	}

	public void ShowTutorialGUI()
	{
		this.tutorialGUI.Activate();
	}

	public void OpenMask()
	{
		this.maskGUI.Activite();
	}

	public void CloseMask()
	{
		this.maskGUI.DeadActivite();
	}

	public void ShowExplodeSkill()
	{
		this.tutorialGUI.ShowPager3();
		ScoreHandler.instance.SaveFristExplodeSkillState();
	}

	public void ShowRubbish()
	{
		this.tutorialGUI.ShowPager2();
		ScoreHandler.instance.SaveUseDustbinState();
	}

	public void ShowPurchaseMask()
	{
		this.purchseGUI.OpenPurchseMask();
	}

	public void PurchseComplete()
	{
		this.purchseGUI.ClosePurchseMask();
	}

	public void ShowSettingGUI()
	{
		this.settingGUI.gameObject.SetActive(true);
		SettingsGUI.instance.OpenSetting();
	}

	public void CloseSettingGUI()
	{
		this.settingGUI.gameObject.SetActive(false);
	}
}
