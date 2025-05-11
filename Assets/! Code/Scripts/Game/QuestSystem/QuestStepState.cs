[System.Serializable]
public class QuestStepState
{
    public string state;    // Wewnêtrzny stan logiki kroku (np. "key_A_found")
    public string status;   // Tekst wyœwietlany graczowi dla tego kroku

    public QuestStepState(string state, string status)
    {
        this.state = state;
        this.status = status;
    }

    public QuestStepState()
    {
        this.state = "";
        this.status = "";
    }
}