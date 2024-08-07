
using System;
using System.Collections.Generic;

public class Integrations
{
	private Dictionary<string, string> globalParams = new Dictionary<string, string>();

	private static Integrations _instance;

	protected bool isLeadboardLogged;

	public static Integrations Instance()
	{
		if (Integrations._instance == null)
		{
			Integrations._instance = new Integrations();
			Integrations._instance.Init();
		}
		return Integrations._instance;
	}

	public string GetGlobalParam(string key, string defaultValue = "1")
	{
		if (this.globalParams.ContainsKey(key))
		{
			return this.globalParams[key];
		}
		return defaultValue;
	}

	public void SetGlobalParam(string key, string value)
	{
		if (this.globalParams.ContainsKey(key))
		{
			this.globalParams[key] = value;
		}
		else
		{
			this.globalParams.Add(key, value);
		}
	}

	private void Init()
	{
		this.RequestInterstitial();
	}

	public void RequestBanner()
	{
		
	}

	public void RequestInterstitial()
	{
	
	}

	public void ShowInterstitial()
	{
		
	}
}
