using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MachineApps.PrepareAndDeploy
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputField playerNameInputField;

        public void OnPlay()
        {
            AnimationManager.instance.FadeOutPlayButton(true);
            AnimationManager.instance.FadeOutHighScoresTable(1, true);
            AnimationManager.instance.FadeFireCurtain(true);
            AnimationManager.instance.LowerStartButton(true);

            GameManager.instance.PlayAudio("missionStatementPart1");

            //StartCoroutine(WaitForInitialAnimationsAndRemoveHighScores(28));
        }

        public void OnEngage()
        {
            EngageGame();
        }

        public void EngageGame()
        {
            Debug.Log("EngageGame");
            GameManager.instance.PlayAudio("useKeyboard");
            AnimationManager.instance.OpenInputKeyboard(true);
            playerNameInputField.text = "";
            GameManager.instance.StartButtonText("Please enter your name");
        }

        public void OnKeyEnter()
        {
            var playerName = playerNameInputField.text;

            if (playerName != string.Empty)
            {
                AnimationManager.instance.OpenInputKeyboard(false);

                //var kb = GameObject.Find("InputKeyboard");
                //var slate = GameObject.Find("Slate");
                //Destroy(slate);
                //Destroy(kb);

                //var newPlayer = PlayerManager.instance.NewPlayer(playerName);
                PlayerManager.instance.Player = playerName;
                var newPlayer = PlayerManager.instance.GetCurrentPlayer();

                //Debug.Log($"Player: {newPlayer.PlayerName}, ID: {newPlayer.PlayerId}, Score: {newPlayer.CurrentScore}, Hi Score: {newPlayer.HighScore}.");

                GameManager.instance.PersonalMessage(playerName);

                GameManager.instance.StartButtonText($"Thank you {playerName}");

                AnimationManager.instance.RaiseCentrePartition(true);
                GameManager.instance.PlayAudio("missionStatementPart2");

                //var fireCurtain = GameObject.Find("FireCurtain");
                //fireCurtain.SetActive(false);

                StartCoroutine(RaiseButtonAfterMessageHasBeenRead(3));
            }
        }

        public void OnPlayAgain()
        {
            Debug.Log("Play Again");
            GameManager.instance.PlayAgain();
        }

        public void OnResetLeaderBoard()
        {
            GameManager.instance.ResetLeaderBoard();
        }

        //private IEnumerator WaitForInitialAnimationsAndRemoveHighScores(int delaySecs)
        //{
        //    // If not removed then it obscures the 'start' button
        //    yield return new WaitForSeconds(delaySecs);

        //    AnimationManager.instance.AnimateHighScoresTable();
        //}

        private IEnumerator RaiseButtonAfterMessageHasBeenRead(int secondsDelay)
        {
            yield return new WaitForSeconds(secondsDelay);
            AnimationManager.instance.LowerStartButton(false);
        }
    }
}
