using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class FinishUIView : MonoBehaviour
{
    LabyrinthModel labyrinthModel;
    InteractiveModel interactiveModel;

    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    StarsView timerStars;
    [SerializeField]
    TextMeshProUGUI collectionText;
    [SerializeField]
    StarsView collectionStars;
    [SerializeField]
    TextMeshProUGUI infoText;
    [SerializeField]
    StarsView infoStars;
    [SerializeField]
    StarsView overallStars;

    void Start()
    {
        labyrinthModel = LabyrinthModel.Instance;
        interactiveModel = InteractiveModel.Instance;
    }

    private void OnEnable()
    {
        if (LabyrinthModel.Instance == null)
        {
            return;
        }

        if (labyrinthModel == null)
        {
            labyrinthModel = LabyrinthModel.Instance;
            interactiveModel = InteractiveModel.Instance;
        }

        if (labyrinthModel.CurrentSelectedLabyrinth != null)
        {
            UpdateTimer();
            UpdateCollection();
            UpdateInfoPoints();
            SetStars();
        }
    }

    void SetStars()
    {
        int timeStarsCount = 0;
        if (labyrinthModel.ElapsedTime > 2800)
        {
            timeStarsCount = 1;
        }
        if (labyrinthModel.ElapsedTime > 2200)
        {
            timeStarsCount = 2;
        }
        if (labyrinthModel.ElapsedTime > 1800)
        {
            timeStarsCount = 3;
        }
        if (labyrinthModel.ElapsedTime > 1400)
        {
            timeStarsCount = 4;
        }
        if (labyrinthModel.ElapsedTime > 0)
        {
            timeStarsCount = 5;
        }

        timerStars.SetStars(timeStarsCount);

        int infoStarsCount = CalculateStars(interactiveModel.InfoSeen, labyrinthModel.CurrentSelectedLabyrinth.InfoPoints.Length);
        infoStars.SetStars(infoStarsCount);

        int collectionStarsCount = CalculateStars(interactiveModel.AmountCollected, labyrinthModel.CurrentSelectedLabyrinth.Collectables.Length);
        collectionStars.SetStars(collectionStarsCount);

        overallStars.SetStars(CalculateStars(timeStarsCount + infoStarsCount + collectionStarsCount, 15));
    }

    public int CalculateStars(int totalCollected, int totalPossible)
    {
        float ratio = (float)totalCollected / totalPossible;
        int stars = Mathf.RoundToInt(ratio * 5);
        return stars;
    }

    void UpdateTimer()
    {
        timerText.text = labyrinthModel.TimeText + "<size=50%>min";
    }

    void UpdateCollection()
    {
        collectionText.text = $"<b>{interactiveModel.AmountCollected}</b>/{labyrinthModel.CurrentSelectedLabyrinth.Collectables.Length}";
    }

    void UpdateInfoPoints()
    {
        infoText.text = $"<b>{interactiveModel.InfoSeen}</b>/{labyrinthModel.CurrentSelectedLabyrinth.InfoPoints.Length}";
    }
}
