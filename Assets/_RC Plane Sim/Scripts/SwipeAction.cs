using UnityEngine;
using System.Collections;

public enum SwipeDirection
{
    Null = 0,
    Right = 1,
    Left = 2
}

public enum Difficult
{
	EASY = 0,
	MEDIUM = 1,
	HARD = 2,
	MASTER = 3,
	NIGHTMARE = 4,
}

public class SwipeAction : MonoBehaviour 
{
	private string[] DIFFICULT = new string[5] { "EASY", "MEDIUM", "HARD", "MASTER", "NIGHTMARE"};

    public UILabel text;
    public static int levelDifficult;

    private SwipeDirection sSwipeDirection;

    private float fInitialX;
    private float fInitialY;
    private float fFinalX;
    private float fFinalY;

    private float inputX;
    private float inputY;
    private float slope;
    private float fDistance;
    private float iTouchStateFlag;

	public TweenScale[] twScale;
	public TweenPosition[] twPosition;

	private int currentLevel = 0;
	private int prevLevel = 0;

	private Vector3 stepScale = new Vector3(0.1f, 0.1f, 0.1f);
	private Vector3 stepPosition = new Vector3(80f, 0f, 0f);

    void OnEnable()
    {
        iTouchStateFlag = 0;

		text.text = DIFFICULT[currentLevel];
    }

    void Update()
    {
        if (iTouchStateFlag == 0 && Input.GetMouseButtonDown(0))
        {
            fInitialX = Input.mousePosition.x;
            fInitialY = Input.mousePosition.y;

            sSwipeDirection = SwipeDirection.Null;
            iTouchStateFlag = 1;
        }
        if (iTouchStateFlag == 1)
        {
            fFinalX = Input.mousePosition.x;
            fFinalY = Input.mousePosition.y;

            sSwipeDirection = swipeDirection();

            if (sSwipeDirection != SwipeDirection.Null)
            {
                iTouchStateFlag = 2;
				prevLevel = currentLevel;

                switch (sSwipeDirection)
                {
                    case SwipeDirection.Left:
						currentLevel = Mathf.Min(twScale.Length - 1, currentLevel + 1);
                        levelDifficult = currentLevel + 1;
                        break;
                    case SwipeDirection.Right:
						currentLevel = Mathf.Max(0, currentLevel - 1);
                        levelDifficult = currentLevel + 1;
                        break;
                }

				if(prevLevel != currentLevel)
				{
                	startTween(sSwipeDirection);
				}
				else
				{
					iTouchStateFlag = 1;
				}
            }
        }
        if (iTouchStateFlag == 1 && Input.GetMouseButtonUp(0))
        {
            iTouchStateFlag = 0;
        }
    }

    public void startTween(SwipeDirection direction)
	{
		int min = direction == SwipeDirection.Left ? (prevLevel - 1) : prevLevel - 2;
		int max = direction == SwipeDirection.Left ? (prevLevel + 3) : prevLevel + 2;
		int sign = direction == SwipeDirection.Left ? 1 : -1;

		for(int i = Mathf.Max(0, min); i < Mathf.Min(twScale.Length, max); i++)
		{
			if(twPosition[i].to.x == 160 * sign)
			{
				twScale[i].from = twScale[i].to;
				twScale[i].to = twScale[i].to + stepScale;
			}
			else if(twPosition[i].to.x == -80)
			{
				twScale[i].from = twScale[i].to;
				twScale[i].to = twScale[i].to - stepScale * sign;
			}
			else if(twPosition[i].to.x == 80)
			{
				twScale[i].from = twScale[i].to;
				twScale[i].to = twScale[i].to + stepScale * sign;
			}
			else if(twPosition[i].to.x == 0)
			{
				twScale[i].from = twScale[i].to;
				twScale[i].to = twScale[i].to - stepScale;
			}

			twPosition[i].duration = 0.2f;
			twPosition[i].from = twPosition[i].to;
			twPosition[i].to = twPosition[i].to - stepPosition * sign;

			twScale[i].duration = 0.2f;
			twScale[i].GetComponent<UISprite>().depth = currentLevel * sign + 165 - Mathf.Abs((int)twPosition[i].to.x);
			twScale[i].transform.GetChild(0).GetComponent<UISprite>().depth = currentLevel * sign + 166 - Mathf.Abs((int)twPosition[i].to.x);

			twScale[i].enabled = true;
			twPosition[i].enabled = true;
			twScale[i].ResetToBeginning();
			twPosition[i].ResetToBeginning();
		}

		StartCoroutine (releaseRotation ());
    }

	public IEnumerator releaseRotation()
	{
		yield return new WaitForSeconds (0.2f);
		text.text = DIFFICULT[currentLevel];
		iTouchStateFlag = 0;
	}

    private SwipeDirection swipeDirection()
    {
        inputX = fFinalX - fInitialX;
        inputY = fFinalY - fInitialY;
        slope = inputY / inputX;

        fDistance = Mathf.Sqrt(Mathf.Pow((fFinalY - fInitialY), 2) + Mathf.Pow((fFinalX - fInitialX), 2));

        if (fDistance <= (Screen.width / 15))
            return SwipeDirection.Null;

        else if (inputX > 0 && inputY >= 0 && slope < 1 && slope >= 0)
        {
            return SwipeDirection.Right;
        }
        else if (inputX > 0 && inputY <= 0 && slope > -1 && slope <= 0)
        {
            return SwipeDirection.Right;
        }
        else if (inputX < 0 && inputY >= 0 && slope > -1 && slope <= 0)
        {
            return SwipeDirection.Left;
        }
        else if (inputX < 0 && inputY <= 0 && slope >= 0 && slope < 1)
        {
            return SwipeDirection.Left;
        }

        return SwipeDirection.Null;
    }
}
