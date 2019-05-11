[System.Serializable]
public class PlayerClass
{
    public string playerName;
    public float playerScore;
    public bool hasColor;
    public int playerColor;

    public void AddScore()
    {
        playerScore++;
    }

    public void ChooseColor(int _color)
    {
        hasColor = true;
        playerColor = _color;
    }
}
