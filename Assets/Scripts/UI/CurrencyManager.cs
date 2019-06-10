using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{

    public TopPanel panel;
    public int startingSouls;
    public GameObject soulsCount;

    public delegate void OnSoulsChanged();
    public OnSoulsChanged soulsChangedEvent;


	private AchievementManager achievement;
    private int souls;
    public int Souls
    {
        get
        {
            return souls;
        }
    }

    void Start()
    {
        AddSouls(startingSouls);
		achievement = GameObject.Find ("GameManager").GetComponent<AchievementManager> ();
        panel = soulsCount.transform.parent.GetComponent<TopPanel>();
        SetTextSouls();
    }

	void Update() {
		if (souls >= 1000) {
			achievement.AwardAchievement (1);
		}
	}

    public void AddSouls(int amount)
    {
        souls += amount;
        SetTextSouls();

        if (soulsChangedEvent != null)
        {
            soulsChangedEvent();
        }
    }

    public void LoseSouls(int amount)
    {
        souls -= amount;
        SetTextSouls();

        if (soulsChangedEvent != null)
        {
            soulsChangedEvent();
        }
    }

    public void SetTextSouls()
    {
        soulsCount.GetComponent<Text>().text = souls.ToString();
        panel.ResizeContainer();
    }
}