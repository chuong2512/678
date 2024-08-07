using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameGUI : MonoBehaviour
{
	public static InGameGUI instance;

	public Text skillCount;

	public Text scoreText;

	public Text highScoreText;

	public Text coinsText;

	public Text trashPrice;

	public Text hammerText;

	public Button trashButton;

	public GameObject UIMask;

	public Button pauseButton;

	public Button skillButton;

	public GameObject tip;

	public int lastCoins;

	public int lastHighScore;

	public bool useHammer;

	public GameObject[] allDice;

	public Button hammerButton;

	public GameObject dustbin;

	public GameObject shovel;

	public GameObject shovelTip;

	private void Awake()
	{
		InGameGUI.instance = this;
	}

	private void Start()
	{
		this.lastCoins = ScoreHandler.instance.specialPoints;
		ScoreHandler.instance.LoadFristUseShovelState();
		if (ScoreHandler.instance.fristUseShovel == "true")
		{
			this.shovelTip.SetActive(false);
		}
		if (ScoreHandler.instance.fristUseDustbin == "true")
		{
			this.tip.SetActive(false);
		}
		else
		{
			this.tip.SetActive(true);
		}
	}

	private void Update()
	{
		this.hammerText.text = string.Empty + GameManager.instance.HammerPrice;
	}

	private void OnEnable()
	{
		this.RefreshCoins();
		this.RefreshHighScore();
		this.RefreshScore();
		this.RefreshTrashPrice();
		this.RefreshSkillCount();
		Integrations.Instance().RequestBanner();
		ScoreHandler.instance.loadHighScoreFromPrefs();
		this.lastHighScore = ScoreHandler.instance.highScore;
		ScoreHandler.instance.LoadFirstUseDustbin();
		this.shovel.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idle", true);
		this.dustbin.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idle", true);
	}

	public void RefreshHighScore()
	{
		this.highScoreText.text = string.Empty + ScoreHandler.instance.highScore;
	}

	public void RefreshScore()
	{
		this.scoreText.text = string.Empty + ScoreHandler.instance.score;
	}

	public void RefreshHanmmerPrice()
	{
		this.RefreshCoins();
	}

	public void RefreshTrashPrice()
	{
		this.trashPrice.text = string.Empty + GameManager.instance.trashPrice;
		this.RefreshCoins();
	}

	public void RefreshCoins()
	{
		this.coinsText.text = string.Empty + ScoreHandler.instance.specialPoints;
	}

	public void RefreshSkillCount()
	{
		if (GameManager.instance.skillCount == 0)
		{
			this.skillCount.gameObject.SetActive(false);
		}
		else
		{
			this.skillCount.gameObject.SetActive(true);
			this.skillCount.text = GameManager.instance.skillCount.ToString();
		}
	}

	public void OnTrashButtonClick()
	{
		ScoreHandler.instance.LoadFirstUseDustbin();
		if (ScoreHandler.instance.fristUseDustbin != "true")
		{
			this.tip.SetActive(false);
			GUIManager.instance.OpenMask();
			GUIManager.instance.ShowRubbish();
		}
		else
		{
			if (GameManager.instance.trashPrice > ScoreHandler.instance.specialPoints)
			{
				Store.instance.OpenStore();
				SoundsManager.instance.PlayAudioSource(SoundsManager.instance.wrongDrop);
			}
			if (ScoreHandler.instance.specialPoints >= GameManager.instance.trashPrice)
			{
				this.trashButton.interactable = false;
				this.dustbin.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "product", false);
				ScoreHandler.instance.removeSpecialPoints(GameManager.instance.trashPrice);
				if (GameManager.instance.trashPriceAdd == 0 || GameManager.instance.trashPriceAdd < 20)
				{
					GameManager.instance.trashPrice += 20;
				}
				else
				{
					GameManager.instance.trashPrice = GameManager.instance.trashPrice + GameManager.instance.trashPriceAdd;
				}
				SoundsManager.instance.PlayAudioSource(SoundsManager.instance.delete);
				this.RefreshTrashPrice();
				DiceCompound.instance.RespawnDice(0);
			}
		}
	}

	public void OnPauseButtonClick()
	{
		SoundsManager.instance.PlayAudioSource(SoundsManager.instance.UIButton);
		GUIManager.instance.ShowPauseGUI();
	}

	public void Deactivate()
	{
		base.gameObject.SetActive(false);
	}

	public void OpenStore()
	{
		Store.instance.OpenStore();
		this.trashButton.interactable = true;
	}

	public void OpenUIMask()
	{
		this.UIMask.SetActive(true);
		this.trashButton.interactable = false;
		this.pauseButton.interactable = false;
	}

	public void CloseUIMask()
	{
		this.UIMask.SetActive(false);
		this.trashButton.interactable = true;
		this.pauseButton.interactable = true;
	}

	public void OnCilckHammer()
	{
		if (ScoreHandler.instance.specialPoints < GameManager.instance.HammerPrice)
		{
			SoundsManager.instance.PlayAudioSource(SoundsManager.instance.wrongDrop);
			Store.instance.OpenStore();
		}
		else
		{
			this.allDice = GameObject.FindGameObjectsWithTag("Dice");
			if (this.allDice.Length == 0)
			{
				SoundsManager.instance.PlayAudioSource(SoundsManager.instance.wrongDrop);
				return;
			}
			ScoreHandler.instance.LoadFristUseShovelState();
			if (ScoreHandler.instance.fristUseShovel != "true")
			{
				TutorialGUI.instance.ShowPager4();
				this.shovelTip.SetActive(false);
				GUIManager.instance.OpenMask();
				ScoreHandler.instance.SaveFristUseShovel();
			}
			else
			{
				if (GameManager.instance.skillCount > 0)
				{
					GameObject.Find("Skill").transform.GetChild(1).gameObject.SetActive(false);
				}
				SoundsManager.instance.PlayAudioSource(SoundsManager.instance.delete);
				ExplodeSkill.instance.hideAllUI(true);
				this.shovel.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "product1", false);
				this.useHammer = true;
				this.UIMask.SetActive(true);
				GameManager.instance.gamePaused = true;
				for (int i = 0; i < this.allDice.Length; i++)
				{
					this.allDice[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
					this.allDice[i].transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = 3;
					this.allDice[i].transform.GetChild(1).GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "cz-wait", true);
				}
			}
		}
	}
}
