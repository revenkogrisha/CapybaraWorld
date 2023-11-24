using Core.Common;
using Core.Game;
using Core.Infrastructure;
using TMPro;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        private const string ScoreFormat = "Current Score: \n {0}";
        private const string HighestScoreFormat = "Highest Score: \n {0}";

        [Header("Buttons")]
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _menuButton;

        [Header("Texts")]
        [SerializeField] private TMP_Text _scoreTMP;
        [SerializeField] private TMP_Text _highestScoreTMP;

        private IGlobalStateMachine _globalStateMachine;
        private Score _score;

        #region MonoBehaviour

        private void OnEnable()
        {
            _restartButton.OnClicked += RestartGame;
            _menuButton.OnClicked += ReturnToMenu;
        }

        private void Start() =>
            SetScoreTexts();

        private void OnDisable()
        {
            _restartButton.OnClicked -= RestartGame;
            _menuButton.OnClicked -= ReturnToMenu;
        }

        #endregion

        [Inject]
        private void Construct(IGlobalStateMachine globalStateMachine, Score score)
        {
            _globalStateMachine = globalStateMachine;
            _score = score;
        }

        private void SetScoreTexts()
        {
            string scoreText = string.Format(ScoreFormat, _score.PlaythroughScore.Value);
            string highestScoreText = string.Format(HighestScoreFormat, _score.HighestScore);

            _scoreTMP.text = scoreText;
            _highestScoreTMP.SetText(highestScoreText);
        }

        private void RestartGame()
        {
            GameplayState gameplayState = _globalStateMachine.GetState<GameplayState>();
            GenerationState generationState = _globalStateMachine
                .GetStateWithArg<GenerationState, State>(gameplayState);

            _globalStateMachine.ChangeState(generationState);
        }

        private void ReturnToMenu()
        {
            MainMenuState mainMenuState = _globalStateMachine.GetState<MainMenuState>();
            GenerationState generationState = _globalStateMachine
                .GetStateWithArg<GenerationState, State>(mainMenuState);

            _globalStateMachine.ChangeState(generationState);
        }
    }
}
